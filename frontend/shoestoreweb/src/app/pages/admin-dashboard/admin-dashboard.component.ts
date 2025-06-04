import { Component } from '@angular/core';
import { UserTableComponent } from '../../components/user-table/user-table.component';
import { ProductModalComponent } from '../../components/product-modal/product-modal.component';
import { CommonModule } from '@angular/common';
import { ProductsService } from '../../services/products.service';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [UserTableComponent, ProductModalComponent, CommonModule,CommonModule,FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {
  
  productNumber: number = 0;
  isModalOpen: boolean = false;

 

  updateProductNumber(newNumber: number): void {
    this.productNumber = newNumber;
  }
  
  


  openModal() {
    this.isModalOpen = true;
    console.log('clicked');
  }
  
  closeModal() {
    this.isModalOpen = false;
  }
}
