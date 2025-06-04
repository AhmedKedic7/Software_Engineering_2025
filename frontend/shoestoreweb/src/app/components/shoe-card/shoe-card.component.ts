import { Component, Input, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartsService } from '../../services/carts.service';
import { CommonModule } from '@angular/common';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';


@Component({
  selector: 'app-shoe-card',
  standalone: true,
  imports: [RouterLink,CommonModule,NgbAlert,FormsModule],
  templateUrl: './shoe-card.component.html',
  styleUrl: './shoe-card.component.scss'
})
export class ShoeCardComponent implements OnInit{
  @Input() product: any;
  showAlert: boolean = false;
  isLoggedIn: boolean = false;
   
  constructor(
    private cartService: CartsService,
    private authService:AuthService, 
    private router:Router,
    private notificationService:NotificationService
  ) {}

  ngOnInit(): void {
    this.authService.getUserObservable().subscribe((user) => {
      this.isLoggedIn = this.authService.isLoggedIn();
    });
  }
  addToCart(productId: string): void {
    const userData = this.authService.getUserData();
    if (!userData) {
      console.error('User data not found. Please log in.');
      return;
    }
    
    const userId = this.authService.getUserData()?.userId;
    if(userId){
    this.cartService.addItemToCart(productId, 1).subscribe(
      () => {
        this.notificationService.showMessage('Succesfully added to cart!','alert-success');
        this.cartService.loadCart(userId); 
      },
      (error) => {
        this.notificationService.showMessage('Error loading item to cart, try again later!','alert-error');
        console.error('Error adding item to cart:', error);
      }
    );
  }
  }

  getMainImage(images: { imageUrl: string; isMain: boolean }[]): string {
    const mainImage = images.find(image => image.isMain);
    return mainImage ? mainImage.imageUrl : 'https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/0d5b542c-bb5d-470d-84c3-7f0e17027ab7/NIKE+DUNK+LOW+RETRO.png'; // Default image fallback
  }

  navigateToLogin(): void {
    this.router.navigate(['/login'])
  }
}
