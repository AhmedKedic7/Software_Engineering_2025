import { Component } from '@angular/core';

import { CommonModule } from '@angular/common';
import { OrderTableComponent } from '../../components/order-table/order-table.component';

@Component({
  selector: 'app-order-admin-dashboard',
  standalone: true,
  imports: [OrderTableComponent,CommonModule],
  templateUrl: './order-admin-dashboard.component.html',
  styleUrl: './order-admin-dashboard.component.scss'
})
export class OrderAdminDashboardComponent {
  orderNumber: number = 0;

  updateOrderNumber(newNumber: number): void {
    this.orderNumber = newNumber;
  }
}
