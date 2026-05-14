import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: false
})
export class LoginComponent {
  email = '';
  password = '';
  errorMessage = '';
  loading = false;

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.errorMessage = '';
    this.loading = true;
    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: res => {
        this.loading = false;
        if (res.success) {
          const role = res.data.role;
          const routes: Record<string, string> = {
            Admin: '/admin/users',
            Planner: '/planner/work-orders',
            Operator: '/operator/tasks',
            Inspector: '/inspector/inspections',
            InventoryManager: '/inventory/stock',
            ComplianceOfficer: '/compliance/audit-logs'
          };
          this.router.navigate([routes[role] || '/analytics']);
        } else {
          this.errorMessage = res.message;
        }
      },
      error: err => {
        this.loading = false;
        this.errorMessage = err.error?.message || 'Login failed. Please try again.';
      }
    });
  }
}
