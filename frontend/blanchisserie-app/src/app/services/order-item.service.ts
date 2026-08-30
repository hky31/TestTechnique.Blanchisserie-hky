import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { OrderItem } from '../models/order.models';

@Injectable({
  providedIn: 'root',
})
export class OrderItemService {
  // default API URL for order items
  private apiUrl = `${environment.apiUrl}/orderitem`;

  constructor(private http: HttpClient) {}

  // call the API endpoint to get all order items
  getAllItems(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.apiUrl}/get`);
  }

  // call the API endpoint to get all order items of one order by ID
  getAllItemByOrderId(orderId: number): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.apiUrl}/get${orderId}`);
  }
}
