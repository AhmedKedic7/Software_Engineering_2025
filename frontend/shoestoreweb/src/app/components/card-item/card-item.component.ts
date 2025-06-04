import { CommonModule } from '@angular/common';
import { Component,EventEmitter,Input,OnChanges,Output, SimpleChanges } from '@angular/core';
import { CartItemContract, ImageContract } from '../../models/cart.model';
import { FormsModule } from '@angular/forms'; 
import { NotificationService } from '../../services/notification.service';
import { RouterLink } from '@angular/router';
 
@Component({
  selector: 'app-card-item',
  standalone: true,
  imports: [RouterLink,CommonModule,FormsModule],
  templateUrl: './card-item.component.html',
  styleUrl: './card-item.component.scss'
})
export class CardItemComponent implements OnChanges{
  @Input() cartItem: CartItemContract | null = null;
  @Output() remove = new EventEmitter<string>();
  @Output() quantityChange = new EventEmitter<{ cartItemId: string; quantity: number }>();

  editableQuantity: number = 1;


  constructor(private notificationService:NotificationService){}
  
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['cartItem'] && this.cartItem) {
      
      this.editableQuantity = this.cartItem.quantity;
    }
  }
 
  onRemove() {
    if (this.cartItem) {
      this.remove.emit(this.cartItem.cartItemId);
    }
    if (this.cartItem) {
      this.editableQuantity = this.cartItem.quantity;
    }
  }

  onQuantityChange(): void {
    // if (this.editableQuantity < 1) {
    //   this.notificationService.showMessage('Quantity must be at least 1!', 'alert-info');
    //   this.editableQuantity = 1;  
    // }

    if (!this.checkQuantity()) {
      return;  
    }
    if (this.cartItem) {
      this.quantityChange.emit({ cartItemId: this.cartItem.cartItemId, quantity: this.editableQuantity });
    }
  }

  getMainImage(images: ImageContract[] | null | undefined): string {
    if (!images || images.length === 0) {
      return 'https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/0d5b542c-bb5d-470d-84c3-7f0e17027ab7/NIKE+DUNK+LOW+RETRO.png';
    }
    const mainImage = images.find(image => image.isMain);
    return mainImage?.imageUrl || 'https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/0d5b542c-bb5d-470d-84c3-7f0e17027ab7/NIKE+DUNK+LOW+RETRO.png';
  }
  checkQuantity(): boolean {
     
     
    if (this.editableQuantity < 1) {
      this.notificationService.showMessage('Quantity must be at least 1!', 'alert-info');
      this.editableQuantity = 1;  
      return false; 
    }
  
    return true;  
  }

  

}
