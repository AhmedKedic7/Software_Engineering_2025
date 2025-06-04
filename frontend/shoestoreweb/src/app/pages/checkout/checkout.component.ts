import { Component, OnInit } from '@angular/core';
import { CheckoutItemComponent } from '../../components/checkout-item/checkout-item.component';
import { AuthService } from '../../services/auth.service';
import { CartsService } from '../../services/carts.service';
import { CartContract } from '../../models/cart.model';
import { CommonModule, Location } from '@angular/common';
import { CreateOrderRequest } from '../../models/order.model';
import { OrderService } from '../../services/order.service';
import { User } from '../../models/user-profile.model';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';



@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit{
  cartItems:any[]=[];
  cart: CartContract | null = null;
  errorMessage: string | null = null;
  user?: User;
  userDetails?:User;
  selectedAddressIndex:  number | null = null;
  isModalOpen: boolean = false;
  animationState = false; 
  newAddress = { addressLine: '' };
  
  constructor(
    private cartsService: CartsService, 
    private authService: AuthService ,
    private orderService:OrderService, 
    private router:Router, 
    private notificationService:NotificationService,
    private userService:UserService
  ) {}
  
  ngOnInit(): void {
    this.loadCart();
    this.loadUserDetails();
  }

  
  loadCart(): void {
    const userData = this.authService.getUserData();
    if (userData) {
      this.cartsService.getCartByUserId(userData.userId).subscribe({
        next: (cart) => {
          this.cart = cart; 
          this.cartItems = this.cart?.cartItems || [];
          console.log('Cart Items:', this.cartItems);
        },
        error: (error) => {
          console.error('Error loading cart', error);
          this.notificationService.showMessage("Cart is empty, please add new products!","alert-info")
        },
      });
    }
  }
  loadUserDetails(): void {
    const userDataString = localStorage.getItem('userData');
    if (userDataString) {
      try {
        const userData = JSON.parse(userDataString); 
        const loggedInUserId = userData.userId;
  
         
        this.authService.getUserDetails(loggedInUserId).subscribe(
          (response: User) => {
            this.user = response; 
            console.log('User details fetched successfully:', this.user);
          },
          (error) => {
            console.error('Error fetching user details:', error);
          }
        );
      } catch (error) {
        console.error('Error parsing userData from localStorage:', error);
      }
    } else {
      console.error('No user data found in localStorage.');
    }
  }

  checkout(): void {
    if (!this.cart || !this.cart.cartId) {
      this.notificationService.showMessage("Cart is empty, please add new products!","alert-info")
      return;
    }

    const userData = this.authService.getUserData();
    
    const selectedAddress = this.user?.addresses[this.selectedAddressIndex!];
    
    if (!selectedAddress) {
      this.notificationService.showMessage("Please select one address!","alert-info")
      return;
    }
  
     
    const addressId = selectedAddress.addressId;
    if (!userData) {
      this.errorMessage = 'User not logged in.';
      return;
    }

    const orderRequest: CreateOrderRequest = {
      userId: userData.userId,
      totalPrice: this.getTotalPrice(),
      addressId: addressId,
      contactName:`${userData.firstName} ${userData.lastName}`,
      phone: this.user?.phoneNumber || 'N/A',  
      email: this.user?.email || 'N/A',  
      shippingAddress: selectedAddress!.addressLine,
      orderItems: this.mapCartItemsToOrderItems()
    };
    this.orderService.createOrder(orderRequest).subscribe({
      next: (orderResponse) => {
        this.notificationService.showMessage('Your order has been placed successfully!','alert-success');
        console.log('Order Response:', orderResponse);

        this.animationState = true;
        this.cartItems = [];
        this.getFinalPrice()
        setTimeout(() => {
          this.router.navigate(['/account']);
        }, 3000);
      },
      error: (error) => {
        this.errorMessage = 'Failed to complete checkout. Please try again later.';
      },
    });
  
    this.cartsService.checkout(this.cart.cartId).subscribe({
      next: () => {
        console.log('Checkout successful');
         
       
        this.cartItems = [];
        
         
      },
      error: (error) => {
        console.error('Checkout failed', error);
        this.errorMessage = 'Failed to complete checkout. Please try again later.';
      },
    });
  }

  mapCartItemsToOrderItems() {
    return this.cartItems.map(item => {
      if (!item.product || !item.product.productId) {
          throw new Error("Invalid product data in cart item.");
      }

      return {
          productId: item.product.productId,
          quantity: item.quantity > 0 ? item.quantity : 1,  
          price: item.product.price ?? 0,                 
          productName: item.product.name ?? "Unknown",     
          imageUrl: item.product.imageUrl || "/images/default-product.png"   
      };
  });
  }

  selectAddress(index: number): void {
    this.selectedAddressIndex = index;
    console.log('Selected Address Index:', this.selectedAddressIndex);
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
  getMainImage(images: { imageUrl: string; isMain: boolean }[]): string {
    const mainImage = images.find(image => image.isMain);
    return mainImage ? mainImage.imageUrl : 'https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/0d5b542c-bb5d-470d-84c3-7f0e17027ab7/NIKE+DUNK+LOW+RETRO.png'; // Default image fallback
  }

  getFinalPrice(): number {
    return this.getTotalPrice() + 14;
  } 
  navigateToCart(): void {
    this.router.navigate(['/cart']);
  }

  addAddress():void{
    const userData = this.authService.getUserData();
  }
  
   openModal(): void {
    this.isModalOpen = true;
  }

  
  closeModal(): void {
    this.isModalOpen = false;
  }

   
  saveAddress(): void {
    const userData = this.authService.getUserData();
  
    if (userData && this.newAddress.addressLine) {
      const address = {
        userId: userData.userId,
        addressLine: this.newAddress.addressLine
      };
  
      this.userService.addAddress(address).subscribe(
        (response) => {
          console.log('Address added successfully:', response);
          
          // Update user addresses locally after adding the new address
          if (this.user) {
            this.user.addresses.push(response);  // Add the new address to the list
          }
  
          // Close the modal and clear input field
          this.closeModal();
          this.newAddress.addressLine = '';
        },
        (error) => {
          console.error('Error saving address:', error);
        }
      );
    } else {
      console.log('Address line is required!');
    }
  }
  
  
}
