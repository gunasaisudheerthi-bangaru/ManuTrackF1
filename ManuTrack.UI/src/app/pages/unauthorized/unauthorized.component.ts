import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-unauthorized',
  template: `
    <div class="min-vh-100 d-flex align-items-center justify-content-center">
      <div class="text-center">
        <i class="bi bi-shield-exclamation text-danger" style="font-size:4rem"></i>
        <h3 class="mt-3">Access Denied</h3>
        <p class="text-muted">You don't have permission to view this page.</p>
        <button class="btn btn-primary mt-2" (click)="goBack()">Go to Dashboard</button>
      </div>
    </div>
  `,
  standalone: false
})
export class UnauthorizedComponent {
  constructor(private authService: AuthService, private router: Router) {}

  goBack(): void {
    const role = this.authService.getRole();
    const routes: Record<string, string> = {
      Admin: '/admin/users', Planner: '/planner/work-orders',
      Operator: '/operator/tasks', Inspector: '/inspector/inspections',
      InventoryManager: '/inventory/stock', ComplianceOfficer: '/compliance/audit-logs'
    };
    this.router.navigate([routes[role || ''] || '/analytics']);
  }
}
