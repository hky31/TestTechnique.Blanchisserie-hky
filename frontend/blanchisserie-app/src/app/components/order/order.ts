import { Component, inject, OnInit } from '@angular/core';
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
  orderItems: OrderItem[] = [];
  selectedItems: OrderItem[] = [];
  orderForm: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  readonly authService = inject(AuthService);
  user: User | null = null;

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
      next: (orderItems) => (this.orderItems = orderItems),
      error: (err) => console.error('Erreur chargement catalogue', err),
    });

    this.authService.currentUser$.subscribe((user: User | null) => {
      this.user = user;
    });
  }

  addArticle(): void {
    const selectedId = this.orderForm.get('selectedItemId')?.value;
    if (!selectedId) return;

    const article = this.orderItems.find((a) => a.id === +selectedId);
    if (article) {
      this.selectedItems.push(article);
    }

    // Réinitialise uniquement le select, pas tout le formulaire
    this.orderForm.get('selectedItemId')?.reset();
  }

  removeArticle(index: number): void {
    this.selectedItems.splice(index, 1);
  }

  get totalPrice(): number {
    return this.selectedItems.reduce((sum, a) => sum + a.price, 0);
  }

  onSubmit(): void {
    if (this.selectedItems.length === 0) {
      this.errorMessage = 'Veuillez ajouter au moins un article.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const orderRequest: OrderRequest = {
      orderItemIds: this.selectedItems.map((item) => item.id),
      commentaire: this.orderForm.get('commentaire')?.value || '',
      status: OrderStatus.Waiting,
    };

    this.orderService.createOrder(orderRequest).subscribe({
      next: () => {
        this.successMessage = 'Commande envoyée avec succès !';
        this.isSubmitting = false;
        this.selectedItems = [];
        this.orderForm.reset();
        this.orderService.notifyOrderCreated(); // Notifie les autres composants que la commande a été créée
      },
      error: (err) => {
        this.errorMessage = 'Erreur lors de la création de la commande.';
        this.isSubmitting = false;
        console.error(err);
      },
    });
  }
}
