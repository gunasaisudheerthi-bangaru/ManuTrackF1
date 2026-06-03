import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { AdminComponent } from './features/admin/admin.component';
import { PlannerComponent } from './features/planner/planner.component';
import { OperatorComponent } from './features/operator/operator.component';
import { InventoryManagerComponent } from './features/inventory-manager/inventory-manager.component';
import { QualityInspectorComponent } from './features/quality-inspector/quality-inspector.component';
import { ComplianceOfficerComponent } from './features/compliance-officer/compliance-officer.component';
import { AuthGuard } from './core/guards/auth.guard';
import { NoAuthGuard } from './core/guards/no-auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  { path: 'admin',               component: AdminComponent,              canActivate: [AuthGuard] },
  { path: 'planner',             component: PlannerComponent,            canActivate: [AuthGuard] },
  { path: 'operator',            component: OperatorComponent,           canActivate: [AuthGuard] },
  { path: 'inventory-manager',   component: InventoryManagerComponent,   canActivate: [AuthGuard] },
  { path: 'quality-inspector',   component: QualityInspectorComponent,   canActivate: [AuthGuard] },
  { path: 'compliance-officer',  component: ComplianceOfficerComponent,  canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
