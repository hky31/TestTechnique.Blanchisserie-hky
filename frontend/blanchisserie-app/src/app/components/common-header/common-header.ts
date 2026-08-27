import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.models';

@Component({
  selector: 'app-common-header',
  imports: [CommonModule, ButtonModule],
  template: `
    <header class="common-header">
      <div class="header-content">
        <div class="brand">
          <i [class]="brandIcon" class="mr-2"></i>
          <span class="brand-text">{{ title }}</span>
          <span *ngIf="showAdminBadge" class="admin-badge">
            <i class="pi pi-shield"></i>
            Admin
          </span>
        </div>
        
        <div class="header-actions">
          <span *ngIf="user" class="user-name">{{ user.firstName }} {{ user.lastName }}</span>
          
          <p-button 
            *ngIf="showDashboardButton"
            label="Dashboard" 
            icon="pi pi-home"
            [text]="true"
            severity="secondary"
            (onClick)="goToDashboard()">
          </p-button>
          
          <p-button 
            icon="pi pi-sign-out" 
            label="Déconnexion"
            [text]="true"
            severity="secondary"
            (onClick)="logout()"
            pTooltip="Se déconnecter"
            tooltipPosition="bottom">
          </p-button>
        </div>
      </div>
    </header>
  `,
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

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
