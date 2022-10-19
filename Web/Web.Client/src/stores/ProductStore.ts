import 'reflect-metadata';

import { inject, injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';

import { IoCTypes } from 'ioc';
import type { Product } from 'models';
import type { ProductsService } from 'services';

@injectable()
export default class ProductStore {
  @inject(IoCTypes.productsService)
  private readonly productsService!: ProductsService;

  public product: Product | undefined = undefined;

  public isLoading = false;

  constructor() {
    this.product = undefined;
    this.isLoading = false;
    makeAutoObservable(this);
  }

  public getById = async (id: number): Promise<Product | undefined> => {
    if (this.product?.id === id) {
      return this.product;
    }

    if (new RegExp(/\/products\/\d+/).test(window.location.pathname)) {
      this.isLoading = true;
    }

    try {
      this.product = await this.productsService.getById(id);
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    this.isLoading = false;

    return this.product;
  };
}
