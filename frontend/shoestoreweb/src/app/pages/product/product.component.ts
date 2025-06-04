import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../services/products.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { CartsService } from '../../services/carts.service';
import { NotificationService } from '../../services/notification.service';
import { SoldProduct } from '../../models/sold-product.model';
import { FormsModule } from '@angular/forms';

interface ProductImage {
  imageId: number;
  imageUrl: string;
  isMain: boolean;
  productId: string;
}
interface Product {
  productId: string;
  name: string;
  price: number;
  description: string;
  quantityInStock: number;
  images: ProductImage[];
}

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent implements OnInit{
  product: any = ""; 
  productId: string | null = null;
  mainImage: string="";
  images: string[] = [];
  userData!:any;
  editableQuantity: number = 1;
  totalSold: number = 0;

  constructor(
    private route: ActivatedRoute,
    private productsService: ProductsService,
    private authService:AuthService,
    private cartService:CartsService,
    private notificationService: NotificationService,
    private router:Router
  ) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');   
    this.userData = this.authService.getUserData();
    
    
    console.log('Product ID:', this.productId);

    if (this.productId) {
      this.loadTotalSold(this.productId);
      this.productsService.getProductById(this.productId).subscribe(
        (data) => {
          this.product = data;
          console.log(this.product)
          const mainImageData = data.images.find((image: ProductImage) => image.isMain);
          if (mainImageData) {
            this.mainImage = mainImageData.imageUrl;
          }

          this.images = data.images;
          console.log(data);
        },
        (error) => {
          console.error('Error fetching product:', error);
        }
      );
    } else {
      console.error('Product ID is missing');
    }
  }
  setMainImage(imageUrl: string): void {
    this.mainImage = imageUrl;
  }
  
  checkQuantity(): boolean {
     
    if (this.product && this.editableQuantity > this.product.quantityInStock) {
      this.notificationService.showMessage(
        `Only ${this.product.quantityInStock} items available in stock!`,
        'alert-info'
      );
      this.editableQuantity = this.product.quantityInStock;  
      return false;  
    }
  
     
    if (this.editableQuantity < 1) {
      this.notificationService.showMessage('Quantity must be at least 1!', 'alert-info');
      this.editableQuantity = 1;  
      return false; 
    }
  
    return true;  
  }

  addToCart(productId: string): void {
    
    if (!this.userData) {
      this.notificationService.showMessage("You need to log in to add to cart!",'alert-info')
      return;
    }
    
    if (!this.checkQuantity()) {
      return;  
    }
   
    this.cartService.addItemToCart(productId, this.editableQuantity).subscribe(
      () => {
        console.log("test");
        this.cartService.loadCart(this.userData.userId);
        this.notificationService.showMessage('Item added to cart successfully!', 'alert-success');
      },
      (error) => {
         this.notificationService.showMessage('Insufficient stock, check your cart!', 'alert-error');
      }
    );
  }
  addToCartAndCheckout(productId: string): void {
    
    if (!this.userData) {
      this.notificationService.showMessage("You need to log in to buy the product!",'alert-info')
      return;
    }
    if (!this.checkQuantity()) {
      return;  
    }
    this.cartService.addItemToCart(productId, this.editableQuantity).subscribe(
      () => {
        console.log("test");
        this.router.navigate(['/checkout']);
      },
      (error) => {
         this.notificationService.showMessage('Insufficient stock, check your cart!', 'alert-error');
      }
    );
  }

  loadTotalSold(productId: string): void {
    this.productsService.getSoldProduct(productId).subscribe(
      (data) => {
        if (data && data.totalSold !== undefined) {
          this.totalSold = data.totalSold;
        } else {
          this.totalSold = 0; 
        }
      },
      (error) => {
        console.error('Error loading sold product data:', error);
         
        this.totalSold = 0;
      }
    );
  }
  
  
}

