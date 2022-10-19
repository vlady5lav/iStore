import { ProductDto } from './ProductDto';

export interface PaginatedItemsDto {
  count: number;
  data: ProductDto[];
  pageIndex: number;
  pageSize: number;
}
