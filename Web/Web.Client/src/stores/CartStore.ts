import 'reflect-metadata';

import { inject, injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';

import { CartDto } from 'dtos';
import { IoCTypes } from 'ioc';
import { Cart } from 'models';
import type { CartService } from 'services';
import { AuthStore, ProductStore } from 'stores';

@injectable()
export default class CartStore {
  @inject(IoCTypes.authStore)
  private readonly authStore!: AuthStore;

  @inject(IoCTypes.productStore)
  private readonly productStore!: ProductStore;

  @inject(IoCTypes.cartService)
  private readonly cartService!: CartService;

  cartDto: CartDto = { data: '{}' };
  cart: Cart = { items: [], totalCount: 0, totalPrice: 0 };
  currentCount: number | undefined = undefined;
  isLoading = false;

  constructor() {
    this.cartDto = { data: '{}' };
    this.cart = { items: [], totalCount: 0, totalPrice: 0 };
    this.currentCount = undefined;
    this.isLoading = true;
    makeAutoObservable(this);
  }

  public getCount = (id: number): number => {
    try {
      return this.cart.items.find((item) => item.id === id)?.count ?? 0;
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    return 0;
  };

  public setCount = async (id: number, count?: string | number): Promise<void> => {
    try {
      const item = this.cart.items?.find((ci) => ci.id === id);
      const parsedCount = count ? (typeof count === 'string' ? Number.parseInt(count) : count) : this.currentCount;

      if (item && parsedCount !== undefined && parsedCount > 0) {
        item.count = parsedCount;
        item.totalPrice = parsedCount * item.price;

        await this.updateCart();
      } else if (item && parsedCount !== undefined && parsedCount < 1) {
        await this.clearItem(id);
      } else {
        console.error(`There is no item with id ${id} in your cart to set the count!`);
      }

      this.currentCount = undefined;
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  public addItem = async (id: number): Promise<void> => {
    try {
      const item = this.cart.items?.find((ci) => ci.id === id);

      if (item) {
        item.count += 1;
        item.totalPrice += item.price;
      } else {
        await this.pushItem(id);
      }

      await this.updateCart();
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  public removeItem = async (id: number): Promise<void> => {
    try {
      let index = -1;

      const item = this.cart.items?.find((ci, idx) => {
        index = idx;

        return ci.id === id;
      });

      if (item) {
        if (item.count > 1) {
          item.count -= 1;
          item.totalPrice -= item.price;
        } else {
          this.cart.items.splice(index, 1);
        }

        await this.updateCart();
      } else {
        console.error(`There is no item with id ${id} in your cart to remove!`);
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  public clearItem = async (id: number): Promise<void> => {
    try {
      const index = this.cart.items?.findIndex((ci) => ci.id === id);

      if (index >= 0) {
        this.cart.items.splice(index, 1);

        await this.updateCart();
      } else {
        console.error(`There is no item with id ${id} in your cart to remove!`);
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  public getCart = async (): Promise<void> => {
    this.isLoading = true;

    try {
      if (this.authStore.user) {
        this.cartService.getAuthorizationHeaders();

        const response = await this.cartService.getCart();

        if (response.data) {
          this.cartDto = response.data;
        }

        const basket =
          this.cartDto.data && this.cartDto.data !== '{}'
            ? JSON.parse(this.cartDto.data)
            : { items: [], totalCount: 0, totalPrice: 0 };

        const localCart: string | null = localStorage.getItem('cart');
        const parsedCart: Cart | null = localCart ? JSON.parse(localCart) : null;

        if (parsedCart?.items) {
          this.cart = this.cartMerger(parsedCart, basket);
          this.cartDto = { data: '{}' };
          await this.updateCart();
          localStorage.removeItem('cart');
        } else {
          this.cart = basket;
          this.cartDto = { data: '{}' };
          localStorage.removeItem('cart');
        }
      } else {
        const localCart = localStorage.getItem('cart');
        const parsedCart = localCart
          ? JSON.parse(localCart) ?? { items: [], totalCount: 0, totalPrice: 0 }
          : { items: [], totalCount: 0, totalPrice: 0 };

        this.cart = parsedCart;
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    this.cartDto = { data: '{}' };
    this.isLoading = false;
  };

  public updateCart = async (): Promise<void> => {
    if (!this.authStore.user) {
      await this.authStore.getUser();
    }

    try {
      this.updateCartTotals();

      const cartString = JSON.stringify(this.cart);
      this.cartDto.data = cartString;

      if (this.authStore.user) {
        this.cartService.getAuthorizationHeaders();

        await this.cartService.updateCart(this.cartDto);
      } else {
        const parsedCart: string = JSON.stringify(this.cart);
        localStorage.setItem('cart', parsedCart);
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    this.cartDto = { data: '{}' };
  };

  public deleteCart = async (): Promise<void> => {
    if (!this.authStore.user) {
      await this.authStore.getUser();
    }

    try {
      if (this.authStore.user) {
        this.cartService.getAuthorizationHeaders();

        await this.cartService.deleteCart();
      } else {
        localStorage.removeItem('cart');
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    await this.getCart();
  };

  public updateCartTotals = (): void => {
    try {
      let totalPrice = 0;
      let totalCount = 0;

      for (const item of this.cart.items) {
        totalCount += item.count;
        totalPrice += item.price * item.count;
      }

      this.cart.totalCount = totalCount;
      this.cart.totalPrice = totalPrice;
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  private readonly pushItem = async (id: number): Promise<void> => {
    try {
      const product = await this.productStore.getById(id);

      if (product) {
        this.cart.items.push({
          count: 1,
          brand: product.catalogBrand.name,
          type: product.catalogType.name,
          id: product.id,
          picture: product.pictureUrl,
          price: product.price,
          name: product.name,
          totalPrice: product.price,
        });
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }
  };

  private readonly cartMerger = (cart1: Cart, cart2: Cart): Cart => {
    const newCart: Cart = { items: [], totalCount: 0, totalPrice: 0 };

    try {
      newCart.items = Object.assign(newCart.items, cart1.items);

      for (const item of cart2.items) {
        const index = newCart.items.findIndex((n) => n.id === item.id);

        if (index === -1) {
          newCart.items.push(item);
        }
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    }

    this.updateCartTotals();

    return newCart;
  };

  public changeCount = (count: number | string): void => {
    this.currentCount = typeof count === 'string' ? Number.parseInt(count) : count;
  };
}
