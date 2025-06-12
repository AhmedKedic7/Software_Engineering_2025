import { Component } from '@angular/core';
import { ShoeCardComponent } from '../../components/shoe-card/shoe-card.component';
import { ProductsService } from '../../services/products.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ShoeCardComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  latestShoes: any[] = [];

  constructor(private productsService: ProductsService) {
    this.productsService.getLatestProducts().subscribe(
      (data) => {
        this.latestShoes = data;
      },
      (error) => {
        console.error('Error fetching latest products:', error);
      }
    );
  }
}
