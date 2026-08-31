import { Component, Input, Output, EventEmitter, inject, OnInit, computed } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.models';

@Component({
  selector: 'app-common-header',
  imports: [CommonModule, ButtonModule],
  templateUrl: './common-header.html',
  styleUrl: './common-header.scss'
})
export class CommonHeaderComponent {
  @Input() title: string = 'BlanchisserieStart';
  @Input() brandIcon: string = 'pi pi-home';
  @Input() showAdminBadge: boolean = false;
  @Input() showDashboardButton: boolean = false;
  @Input() user: User | null = null;
  
  readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  userRole = computed(() => this.user?.roles?.[0]);

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  goToAdminOrder(): void {
    this.router.navigate(['/admin']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
