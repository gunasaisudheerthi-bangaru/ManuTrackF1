import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { timeout, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  standalone: false
})
export class NavbarComponent implements OnInit {
  userName = '';
  userRole = '';
  unreadCount = 0;

  constructor(
    private authService: AuthService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userName = this.authService.getName() || '';
    this.userRole = this.authService.getRole() || '';
    this.loadUnreadCount();
  }

  loadUnreadCount(): void {
    this.notificationService.getUnreadCount().pipe(
      timeout(4000),
      catchError(() => of({ success: false, data: 0, message: '', errors: [] }))
    ).subscribe(res => {
      if (res.success) this.unreadCount = res.data ?? 0;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
