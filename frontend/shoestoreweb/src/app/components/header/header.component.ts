import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SearchService } from '../../services/search.service';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { CartsService } from '../../services/carts.service';



@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit{
  searchQuery: string = '';
  cartItemCount: number = 0;
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  userId:string | null = null;

  constructor(private searchService: SearchService, private router:Router,private authService: AuthService,private cartsService: CartsService) {}

  onSearchChange(event: Event): void {
  const target = event.target as HTMLInputElement | null;
  const value = target?.value || '';
  this.searchService.updateSearchTerm(value);
  this.searchQuery = value;
}
navigateToProducts(): void {
  this.router.navigate(['/products'], {
    queryParams: {
      search: this.searchQuery,
    },
  });
}

navigateToAccount(): void {
  this.router.navigate(['/account'])
}

ngOnInit(): void {
 
  this.authService.getUserObservable().subscribe((user) => {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.isAdmin = user?.isAdmin ?? false;
    this.userId = user?.userId ?? null;
    console.log('Header State - isLoggedIn:', this.isLoggedIn, 'isAdmin:', this.isAdmin);
  });

 
  this.router.events.subscribe((event) => {
    if (event instanceof NavigationEnd) {
      this.isLoggedIn = this.authService.isLoggedIn();
      this.isAdmin = this.authService.isAdmin();
      console.log('Navigation Update - isLoggedIn:', this.isLoggedIn, 'isAdmin:', this.isAdmin);
    }
  });
  this.cartsService.cart$.subscribe({
    next: (cart) => {
      if (cart) {
        const uniqueItems = new Set(cart.cartItems.map(item => item.productId));
        this.cartItemCount = uniqueItems.size;
      } else {
        this.cartItemCount = 0;
      }
    },
    error: (error) => console.error('Error subscribing to cart updates:', error),
  });
  
  this.loadCartItemCount()
}

loadCartItemCount(): void {
  const userData = this.authService.getUserData();
  if (userData) {
    this.cartsService.getCartByUserId(userData.userId).subscribe({
      next: (cart) => {
         console.log(cart)
         const uniqueItems = new Set(cart.cartItems.map(item => item.productId));
         this.cartItemCount = uniqueItems.size;
        console.log(this.cartItemCount)
        
      },
      error: (error) => console.error('Error fetching cart item count:', error),
    });
  }
}

logout(): void {
  if (this.userId) {
    this.authService.logout(this.userId);  
    this.cartItemCount=0;
  } else {
    console.error('User ID is missing, cannot log out properly.');
  }
}
  
}
