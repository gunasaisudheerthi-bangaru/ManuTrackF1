import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../core/services/notification.service';
import { NotificationViewModel, SendNotificationRequest } from '../../../core/models/notification.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-notification-history',
  templateUrl: './notification-history.component.html',
  standalone: false
})
export class NotificationHistoryComponent implements OnInit {
  notifications: NotificationViewModel[] = [];
  loading = false;
  errorMessage = '';
  successMessage = '';
  showSendForm = false;
  isAdmin = false;

  sendForm: SendNotificationRequest = {
    userID: '', title: '', message: '', category: 'General'
  };

  categories = ['WorkOrder', 'Inventory', 'Quality', 'Compliance', 'General'];

  constructor(
    private notificationService: NotificationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.isAdmin = this.authService.getRole() === 'Admin';
    this.loadHistory();
  }

  loadHistory(): void {
    this.loading = true;
    this.notificationService.getMyNotifications().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.notifications = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load history.'; }
    });
  }

  send(): void {
    this.notificationService.send(this.sendForm).subscribe({
      next: res => {
        if (res.success) {
          this.successMessage = 'Notification sent successfully.';
          this.showSendForm = false;
          this.sendForm = { userID: '', title: '', message: '', category: 'General' };
          setTimeout(() => this.successMessage = '', 3000);
        } else { this.errorMessage = res.message; }
      },
      error: () => { this.errorMessage = 'Failed to send notification.'; }
    });
  }

  getCategoryClass(cat: string): string {
    const map: Record<string, string> = {
      WorkOrder: 'bg-primary', Inventory: 'bg-info text-dark',
      Quality: 'bg-warning text-dark', Compliance: 'bg-danger', General: 'bg-secondary'
    };
    return map[cat] || 'bg-dark';
  }
}
