import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductsService } from './products.service';

@Injectable({
  providedIn: 'root'
})
export class CartsService {

  private cartItems: string[] = []; // Store only product IDs

  constructor(private productsService: ProductsService) {}

  
  addToCart(productId: string): void {
    this.cartItems.push(productId); // Add the product ID to the cart
    localStorage.setItem('cart', JSON.stringify(this.cartItems)); // Store in localStorage
  }

  // Remove a product ID from the cart
  removeFromCart(productId: string): void {
    this.cartItems = this.cartItems.filter(id => id !== productId);
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }

  // Clear the cart
  clearCart(): void {
    this.cartItems = [];
    localStorage.removeItem('cart');
  }

  // Get full product details for all cart items
  getFullCartItems() {
    return this.cartItems.map(id => this.productsService.getProductById(id));
  }
}
