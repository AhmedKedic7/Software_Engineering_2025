import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartsService } from '../../services/carts.service';
import { CommonModule } from '@angular/common';
// import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-shoe-card',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './shoe-card.component.html',
  styleUrl: './shoe-card.component.scss'
})
export class ShoeCardComponent {
  @Input() product: any;
  showAlert: boolean = false;
  constructor(private cartService: CartsService) {}

  addToCart(productId: string): void {
    this.cartService.addToCart(productId); // Add product ID to cart
    this.showAlert = true; // Show alert when product is added
    setTimeout(() => {
      this.showAlert = false; // Hide alert after 3 seconds
    }, 3000);
  }

  getMainImage(images: { imageUrl: string; isMain: boolean }[]): string {
    const mainImage = images.find(image => image.isMain);
    return mainImage ? mainImage.imageUrl : 'https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/0d5b542c-bb5d-470d-84c3-7f0e17027ab7/NIKE+DUNK+LOW+RETRO.png'; // Default image fallback
  }
}
