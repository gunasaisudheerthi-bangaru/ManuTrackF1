import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

// Shared Components
import { NavbarComponent } from './shared/navbar/navbar.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { ShellComponent } from './shared/shell/shell.component';

// Pages
import { LoginComponent } from './pages/login/login.component';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';

// Admin
import { UsersComponent } from './pages/admin/users/users.component';
import { ProductsComponent } from './pages/admin/products/products.component';
import { BomComponent } from './pages/admin/bom/bom.component';
import { SystemConfigComponent } from './pages/admin/system-config/system-config.component';

// Planner
import { WorkOrdersComponent } from './pages/planner/work-orders/work-orders.component';
import { ScheduleComponent } from './pages/planner/schedule/schedule.component';
import { ProgressComponent } from './pages/planner/progress/progress.component';

// Operator
import { TasksComponent } from './pages/operator/tasks/tasks.component';
import { ProgressUpdateComponent } from './pages/operator/progress-update/progress-update.component';
import { IssueReportComponent } from './pages/operator/issue-report/issue-report.component';

// Inspector
import { InspectionsComponent } from './pages/inspector/inspections/inspections.component';
import { DefectsComponent } from './pages/inspector/defects/defects.component';
import { QualityReportsComponent } from './pages/inspector/reports/quality-reports.component';

// Inventory
import { StockComponent } from './pages/inventory/stock/stock.component';
import { PurchaseOrdersComponent } from './pages/inventory/purchase-orders/purchase-orders.component';
import { ReconciliationComponent } from './pages/inventory/reconciliation/reconciliation.component';

// Compliance
import { AuditLogsComponent } from './pages/compliance/audit-logs/audit-logs.component';
import { ComplianceReportsComponent } from './pages/compliance/reports/compliance-reports.component';

// Analytics
import { AnalyticsDashboardComponent } from './pages/analytics/dashboard/analytics-dashboard.component';

// Notifications
import { AlertsComponent } from './pages/notifications/alerts/alerts.component';
import { NotificationHistoryComponent } from './pages/notifications/history/notification-history.component';

@NgModule({
  declarations: [
    App,
    NavbarComponent, SidebarComponent, ShellComponent,
    LoginComponent, UnauthorizedComponent,
    UsersComponent, ProductsComponent, BomComponent, SystemConfigComponent,
    WorkOrdersComponent, ScheduleComponent, ProgressComponent,
    TasksComponent, ProgressUpdateComponent, IssueReportComponent,
    InspectionsComponent, DefectsComponent, QualityReportsComponent,
    StockComponent, PurchaseOrdersComponent, ReconciliationComponent,
    AuditLogsComponent, ComplianceReportsComponent,
    AnalyticsDashboardComponent,
    AlertsComponent, NotificationHistoryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
