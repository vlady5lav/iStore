import 'reflect-metadata';

import { inject, injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';

import { IoCTypes } from 'ioc';
import type { Brand } from 'models';
import type { ProductsService } from 'services';

@injectable()
export default class BrandsStore {
  @inject(IoCTypes.productsService)
  private readonly productsService!: ProductsService;

  public brands: Brand[] = [];
  public selectedBrandIds: number[] = [];
  public isLoading = false;

  constructor() {
    const urlParameters = new URLSearchParams(window.location.search);
    const brands = urlParameters.get('brands');
    this.selectedBrandIds = brands ? brands.split(',').map(Number) : [];
    this.brands = [];
    this.isLoading = false;
    makeAutoObservable(this);
  }

  public getBrands = async (): Promise<void> => {
    try {
      this.isLoading = true;

      if (this.brands.length === 0) {
        this.brands = await this.productsService.getBrands();
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

  public changeBrandIds = (brand: number[]): void => {
    this.selectedBrandIds = brand;
  };
}
