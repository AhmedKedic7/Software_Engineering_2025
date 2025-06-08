import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { ProductsService } from './products.service';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

interface AddItemToCartRequest {
  userId: string;
  productId: string;
  quantity: number;
}

interface CartContract {
  cartId: string;
  userId: string;
  createdAt: Date;
  cartItems: CartItemContract[];
}

interface CartItemContract {
  cartItemId: string;
  productId: string;
  quantity: number;
  product: ProductContract;
}

interface ProductContract {
  productId: string;
  brandId: number;
  name: string;
  gender: string;
  size: number;
  description: string;
  price: number;
  createdAt: Date;
  updatedAt?: Date;
  deletedAt?: Date;
  colorId: number;
  brandName: string;
  colorName: string;
  images: ImageContract[];
}

interface ImageContract {
  imageUrl: string;
  isMain: boolean;
  productId: string;
}

@Injectable({
  providedIn: 'root',
})
export class CartsService {
  private apiUrl = `${environment.url}/Carts`;

  private cartSubject: BehaviorSubject<CartContract | null> =
    new BehaviorSubject<CartContract | null>(null);
  cart$: Observable<CartContract | null> = this.cartSubject.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {}

  addItemToCart(productId: string, quantity: number): Observable<void> {
    const userData = this.authService.getUserData();
    if (!userData) {
      throw new Error('User data not found in token');
    }

    const request: AddItemToCartRequest = {
      userId: userData.userId,
      productId,
      quantity,
    };

    return this.http.post<void>(`${this.apiUrl}/add-item`, request);
  }

  loadCart(userId: string): void {
    this.getCartByUserId(userId).subscribe({
      next: (cart) => this.cartSubject.next(cart),
      error: (error) => console.error('Error loading cart:', error),
    });
  }

  removeItemFromCart(cartItemId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/remove-item/${cartItemId}`);
  }

  checkout(cartId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/checkout/${cartId}`, {});
  }

  getCartByUserId(userId: string): Observable<CartContract> {
    return this.http.get<CartContract>(`${this.apiUrl}/${userId}`);
  }
  updateCartItemQuantity(
    cartItemId: string,
    quantity: number
  ): Observable<void> {
    const body = {
      CartItemId: cartItemId,
      Quantity: quantity,
    };
    return this.http.put<void>(`${this.apiUrl}/update-item-quantity`, body);
  }
}
