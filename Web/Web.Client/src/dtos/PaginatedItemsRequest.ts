export interface PaginatedItemsRequest {
  pageIndex: number;
  pageSize: number;
  brandIdFilter: number[] | null;
  typeIdFilter: number[] | null;
}
