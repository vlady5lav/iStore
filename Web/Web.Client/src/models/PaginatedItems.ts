import { Product } from './Product';

export interface PaginatedItems {
  data: Product[];
  total_pages: number;
}
