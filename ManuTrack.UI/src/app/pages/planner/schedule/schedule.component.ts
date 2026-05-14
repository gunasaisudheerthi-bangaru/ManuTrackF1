import { Component, OnInit } from '@angular/core';
import { WorkOrderService } from '../../../core/services/work-order.service';
import { WorkOrderViewModel } from '../../../core/models/work-order.model';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  standalone: false
})
export class ScheduleComponent implements OnInit {
  workOrders: WorkOrderViewModel[] = [];
  loading = false;
  errorMessage = '';
  currentWeekStart: Date = new Date();
  weekDays: Date[] = [];

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.setWeekStart(new Date());
    this.loadOrders();
  }

  setWeekStart(date: Date): void {
    const d = new Date(date);
    const day = d.getDay();
    const diff = d.getDate() - day + (day === 0 ? -6 : 1);
    d.setDate(diff);
    d.setHours(0, 0, 0, 0);
    this.currentWeekStart = d;
    this.weekDays = Array.from({ length: 7 }, (_, i) => {
      const wd = new Date(d);
      wd.setDate(d.getDate() + i);
      return wd;
    });
  }

  loadOrders(): void {
    this.loading = true;
    this.workOrderService.getAll().subscribe({
      next: res => {
        this.loading = false;
        if (res.success) this.workOrders = res.data.filter(o => o.status !== 'Cancelled');
      },
      error: () => { this.loading = false; this.errorMessage = 'Failed to load schedule.'; }
    });
  }

  prevWeek(): void {
    const d = new Date(this.currentWeekStart);
    d.setDate(d.getDate() - 7);
    this.setWeekStart(d);
  }

  nextWeek(): void {
    const d = new Date(this.currentWeekStart);
    d.setDate(d.getDate() + 7);
    this.setWeekStart(d);
  }

  getOrdersForDay(day: Date): WorkOrderViewModel[] {
    return this.workOrders.filter(wo => {
      if (!wo.startDate) return false;
      const start = new Date(wo.startDate);
      const end = wo.endDate ? new Date(wo.endDate) : start;
      start.setHours(0, 0, 0, 0);
      end.setHours(23, 59, 59, 999);
      return day >= start && day <= end;
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      Planned: 'bg-secondary', InProgress: 'bg-primary',
      Completed: 'bg-success', OnHold: 'bg-warning text-dark'
    };
    return map[status] || 'bg-dark';
  }

  isToday(day: Date): boolean {
    const today = new Date();
    return day.toDateString() === today.toDateString();
  }
}
