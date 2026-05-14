import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../core/services/notification.service';
import { NotificationViewModel } from '../../../core/models/notification.model';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  standalone: false
})
export class AlertsComponent implements OnInit {
  notifications: NotificationViewModel[] = [];
  loading = false;
  errorMessage = '';
  filterCategory = '';
  showUnreadOnly = false;

  categories = ['WorkOrder', 'Inventory', 'Quality', 'Compliance', 'General'];

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {
    this.loading = true;
    this.notificationService.getMyNotifications().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.notifications = res.data;
        else this.errorMessage = res.message;
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load notifications.'; }
    });
  }

  get filteredNotifications(): NotificationViewModel[] {
    return this.notifications.filter(n => {
      const matchCat = !this.filterCategory || n.category === this.filterCategory;
      const matchUnread = !this.showUnreadOnly || n.status === 'Unread';
      return matchCat && matchUnread;
    });
  }

  markRead(n: NotificationViewModel): void {
    if (n.status === 'Read') return;
    this.notificationService.markAsRead(n.notificationID).subscribe({
      next: res => { if (res.success) n.status = 'Read'; }
    });
  }

  markAllRead(): void {
    this.notificationService.markAllAsRead().subscribe({
      next: res => {
        if (res.success) this.notifications.forEach(n => n.status = 'Read');
      }
    });
  }

  getCategoryClass(cat: string): string {
    const map: Record<string, string> = {
      WorkOrder: 'bg-primary', Inventory: 'bg-info text-dark',
      Quality: 'bg-warning text-dark', Compliance: 'bg-danger', General: 'bg-secondary'
    };
    return map[cat] || 'bg-dark';
  }

  get unreadCount(): number { return this.notifications.filter(n => n.status === 'Unread').length; }
}
