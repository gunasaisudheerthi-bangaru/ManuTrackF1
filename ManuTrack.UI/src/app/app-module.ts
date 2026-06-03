import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

// ── Features ───────────────────────────────────────────
import { LoginComponent } from './features/auth/login/login.component';
import { AdminComponent } from './features/admin/admin.component';
import { PlannerComponent } from './features/planner/planner.component';
import { OperatorComponent } from './features/operator/operator.component';
import { InventoryManagerComponent } from './features/inventory-manager/inventory-manager.component';
import { QualityInspectorComponent } from './features/quality-inspector/quality-inspector.component';
import { ComplianceOfficerComponent } from './features/compliance-officer/compliance-officer.component';

// ── Shared Pipes ───────────────────────────────────────
import { PoCountPipe, PoHasStatusPipe, SupplierPoCountPipe } from './shared/pipes/inventory.pipes';
import { StatusCountPipe, SeverityCountPipe, WoInspCountPipe } from './shared/pipes/quality.pipes';
import { HasStatusPipe } from './shared/pipes/compliance.pipes';
import { WoCountPipe } from './shared/pipes/workorder.pipes';

// ── Core ───────────────────────────────────────────────
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

@NgModule({
  declarations: [
    App,
    // Features
    LoginComponent,
    AdminComponent,
    PlannerComponent,
    OperatorComponent,
    InventoryManagerComponent,
    QualityInspectorComponent,
    ComplianceOfficerComponent,
    // Pipes
    PoCountPipe, PoHasStatusPipe, SupplierPoCountPipe,
    StatusCountPipe, SeverityCountPipe, WoInspCountPipe,
    HasStatusPipe,
    WoCountPipe
  ],
  imports: [
    BrowserModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }
