import { Component, OnInit } from '@angular/core';
import { ComplianceService } from '../../../core/services/compliance.service';
import { AuditLogViewModel } from '../../../core/models/compliance.model';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.component.html',
  standalone: false
})
export class AuditLogsComponent implements OnInit {
  logs: AuditLogViewModel[] = [];
  filteredLogs: AuditLogViewModel[] = [];
  loading = false;
  errorMessage = '';
  searchTerm = '';
  filterAction = '';
  filterEntityType = '';
  dateFrom = '';
  dateTo = '';

  actions = ['Create', 'Update', 'Delete', 'Login', 'Logout', 'Export', 'StatusChange'];
  entityTypes = ['Users', 'Products', 'WorkOrders', 'Inventory', 'Quality', 'Compliance', 'Auth'];

  constructor(private complianceService: ComplianceService) {}

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.loading = true;
    this.complianceService.getAuditLogs().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) { this.logs = res.data; this.applyFilter(); }
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load audit logs.'; }
    });
  }

  applyFilter(): void {
    this.filteredLogs = this.logs.filter(l => {
      const matchSearch = !this.searchTerm ||
        (l.userName || '').toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (l.details || '').toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchAction = !this.filterAction || l.action === this.filterAction;
      const matchEntity = !this.filterEntityType || l.entityType === this.filterEntityType;
      const matchFrom = !this.dateFrom || new Date(l.timestamp) >= new Date(this.dateFrom);
      const matchTo = !this.dateTo || new Date(l.timestamp) <= new Date(this.dateTo + 'T23:59:59');
      return matchSearch && matchAction && matchEntity && matchFrom && matchTo;
    });
  }

  clearFilters(): void {
    this.searchTerm = ''; this.filterAction = ''; this.filterEntityType = '';
    this.dateFrom = ''; this.dateTo = '';
    this.applyFilter();
  }

  getActionClass(action: string): string {
    const map: Record<string, string> = {
      Create: 'bg-success', Update: 'bg-primary', Delete: 'bg-danger',
      Login: 'bg-info text-dark', Logout: 'bg-secondary',
      Export: 'bg-warning text-dark', StatusChange: 'bg-dark'
    };
    return map[action] || 'bg-dark';
  }
}
