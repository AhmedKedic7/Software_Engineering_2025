import { Component } from '@angular/core';
import { RealUserTableComponent } from '../../components/real-user-table/real-user-table.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-admin-dashboard',
  standalone: true,
  imports: [RealUserTableComponent,CommonModule],
  templateUrl: './user-admin-dashboard.component.html',
  styleUrl: './user-admin-dashboard.component.scss'
})
export class UserAdminDashboardComponent {
  userNumber: number = 0;

  updateUserNumber(newNumber: number): void {
    this.userNumber = newNumber;
  }
}
