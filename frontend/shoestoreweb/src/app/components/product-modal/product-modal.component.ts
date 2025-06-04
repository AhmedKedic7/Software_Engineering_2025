import { Component, Output,EventEmitter, OnInit} from '@angular/core';
import { AdminDashboardComponent } from '../../pages/admin-dashboard/admin-dashboard.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { ReactiveFormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';
import { Router } from '@angular/router';

// export interface CreateProductContract {
//   name: string;
//   brandId: number;
//   size: number;
//   gender: string;
//   description: string;
//   price: number;
//   colorId: number;
//   quantityInStock:number;
// }

@Component({
  selector: 'app-product-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './product-modal.component.html',
  styleUrl: './product-modal.component.scss'
})
export class ProductModalComponent  implements OnInit{
  @Output() close = new EventEmitter<void>();

  productForm!: FormGroup;
  
  constructor(private fb: FormBuilder, private productService: ProductsService,private notificationService:NotificationService, private router:Router) {}
  
  ngOnInit(): void {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      brandId: ['', Validators.required],
      size: ['', Validators.required],
      gender: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', Validators.required],
      colorId: ['', Validators.required],
      quantityInStock: ['', Validators.required],
      mainImage: ['', Validators.required],
      image1: [''],
      image2: [''],
      image3: ['']
    });
  }
  
  closeModal() {
    console.log('Modal close triggered');
    this.close.emit();
  }
  
  onSubmit(): void {
    if (this.productForm.invalid){
      this.notificationService.showMessage("Please enter all fields correctly!", "alert-error");
      return;
    } 

    const formValues = this.productForm.value;
    const product = {
        name: formValues.name,
        brandId: formValues.brandId,
        size: formValues.size,
        gender: formValues.gender,
        description: formValues.description,
        price: formValues.price,
        colorId: formValues.colorId,
        quantityInStock: formValues.quantityInStock,
        images: [
            { imageUrl: formValues.mainImage, isMain: true },
            { imageUrl: formValues.image1, isMain: false },
            { imageUrl: formValues.image2, isMain: false },
            { imageUrl: formValues.image3, isMain: false }
        ].filter(img => img.imageUrl) 
    };

    this.productService.addProduct(product).subscribe({
      next: (response) => {
          this.notificationService.showMessage("Product succesfully added","alert-success");
          this.closeModal();
          // const table = $('#product-table').DataTable();
          // table.ajax.reload();
          window.location.reload();
          
      },
      error: (err) => {
          this.notificationService.showMessage("Something went wrong, check input data!","alert-error");
      }
  });
}

}
