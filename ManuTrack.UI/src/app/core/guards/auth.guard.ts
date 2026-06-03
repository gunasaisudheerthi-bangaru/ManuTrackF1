import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

const ROLE_ROUTES: Record<string, string> = {
  'Admin':             '/admin',
  'Planner':           '/planner',
  'Operator':          '/operator',
  'InventoryManager':  '/inventory-manager',
  'Inspector':         '/quality-inspector',
  'ComplianceOfficer': '/compliance-officer'
};

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login'], { replaceUrl: true });
      return false;
    }

    const role = this.auth.getRole() ?? '';
    const allowedRoute = ROLE_ROUTES[role];
    const requestedPath = '/' + route.routeConfig?.path;

    // If user tries to access another role's dashboard, redirect to their own
    if (allowedRoute && requestedPath !== allowedRoute) {
      this.router.navigate([allowedRoute], { replaceUrl: true });
      return false;
    }

    return true;
  }
}
