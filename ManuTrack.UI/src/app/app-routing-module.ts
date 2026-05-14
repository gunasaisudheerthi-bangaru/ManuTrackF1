import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';
import { ShellComponent } from './shared/shell/shell.component';

import { LoginComponent } from './pages/login/login.component';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';

import { UsersComponent } from './pages/admin/users/users.component';
import { ProductsComponent } from './pages/admin/products/products.component';
import { BomComponent } from './pages/admin/bom/bom.component';
import { SystemConfigComponent } from './pages/admin/system-config/system-config.component';

import { WorkOrdersComponent } from './pages/planner/work-orders/work-orders.component';
import { ScheduleComponent } from './pages/planner/schedule/schedule.component';
import { ProgressComponent } from './pages/planner/progress/progress.component';

import { TasksComponent } from './pages/operator/tasks/tasks.component';
import { ProgressUpdateComponent } from './pages/operator/progress-update/progress-update.component';
import { IssueReportComponent } from './pages/operator/issue-report/issue-report.component';

import { InspectionsComponent } from './pages/inspector/inspections/inspections.component';
import { DefectsComponent } from './pages/inspector/defects/defects.component';
import { QualityReportsComponent } from './pages/inspector/reports/quality-reports.component';

import { StockComponent } from './pages/inventory/stock/stock.component';
import { PurchaseOrdersComponent } from './pages/inventory/purchase-orders/purchase-orders.component';
import { ReconciliationComponent } from './pages/inventory/reconciliation/reconciliation.component';

import { AuditLogsComponent } from './pages/compliance/audit-logs/audit-logs.component';
import { ComplianceReportsComponent } from './pages/compliance/reports/compliance-reports.component';

import { AnalyticsDashboardComponent } from './pages/analytics/dashboard/analytics-dashboard.component';

import { AlertsComponent } from './pages/notifications/alerts/alerts.component';
import { NotificationHistoryComponent } from './pages/notifications/history/notification-history.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },

  {
    path: '',
    component: ShellComponent,
    canActivate: [AuthGuard],
    children: [
      // Analytics
      { path: 'analytics', component: AnalyticsDashboardComponent },

      // Notifications
      { path: 'notifications/alerts', component: AlertsComponent },
      { path: 'notifications/history', component: NotificationHistoryComponent },

      // Admin
      { path: 'admin/users', component: UsersComponent, canActivate: [RoleGuard], data: { roles: ['Admin'] } },
      { path: 'admin/products', component: ProductsComponent, canActivate: [RoleGuard], data: { roles: ['Admin'] } },
      { path: 'admin/bom', component: BomComponent, canActivate: [RoleGuard], data: { roles: ['Admin'] } },
      { path: 'admin/system-config', component: SystemConfigComponent, canActivate: [RoleGuard], data: { roles: ['Admin'] } },

      // Planner
      { path: 'planner/work-orders', component: WorkOrdersComponent, canActivate: [RoleGuard], data: { roles: ['Planner', 'Admin'] } },
      { path: 'planner/schedule', component: ScheduleComponent, canActivate: [RoleGuard], data: { roles: ['Planner', 'Admin'] } },
      { path: 'planner/progress', component: ProgressComponent, canActivate: [RoleGuard], data: { roles: ['Planner', 'Admin'] } },

      // Operator
      { path: 'operator/tasks', component: TasksComponent, canActivate: [RoleGuard], data: { roles: ['Operator', 'Admin'] } },
      { path: 'operator/progress-update', component: ProgressUpdateComponent, canActivate: [RoleGuard], data: { roles: ['Operator', 'Admin'] } },
      { path: 'operator/issue-report', component: IssueReportComponent, canActivate: [RoleGuard], data: { roles: ['Operator', 'Admin'] } },

      // Inspector
      { path: 'inspector/inspections', component: InspectionsComponent, canActivate: [RoleGuard], data: { roles: ['Inspector', 'Admin'] } },
      { path: 'inspector/defects', component: DefectsComponent, canActivate: [RoleGuard], data: { roles: ['Inspector', 'Admin'] } },
      { path: 'inspector/reports', component: QualityReportsComponent, canActivate: [RoleGuard], data: { roles: ['Inspector', 'Admin'] } },

      // Inventory
      { path: 'inventory/stock', component: StockComponent, canActivate: [RoleGuard], data: { roles: ['InventoryManager', 'Admin'] } },
      { path: 'inventory/purchase-orders', component: PurchaseOrdersComponent, canActivate: [RoleGuard], data: { roles: ['InventoryManager', 'Admin'] } },
      { path: 'inventory/reconciliation', component: ReconciliationComponent, canActivate: [RoleGuard], data: { roles: ['InventoryManager', 'Admin'] } },

      // Compliance
      { path: 'compliance/audit-logs', component: AuditLogsComponent, canActivate: [RoleGuard], data: { roles: ['ComplianceOfficer', 'Admin'] } },
      { path: 'compliance/reports', component: ComplianceReportsComponent, canActivate: [RoleGuard], data: { roles: ['ComplianceOfficer', 'Admin'] } },
    ]
  },

  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
