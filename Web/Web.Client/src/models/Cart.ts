import { CartItem } from './CartItem';

export interface Cart {
  totalCount: number;
  totalPrice: number;
  items: CartItem[];
}
