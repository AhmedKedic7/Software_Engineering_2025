import { Component } from '@angular/core';
import { CardItemComponent } from '../../components/card-item/card-item.component';
import { CartsService } from '../../services/carts.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { CartContract } from '../../models/cart.model';
import { HttpErrorResponse } from '@angular/common/http';
import { NotificationService } from '../../services/notification.service';


@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CardItemComponent,CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent {
  cart: CartContract | null = null;
  errorMessage: string | null = null;
  succesMessage: string | null = null;
  cartItems: any[] = []; 

  constructor(private cartsService: CartsService, private authService: AuthService,private notificationService:NotificationService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    const userData = this.authService.getUserData();
    if (userData) {
      this.cartsService.getCartByUserId(userData.userId).subscribe({
        next: (cart) => {(this.cart = cart),
        this.cartItems = this.cart?.cartItems || [];
        console.log('Cart Items:', this.cartItems);},
        error: (error) => console.error('Error loading cart', error),
      });
    }
  }
  
  removeItem(cartItemId: string): void {
     
    const cart = this.cart!;
  
     
    this.cartsService.removeItemFromCart(cartItemId).subscribe(
      () => {
        
         
        cart.cartItems = cart.cartItems.filter(
          (item) => item.cartItemId !== cartItemId

        );
        const userId = this.authService.getUserData()?.userId;
      if (userId) {
        this.cartsService.loadCart(userId);  
      }
      },
      (error) => {
        this.notificationService.showMessage( 'Error removing item from cart',"alert-error");
        console.error(error);
      }
    );
  }
  updateQuantity(cartItemId: string, newQuantity: number): void {
    if (!this.cart) return;

    const cartItem = this.cart.cartItems.find(item => item.cartItemId === cartItemId);
    if (cartItem) {
      cartItem.quantity = newQuantity;

       
      this.cartsService.updateCartItemQuantity(cartItemId, newQuantity).subscribe(
        () => this.notificationService.showMessage( 'Succesfully updated item quantity from cart!',"alert-success"),
        
        (error) => {
          this.notificationService.showMessage( 'Error updating quantity, out of stock!',"alert-error");
          console.error(error);
        }
      );
    }
  }
  getTotalPrice(): number {
    if (!this.cart || !this.cart.cartItems) {
      return 0;
    }
    return this.cart.cartItems.reduce((total, item) => {
      const itemPrice = item.product?.price || 0;
      return total + itemPrice * item.quantity;
    }, 0);
  }

  getFinalPrice(): number {
    return this.getTotalPrice() + 14;
  }

  
  

}
