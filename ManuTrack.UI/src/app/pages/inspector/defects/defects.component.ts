import { Component, OnInit } from '@angular/core';
import { QualityService } from '../../../core/services/quality.service';
import { DefectViewModel, CreateDefectRequest } from '../../../core/models/quality.model';

@Component({
  selector: 'app-defects',
  templateUrl: './defects.component.html',
  standalone: false
})
export class DefectsComponent implements OnInit {
  defects: DefectViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showForm = false;

  form: CreateDefectRequest = {
    inspectionID: 0,
    description: '',
    severity: 'Low'
  };

  severities = ['Low', 'Medium', 'High', 'Critical'];

  constructor(private qualityService: QualityService) {}

  ngOnInit(): void {
    this.loadDefects();
  }

  loadDefects(): void {
    this.loading = true;
    this.qualityService.getDefects().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.defects = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load defects.'; }
    });
  }

  save(): void {
    this.qualityService.createDefect(this.form).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Defect logged successfully.';
          this.showForm = false;
          this.loadDefects();
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to log defect.'; }
    });
  }

  getSeverityClass(severity: string): string {
    const map: Record<string, string> = {
      Low: 'bg-warning text-dark', Medium: 'bg-orange text-white',
      High: 'bg-danger', Critical: 'bg-dark'
    };
    return map[severity] || 'bg-secondary';
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Open: 'bg-danger', InProgress: 'bg-warning text-dark',
      Resolved: 'bg-success', Closed: 'bg-secondary'
    };
    return map[status] || 'bg-dark';
  }
}
