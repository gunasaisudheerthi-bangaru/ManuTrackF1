import { Pipe, PipeTransform } from '@angular/core';
import { InspectionViewModel, DefectViewModel } from '../../core/services/quality.service';

@Pipe({ name: 'statusCount', standalone: false, pure: false })
export class StatusCountPipe implements PipeTransform {
  transform(items: (InspectionViewModel | DefectViewModel)[], value: string): number {
    return (items ?? []).filter((i: any) =>
      i.status === value || i.result === value
    ).length;
  }
}

@Pipe({ name: 'severityCount', standalone: false, pure: false })
export class SeverityCountPipe implements PipeTransform {
  transform(defects: DefectViewModel[], severity: string): number {
    return (defects ?? []).filter(d => d.severity === severity && d.status !== 'Closed').length;
  }
}

@Pipe({ name: 'woInspCount', standalone: false, pure: false })
export class WoInspCountPipe implements PipeTransform {
  transform(inspections: InspectionViewModel[], workOrderId: number): string {
    const count = (inspections ?? []).filter(i => i.workOrderID === workOrderId).length;
    return count > 0 ? `${count} inspection${count !== 1 ? 's' : ''}` : 'â€”';
  }
}
