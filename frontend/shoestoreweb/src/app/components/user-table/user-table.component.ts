import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { ProductsService } from '../../services/products.service';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { EditProductRequest, ImageRequest } from '../../models/edit-product.model';



export interface Products{
  productId:string,
  name:string,
  brandId:number,
  qunatityInStock:number,
  size:number,
  totalSold?: number;
  lockedBy:string;
}

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [CommonModule,DataTablesModule,FormsModule],
  templateUrl: './user-table.component.html',
  styleUrl: './user-table.component.scss'
})
export class UserTableComponent implements OnInit {
  @Output() productNumberChange: EventEmitter<number> = new EventEmitter<number>();

  productNumber: number = 0;
  productlist:any[]=[];
  dtoptions:Config={};
  dttrigger:Subject<any>=new Subject<any>();
  selectedProduct: any = null;
  isModalVisible: boolean = false;
  isDeleteModalVisible: boolean = false;
  itemToDelete: any = null; 

  brands = [
    { id: 1, name: 'Nike' },
    { id: 2, name: 'Adidas' },
    { id: 3, name: 'New Balance' },
    { id: 4, name: 'Puma' },
    { id: 5, name: 'Jordan' },
    { id: 1002, name: 'Vans' }
  ];

  colors = [
    { id: 1, name: 'Red' },
    { id: 2, name: 'Green' },
    { id: 3, name: 'Yellow' },
    { id: 4, name: 'Blue' },
    { id: 5, name: 'Purple' },
    { id: 6, name: 'Brown' },
    { id: 7, name: 'Black' },
    { id: 8, name: 'White' },
    { id: 9, name: 'Orange' },
    { id: 10, name: 'Gray' },
    { id: 11, name: 'Pink' }
  ];

  genders = [
    { id: "M", name: 'Male' },
    { id: "F", name: 'Female' },
    { id: "U", name: 'Unisex' },
  ];
  
  constructor(private productService:ProductsService){}

  confirmDelete(item: any): void {
    this.itemToDelete = item;
    this.isDeleteModalVisible = true;
  }

  ngOnInit(): void {
    this.dtoptions = {
      pagingType: 'full_numbers',   
      pageLength: 10,  
      processing: true,  
      language: {
        search: "Filter records:",   
        lengthMenu: "Show _MENU_ records per page"
      }
    };
    
    this.loadproducts(); 
    
   
  }

  ngAfterViewInit(): void {
     
    if (this.productlist.length) {
      this.dttrigger.next(null);  
    }
  }

  loadproducts(){
    this.productService.getProducts().subscribe(item => {
      this.productlist = item; 
      console.log(this.productlist),
      this.productNumber=this.productlist.length;
      console.log(this.productNumber)
      this.productNumberChange.emit(this.productNumber);

      // this.productlist.forEach((product) => {
      //   this.loadTotalSold(product.productId);
      // });
      if (this.productlist.length) {
        this.dttrigger.next(null);  
      }
      
    });
  }



  // loadTotalSold(productId: string): void {
  //   this.productService.getSoldProduct(productId).subscribe((data) => {
  //     if (data && data.totalSold !== undefined) {
  //       const product = this.productlist.find((p) => p.productId === productId);
  //       if (product) {
  //         product.totalSold = data.totalSold;  
  //       }
  //     }
  //   });
  // }

  getAdminIdFromLocalStorage(): string | null {
    const userDataString = localStorage.getItem('userData');
  
    if (userDataString) {
      const userData = JSON.parse(userDataString);
      return userData.userId;
    }
  
    return null; // or you can return undefined or an empty string, based on your use case
  }

  onEditProduct(product: any): void {
    const adminId = this.getAdminIdFromLocalStorage(); 
    console.log(adminId);
    if (!adminId) {
      console.error('Admin not found!');
      return;
    }
  
     
    this.productService.lockProduct(product.productId, adminId).subscribe(
      response => {
        console.log('Product locked successfully:', response);
        this.selectedProduct = { ...product };   
        this.isModalVisible = true;
      },
      error => {
        if (error.status === 423) {
          console.error('Product is locked by another admin:', error.error);
          alert('This product is being edited by another admin.');
        } else {
          console.error('Error locking product:', error);
        }
      }
    );
  }
    // this.selectedProduct = { ...product };
    // console.log("test",this.selectedProduct);
    // this.isModalVisible = true;
    // console.log('isModalVisible:', this.isModalVisible);
  // }
  closeEditModal(): void {
    // this.isModalVisible = false;
    const adminId = this.getAdminIdFromLocalStorage();
if(adminId){
  this.productService.unlockProduct(this.selectedProduct.productId, adminId).subscribe(
    () => {
      this.isModalVisible = false;
    },
    (error) => {
      console.error('Error unlocking product:', error);
    }
  );
}
  }

  saveProduct(): void {
     // Prepare the EditProductRequest object
  const editProductRequest: EditProductRequest = {
    productId: this.selectedProduct.productId, // Same productId
    version: this.selectedProduct.version + 1,  // Increment version to create a new version
    name: this.selectedProduct.name,
    description: this.selectedProduct.description,
    size: this.selectedProduct.size.toString(),
    price: this.selectedProduct.price,
    quantityInStock: this.selectedProduct.quantityInStock,
    brandId: this.selectedProduct.brandId,
    colorId: this.selectedProduct.colorId,
    gender: this.selectedProduct.gender,
    images: this.selectedProduct.images.map((img: ImageRequest, index: number) => ({
      imageUrl: img.imageUrl,
      isMain: index === 0 
    })),
    
  };
  console.log(editProductRequest);

   
  this.productService.updateProduct(editProductRequest).subscribe(
    (response) => {
      console.log('New version of product created successfully:', response);
      this.closeEditModal();  
      
      const updatedProduct = response;  // Assume this returns the updated product

      const table = $('#product-table').DataTable(); // Access the DataTable instance

      // Find the row that needs to be updated by the productId
      const rowIndex = table
        .rows()
        .indexes()
        .toArray()
        .find((index) => table.cell(index, 0).data() === updatedProduct.productId);  // Assuming Product ID is in the first column (index 0)

      if (rowIndex !== undefined) {
        
        table.cell(rowIndex, 1).data(updatedProduct.name);               // Update Name (Column 1)
        table.cell(rowIndex, 2).data(updatedProduct.brandId);            // Update Brand ID (Column 2)
        table.cell(rowIndex, 3).data(updatedProduct.quantityInStock);    // Update Quantity in Stock (Column 3)
        table.cell(rowIndex, 4).data(updatedProduct.size);               // Update Size (Column 4)
        table.cell(rowIndex, 5).data(updatedProduct.version);            // Update Version (Column 5)
        table.cell(rowIndex, 6).data(updatedProduct.latest);             // Update Latest (Column 6)
        const deletedAtValue = updatedProduct.deletedAt ? updatedProduct.deletedAt : 'False';
        table.cell(rowIndex, 7).data(deletedAtValue);
        const formattedPrice = `$${updatedProduct.price.toFixed(2)}`;
        table.cell(rowIndex, 8).data(formattedPrice);
              

 
        
        table.row(rowIndex).draw(false); // Draw only this row
      }

      // Optionally close the modal after the update
      this.closeEditModal();
      
    },
    (error) => {
      console.error('Error creating new product version:', error);
      if (error.error) {
        console.log('Backend error details:', error.error);  // Inspect backend error details
      }
    }
  );
  }
  

  // toggleDeletedAt(item: any): void {
  //   const deletedAt: Date | null = item.deletedAt === null ? new Date() : null;
  //   console.log(item.productId,item.version)
    
  //   this.productService.updateDeletedAt(item.productId, item.version, deletedAt).subscribe({
  //       next: (response) => {
  //           console.log('Product updated successfully');
  //           item.deletedAt = deletedAt;  
  //       },
  //       error: (err) => {
  //           console.error('Error updating product:', err);
  //       }
  //   });
  // }

  confirmToggleDelete(): void {
    if (!this.itemToDelete) return;
  
    const deletedAt: Date | null = this.itemToDelete.deletedAt === null ? new Date() : null;
  
    this.productService.updateDeletedAt(
      this.itemToDelete.productId,
      this.itemToDelete.version,
      deletedAt
    ).subscribe({
      next: () => {
        console.log('Product updated successfully');
        this.itemToDelete.deletedAt = deletedAt;  
        this.closeDeleteModal();
      },
      error: (err) => {
        console.error('Error updating product:', err);
      }
    });
  }
  closeDeleteModal(): void {
    this.isDeleteModalVisible = false;
    this.itemToDelete = null;
  }

}

 