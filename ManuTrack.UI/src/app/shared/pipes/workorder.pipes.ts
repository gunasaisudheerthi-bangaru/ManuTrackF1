import { Pipe, PipeTransform } from '@angular/core';
import { WorkOrderViewModel } from '../../core/services/workorder.service';

@Pipe({ name: 'woCount', standalone: false, pure: false })
export class WoCountPipe implements PipeTransform {
  transform(orders: WorkOrderViewModel[], status: string): number {
    return (orders ?? []).filter(o => o.status === status).length;
  }
}
