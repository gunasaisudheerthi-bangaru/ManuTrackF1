import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  standalone: false
})
export class SidebarComponent implements OnInit {
  role = '';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.role = this.authService.getRole() || '';
  }
}
