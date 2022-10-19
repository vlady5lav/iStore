import 'reflect-metadata';

import { inject, injectable } from 'inversify';

import { CartDto } from 'dtos';
import { IoCTypes } from 'ioc';
import { HttpService } from 'services';
import { ApiHeader, ApiResponse, ContentType, MethodType } from 'services/HttpService';
import { AuthStore } from 'stores';

export interface CartService {
  getCart(): Promise<ApiResponse<CartDto>>;
  updateCart(data: CartDto): Promise<void>;
  deleteCart(): Promise<void>;
}

@injectable()
export default class DefaultCartService implements CartService {
  @inject(IoCTypes.authStore)
  private readonly authStore!: AuthStore;

  @inject(IoCTypes.httpService)
  private readonly httpService!: HttpService;

  private readonly basketUrl: string = `${process.env.REACT_APP_BASKET_API_URL}`;

  private readonly basketRoute: string = `${this.basketUrl}${process.env.REACT_APP_BASKET_CONTROLLER_ROUTE}`;

  private headers: ApiHeader = {
    contentType: undefined,
    authorization: undefined,
    accessControlAllowOrigin: undefined,
  };

  public getAuthorizationHeaders(): void {
    this.headers = {
      contentType: ContentType.Json,
      authorization: this.authStore.user?.access_token,
      accessControlAllowOrigin: this.basketUrl,
    };
  }

  public async getCart(): Promise<ApiResponse<CartDto>> {
    return await this.httpService.sendAsync<CartDto>(
      `${this.basketRoute}${process.env.REACT_APP_BASKET_GET_CART_ROUTE}`,
      MethodType.POST,
      this.headers
    );
  }

  public async updateCart(data: CartDto): Promise<void> {
    await this.httpService.sendAsync<unknown>(
      `${this.basketRoute}${process.env.REACT_APP_BASKET_UPDATE_CART_ROUTE}`,
      MethodType.POST,
      this.headers,
      data
    );
  }

  public async deleteCart(): Promise<void> {
    await this.httpService.sendAsync<unknown>(
      `${this.basketRoute}${process.env.REACT_APP_BASKET_DELETE_CART_ROUTE}`,
      MethodType.POST,
      this.headers
    );
  }
}
