import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

// PrimeNG imports
import { ButtonModule } from 'primeng/button';

import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.models';
import { CommonHeaderComponent } from '../common-header/common-header';

@Component({
  selector: 'app-admin',
  imports: [
    CommonModule,
    ButtonModule,
    CommonHeaderComponent
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class Admin implements OnInit {
  readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  user: User | null = null;

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.user = user;
    });
  }

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
