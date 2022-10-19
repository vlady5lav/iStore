export interface CartItem {
  id: number;
  brand: string;
  type: string;
  name: string;
  picture?: string;
  count: number;
  price: number;
  totalPrice: number;
}
