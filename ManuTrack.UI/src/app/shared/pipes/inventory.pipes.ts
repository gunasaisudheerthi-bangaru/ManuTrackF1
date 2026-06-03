import { Pipe, PipeTransform } from '@angular/core';
import { PurchaseOrderViewModel } from '../../core/services/inventory.service';

@Pipe({ name: 'poCount', standalone: false, pure: false })
export class PoCountPipe implements PipeTransform {
  transform(orders: PurchaseOrderViewModel[], status: string): number {
    return (orders ?? []).filter(o => o.status === status).length;
  }
}

@Pipe({ name: 'poHasStatus', standalone: false, pure: false })
export class PoHasStatusPipe implements PipeTransform {
  transform(orders: PurchaseOrderViewModel[], status: string): boolean {
    return (orders ?? []).some(o => o.status === status);
  }
}

@Pipe({ name: 'supplierPoCount', standalone: false, pure: false })
export class SupplierPoCountPipe implements PipeTransform {
  transform(orders: PurchaseOrderViewModel[], supplierName: string): number {
    return (orders ?? []).filter(o => o.supplierName === supplierName).length;
  }
}
