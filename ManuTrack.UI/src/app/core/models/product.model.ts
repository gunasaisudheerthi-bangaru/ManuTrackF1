// Matches ProductViewModel.cs in ProductService
export interface ProductViewModel {
  productID: number;
  name: string;
  category: string;
  version: string;
  status: string;         // Draft | Active | Discontinued
  description?: string;
  createdDate: string;
  modifiedDate?: string;
}

export interface CreateProductRequest {
  name: string;
  category: string;
  version?: string;
  description?: string;
}

// Matches BomViewModel.cs in ProductService
export interface BomViewModel {
  bOMID: number;
  productID: number;
  productName: string;
  componentID: number;
  componentName: string;
  quantity: number;
  version: string;
  status: string;
  notes?: string;
  createdDate: string;
}

export interface CreateBomRequest {
  productID: number;
  componentID: number;
  quantity: number;
  version?: string;
  notes?: string;
}
