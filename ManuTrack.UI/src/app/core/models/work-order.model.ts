// Matches WorkOrderViewModel.cs in WorkOrderService
export interface WorkOrderViewModel {
  workOrderID: number;
  productID: number;
  productName: string;
  quantity: number;
  startDate: string;
  endDate: string;
  status: string;
  assignedTo?: string;
  notes?: string;
  createdDate: string;
  modifiedDate?: string;
  taskCount: number;
}

// Matches WorkOrderTaskViewModel.cs in WorkOrderService
export interface WorkOrderTaskViewModel {
  taskID: number;
  workOrderID: number;
  description: string;
  assignedTo: string;
  status: string;
  completedDate?: string;
  notes?: string;
  createdDate: string;
}

export interface CreateWorkOrderRequest {
  productID: number;
  quantity: number;
  startDate: string;
  endDate: string;
  assignedTo?: string;
  notes?: string;
}

export interface UpdateWorkOrderStatusRequest {
  status: string;
}

export interface CreateWorkOrderTaskRequest {
  workOrderID: number;
  description: string;
  assignedTo: string;
}

export interface UpdateTaskStatusRequest {
  status: string;
}
