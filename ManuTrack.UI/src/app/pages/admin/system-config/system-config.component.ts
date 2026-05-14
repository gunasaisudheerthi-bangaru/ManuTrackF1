import { Component } from '@angular/core';

@Component({
  selector: 'app-system-config',
  templateUrl: './system-config.component.html',
  standalone: false
})
export class SystemConfigComponent {
  configs = [
    { key: 'Max Work Orders Per Day', value: '50', group: 'Production' },
    { key: 'Default Lead Time (days)', value: '7', group: 'Production' },
    { key: 'Low Stock Threshold (%)', value: '20', group: 'Inventory' },
    { key: 'Auto Reorder Enabled', value: 'true', group: 'Inventory' },
    { key: 'Audit Log Retention (days)', value: '365', group: 'Compliance' },
    { key: 'Report Generation Schedule', value: 'Daily at 06:00', group: 'Compliance' },
    { key: 'Notification Expiry (days)', value: '30', group: 'Notifications' },
    { key: 'Max Broadcast Recipients', value: '500', group: 'Notifications' },
  ];

  editingKey: string | null = null;
  editValue = '';

  startEdit(config: any): void {
    this.editingKey = config.key;
    this.editValue = config.value;
  }

  saveEdit(config: any): void {
    config.value = this.editValue;
    this.editingKey = null;
  }

  cancelEdit(): void {
    this.editingKey = null;
  }

  get groups(): string[] {
    return [...new Set(this.configs.map(c => c.group))];
  }

  getConfigsByGroup(group: string) {
    return this.configs.filter(c => c.group === group);
  }
}
