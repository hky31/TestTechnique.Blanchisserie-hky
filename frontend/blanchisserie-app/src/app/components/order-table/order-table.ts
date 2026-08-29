import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderResponse, OrderStatus } from '../../models/order.models';
import { OrderService } from '../../services/order.service';
import { OrderConfirmationModalComponent } from '../order-confirmation-modal/order-confirmation-modal';

@Component({
  selector: 'app-order-table',
  standalone: true,
  imports: [CommonModule, OrderConfirmationModalComponent],
  templateUrl: './order-table.html',
  styleUrl: './order-table.scss',
})
export class OrderTableComponent implements OnInit {
  orders = signal<OrderResponse[]>([]);
  selectedOrder = signal<OrderResponse | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  OrderStatus = OrderStatus;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading.set(true);
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Erreur lors du chargement des commandes.');
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

  onConfirmAction(newStatus: OrderStatus): void {
    const current = this.selectedOrder();
    if (!current) return;

    const payload = {
      orderItemIds: [],
      commentaire: current.commentaire,
      status: newStatus,
    };

    this.orderService.updateOrder(current.id, payload).subscribe({
      next: (updatedOrder) => {
        this.orders.update((list) =>
          list.map((o) => (o.id === updatedOrder.id ? updatedOrder : o)),
        );
        this.closeSummary();
      },
      error: (err) => {
        this.errorMessage.set('Erreur lors de la mise à jour de la commande.');
        console.error(err);
      },
    });
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
