import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { OrderItem, OrderRequest, OrderStatus } from '../../models/order.models';
import { OrderService } from '../../services/order.service';
import { OrderItemService } from '../../services/order-item.service';
import { User } from '../../models/auth.models';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './order.html',
  styleUrl: './order.scss',
})
export class OrderComponent implements OnInit {
  orderItems = signal<OrderItem[]>([]);
  selectedItems = signal<OrderItem[]>([]);
  orderForm: FormGroup;
  isSubmitting = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  readonly authService = inject(AuthService);
  user = signal<User | null>(null);

  totalPrice = computed(() => this.selectedItems().reduce((sum, a) => sum + a.price, 0));

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private orderItemService: OrderItemService,
  ) {
    this.orderForm = this.fb.group({
      selectedItemId: [null, Validators.required],
      commentaire: [''],
    });
  }

  ngOnInit(): void {
    this.orderItemService.getAllItems().subscribe({
      next: (orderItems) => this.orderItems.set(orderItems),
      error: (err) => console.error('Erreur chargement catalogue', err),
    });

    this.authService.currentUser$.subscribe((user: User | null) => {
      this.user.set(user);
    });
  }

  addArticle(): void {
    const selectedId = this.orderForm.get('selectedItemId')?.value;
    if (!selectedId) return;

    const article = this.orderItems().find((a) => a.id === +selectedId);
    if (article) {
      this.selectedItems.update((items) => [...items, article]);
    }
  }

  removeArticle(index: number): void {
    this.selectedItems.update((items) => items.filter((_, i) => i !== index));
  }

  onSubmit(): void {
    if (this.selectedItems().length === 0) {
      this.errorMessage.set('Veuillez ajouter au moins un article.');
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    const orderRequest: OrderRequest = {
      orderItemIds: this.selectedItems().map((item) => item.id),
      commentaire: this.orderForm.get('commentaire')?.value || '',
      status: OrderStatus.Waiting,
    };

    this.orderService.createOrder(orderRequest).subscribe({
      next: () => {
        this.successMessage.set('Commande envoyée avec succès !');
        this.isSubmitting.set(false);
        this.selectedItems.set([]);
        this.orderForm.reset();
        this.orderService.notifyOrderCreated();
      },
      error: (err) => {
        this.errorMessage.set('Erreur lors de la création de la commande.');
        this.isSubmitting.set(false);
        console.error(err);
      },
    });
  }
}
