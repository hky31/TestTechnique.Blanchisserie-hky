import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderResponse, OrderStatus } from '../../models/order.models';

@Component({
  selector: 'app-order-confirmation-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-confirmation-modal.html',
  styleUrl: './order-confirmation-modal.scss',
})
export class OrderConfirmationModalComponent {
  @Input() order!: OrderResponse;
  @Input() showActions = false; // true uniquement pour le tableau admin
  @Output() confirm = new EventEmitter<OrderStatus>();
  @Output() close = new EventEmitter<void>();

  OrderStatus = OrderStatus; // pour l'utiliser dans le template

  get totalPrice(): number {
    return this.order.orderItems.reduce((sum, item) => sum + item.price, 0);
  }

  onValidate(): void {
    this.confirm.emit(OrderStatus.Validated);
  }

  onRefuse(): void {
    this.confirm.emit(OrderStatus.Refused);
  }

  onClose(): void {
    this.close.emit();
  }
}
