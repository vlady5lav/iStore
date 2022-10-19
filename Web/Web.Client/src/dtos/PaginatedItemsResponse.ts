import { ProductDto } from './ProductDto';

export interface PaginatedItemsResponse {
  data: ProductDto[];
  total_pages: number;
}
