// Matches InventoryItemViewModel.cs in InventoryService
export interface InventoryViewModel {
  inventoryID: number;
  productID: number;
  productName: string;
  locationID: string;
  quantityOnHand: number;
  minimumQuantity: number;
  status: string;
  notes?: string;
  createdDate: string;
  modifiedDate?: string;
}

export interface AdjustQuantityRequest {
  inventoryID: number;
  quantity: number;         // positive = add, negative = subtract
  reason?: string;
}

// Matches PurchaseOrderViewModel.cs in InventoryService
export interface PurchaseOrderViewModel {
  pOID: number;
  supplierID: string;
  supplierName: string;
  orderDate: string;
  expectedDeliveryDate: string;
  status: string;
  totalAmount: number;
  notes?: string;
  createdDate: string;
  items: PurchaseOrderItemViewModel[];
}

export interface PurchaseOrderItemViewModel {
  itemID: number;
  productID: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface CreatePurchaseOrderRequest {
  supplierID: string;
  supplierName: string;
  expectedDeliveryDate: string;
  notes?: string;
  items: CreatePurchaseOrderItemRequest[];
}

export interface CreatePurchaseOrderItemRequest {
  productID: number;
  quantity: number;
  unitPrice: number;
}
