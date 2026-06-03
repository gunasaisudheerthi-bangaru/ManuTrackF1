import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class NoAuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.auth.isLoggedIn()) {
      this.router.navigate([this.getDashboardRoute()], { replaceUrl: true });
      return false;
    }
    return true;
  }

  private getDashboardRoute(): string {
    const role = this.auth.getRole();
    switch (role) {
      case 'Admin':              return '/admin';
      case 'Planner':            return '/planner';
      case 'Operator':           return '/operator';
      case 'InventoryManager':   return '/inventory-manager';
      case 'Inspector':          return '/quality-inspector';
      case 'ComplianceOfficer':  return '/compliance-officer';
      default:                   return '/login';
    }
  }
}
