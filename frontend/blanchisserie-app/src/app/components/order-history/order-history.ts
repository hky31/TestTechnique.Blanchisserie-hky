import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderResponse, OrderStatus } from '../../models/order.models';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.models';
import { OrderConfirmationModalComponent } from '../order-confirmation-modal/order-confirmation-modal';

@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [CommonModule, OrderConfirmationModalComponent],
  templateUrl: './order-history.html',
  styleUrl: './order-history.scss',
})
export class OrderHistoryComponent implements OnInit {
  orders = signal<OrderResponse[]>([]);
  selectedOrder = signal<OrderResponse | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  OrderStatus = OrderStatus;

  constructor(
    private orderService: OrderService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      if (user) {
        this.loadMyOrders(user.id);
      }
    });
  }

  loadMyOrders(userId: number): void {
    this.isLoading.set(true);
    this.orderService.getOrdersByUserId(userId).subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Erreur lors du chargement de vos commandes.');
        this.isLoading.set(false);
        console.error(err);
      },
    });
  }

  openSummary(order: OrderResponse): void {
    this.selectedOrder.set(order);
  }

  closeSummary(): void {
    this.selectedOrder.set(null);
  }

  statusLabel(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Waiting:
        return 'En attente';
      case OrderStatus.Validated:
        return 'Validée';
      case OrderStatus.Refused:
        return 'Refusée';
      default:
        return '';
    }
  }
}
