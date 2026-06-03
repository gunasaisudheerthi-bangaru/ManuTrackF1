import { Component, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  get email() { return this.loginForm.get('email')!; }
  get password() { return this.loginForm.get('password')!; }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    this.errorMessage = '';

    this.auth.login(this.loginForm.value)
      .pipe(finalize(() => { this.isLoading = false; this.cdr.detectChanges(); }))
      .subscribe({
        next: (res: any) => {
          if (res?.success) {
            const role = this.auth.getRole();
            const route = role === 'Admin' ? '/admin'
              : role === 'Planner' ? '/planner'
              : role === 'Operator' ? '/operator'
              : role === 'InventoryManager' ? '/inventory-manager'
              : role === 'Inspector' ? '/quality-inspector'
              : role === 'ComplianceOfficer' ? '/compliance-officer'
              : '/login';
            this.router.navigate([route], { replaceUrl: true });
          } else {
            this.errorMessage = res?.message || 'Login failed. Please try again.';
            this.cdr.detectChanges();
          }
        },
        error: (err: any) => {
          this.errorMessage = err?.error?.message || 'Invalid email or password.';
          this.cdr.detectChanges();
        }
      });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
