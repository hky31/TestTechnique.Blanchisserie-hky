import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-order',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './order.html',
  styleUrl: './order.scss',
})
export class OrderComponent {
  private readonly fb = inject(FormBuilder);

  orderForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor() {
    this.orderForm = this.fb.group({
      articlename: ['', [Validators.required]],
      price: ['5', [Validators.required]],
    });
  }

  onSubmit(): void {}

  addOrderItem(): void {}

  removeOrderItem(): void {}
}
