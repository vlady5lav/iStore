import 'reflect-metadata';

import { inject, injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';

import { IoCTypes } from 'ioc';
import type { Type } from 'models';
import type { ProductsService } from 'services';

@injectable()
export default class TypesStore {
  @inject(IoCTypes.productsService)
  private readonly productsService!: ProductsService;

  public types: Type[] = [];
  public selectedTypeIds: number[] = [];
  public isLoading = false;

  constructor() {
    const urlParameters = new URLSearchParams(window.location.search);
    const types = urlParameters.get('types');
    this.selectedTypeIds = types ? types.split(',').map(Number) : [];
    this.types = [];
    this.isLoading = false;
    makeAutoObservable(this);
  }

  public getTypes = async (): Promise<void> => {
    try {
      this.isLoading = true;

      if (this.types.length === 0) {
        this.types = await this.productsService.getTypes();
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    this.isLoading = false;
  };

  public changeTypeIds = (type: number[]): void => {
    this.selectedTypeIds = type;
  };
}
