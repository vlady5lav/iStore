export interface ProductResponse {
  id: number;
  name: string;
  availableStock: number;
  price: number;
  warranty: number;
  description?: string;
  pictureUrl?: string;
  catalogBrand: {
    id: number;
    name: string;
  };
  catalogType: {
    id: number;
    name: string;
  };
}
