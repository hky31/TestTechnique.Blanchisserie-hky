import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { OrderRequest, OrderResponse } from '../models/order.models';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  // default API URL for orders
  private apiUrl = `${environment.apiUrl}/order`;

  // variable to notify when a new order is created
  private orderCreatedSource = new Subject<void>();
  orderCreated$ = this.orderCreatedSource.asObservable();

  constructor(private http: HttpClient) {}

  // call the API endpoint to get the order by id
  getOrderById(orderId: number): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.apiUrl}/${orderId}`);
  }

  // call the API endpoint to get all orders
  getAllOrders(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/get`);
  }

  // call the API endpoint to create a new order
  createOrder(orderRequest: OrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/create`, orderRequest);
  }

  // call the API endpoint to update an existing order -- admin only
  updateOrder(orderId: number, orderRequest: OrderRequest): Observable<OrderResponse> {
    return this.http.put<OrderResponse>(`${this.apiUrl}/update/${orderId}`, orderRequest);
  }

  // call the API endpoint to get all orders by user id
  getOrdersByUserId(userId: number): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/get/user/${userId}`);
  }

  // update the list of orders when a new order is created
  notifyOrderCreated(): void {
    this.orderCreatedSource.next();
  }
}
