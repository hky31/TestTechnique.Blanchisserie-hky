import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, catchError, throwError, Subject } from 'rxjs';
import { LoginRequest, AuthResponse, User } from '../models/auth.models';
import { environment } from '../../environments/environment';
import { OrderRequest, OrderResponse } from '../models/order.models';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = `${environment.apiUrl}/order`;

  // Canal de notification : émet un événement chaque fois qu'une commande est créée
  private orderCreatedSource = new Subject<void>();
  orderCreated$ = this.orderCreatedSource.asObservable();

  constructor(private http: HttpClient) {}

  getOrderById(orderId: number): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.apiUrl}/${orderId}`);
  }

  getAllOrders(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/get`);
  }

  createOrder(orderRequest: OrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/create`, orderRequest);
  }

  notifyOrderCreated(): void {
    this.orderCreatedSource.next();
  }

  updateOrder(orderId: number, orderRequest: OrderRequest): Observable<OrderResponse> {
    return this.http.put<OrderResponse>(`${this.apiUrl}/update/${orderId}`, orderRequest);
  }

  getOrdersByUserId(userId: number): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/get/user/${userId}`);
  }
}
