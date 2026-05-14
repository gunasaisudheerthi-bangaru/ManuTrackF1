import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { AuthUserViewModel, RegisterRequest } from '../../../core/models/auth.model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  standalone: false
})
export class UsersComponent implements OnInit {
  users: AuthUserViewModel[] = [];
  filteredUsers: AuthUserViewModel[] = [];
  searchTerm = '';
  selectedRole = '';
  loading = false;
  errorMessage = '';
  successMessage = '';
  showCreateForm = false;
  createLoading = false;

  roles = ['Admin', 'Planner', 'Operator', 'Inspector', 'InventoryManager', 'ComplianceOfficer'];

  form: RegisterRequest = {
    name: '', email: '', password: '', confirmPassword: '', role: '', phone: ''
  };

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.authService.getAllUsers().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) {
          this.users = res.data;
          this.applyFilter();
        } else {
          this.errorMessage = res.message;
        }
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Failed to load users.';
      }
    });
  }

  applyFilter(): void {
    this.filteredUsers = this.users.filter(u => {
      const matchesSearch = !this.searchTerm ||
        u.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        u.email.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesRole = !this.selectedRole || u.role === this.selectedRole;
      return matchesSearch && matchesRole;
    });
  }

  openCreate(): void {
    this.form = { name: '', email: '', password: '', confirmPassword: '', role: '', phone: '' };
    this.showCreateForm = true;
    this.errorMessage = '';
  }

  createUser(): void {
    if (!this.form.name || !this.form.email || !this.form.password || !this.form.role || !this.form.phone) {
      this.errorMessage = 'All fields are required.';
      return;
    }
    if (this.form.password !== this.form.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }
    this.createLoading = true;
    this.errorMessage = '';
    this.authService.register(this.form).subscribe({
      next: res => {
        this.createLoading = false;
        if (res.success) {
          this.successMessage = `User "${this.form.name}" created successfully.`;
          this.showCreateForm = false;
          this.loadUsers();
          setTimeout(() => this.successMessage = '', 4000);
        } else {
          this.errorMessage = res.message || 'Registration failed.';
        }
      },
      error: (err) => {
        this.createLoading = false;
        const msg = err?.error?.message || err?.error?.errors?.join(', ');
        this.errorMessage = msg || 'Failed to create user.';
      }
    });
  }

  toggleStatus(user: AuthUserViewModel): void {
    const action = user.isActive
      ? this.authService.deactivateUser(user.userID)
      : this.authService.activateUser(user.userID);

    action.subscribe({
      next: res => {
        if (res.success) {
          user.isActive = !user.isActive;
          this.successMessage = `User ${user.isActive ? 'activated' : 'deactivated'} successfully.`;
          setTimeout(() => this.successMessage = '', 3000);
        } else {
          this.errorMessage = res.message;
        }
      },
      error: () => { this.errorMessage = 'Action failed.'; }
    });
  }

  getRoleBadgeClass(role: string): string {
    const map: Record<string, string> = {
      Admin: 'bg-danger', Planner: 'bg-primary', Operator: 'bg-success',
      Inspector: 'bg-warning text-dark', InventoryManager: 'bg-info text-dark',
      ComplianceOfficer: 'bg-secondary'
    };
    return map[role] || 'bg-dark';
  }
}
