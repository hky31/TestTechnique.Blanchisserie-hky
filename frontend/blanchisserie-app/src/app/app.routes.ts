import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Dashboard } from './components/dashboard/dashboard';
import { Admin } from './components/admin/admin';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
import { OrderComponent } from './components/order/order';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: Login },
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [authGuard],
  },
  {
    path: 'order',
    component: OrderComponent,
    canActivate: [authGuard],
  },
  {
    path: 'admin',
    component: Admin,
    canActivate: [adminGuard],
  },
  { path: '**', redirectTo: '/login' },
];
