import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, catchError, throwError } from 'rxjs';
import { LoginRequest, AuthResponse, User } from '../models/auth.models';
import { environment } from '../../environments/environment';
import { OrderItem, OrderRequest, OrderResponse } from '../models/order.models';

@Injectable({
  providedIn: 'root',
})
export class OrderItemService {
  private apiUrl = `${environment.apiUrl}/orderitem`;

  constructor(private http: HttpClient) {}

  getAllItems(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.apiUrl}/get`);
  }

  getAllItemByOrderId(orderId: number): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.apiUrl}/get${orderId}`);
  }
}
