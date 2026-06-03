import { Pipe, PipeTransform } from '@angular/core';
import { ComplianceReportViewModel } from '../../core/services/compliance.service';

@Pipe({ name: 'hasStatus', standalone: false, pure: false })
export class HasStatusPipe implements PipeTransform {
  transform(reports: ComplianceReportViewModel[], status: string): boolean {
    return (reports ?? []).some(r => r.status === status);
  }
}
