import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrderRequest,OrderResponse } from '../models/order.model';


@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = 'http://localhost:5098/api/Orders'; 

  constructor(private http: HttpClient) {}

  createOrder(orderRequest: CreateOrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/create`, orderRequest);
  }

  getOrdersByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/users/${userId}`);
  }
  getAllOrders(): Observable<any[]>{
    return this.http.get<any[]>(this.apiUrl);
  }
}
