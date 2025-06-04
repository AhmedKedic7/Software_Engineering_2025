import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { ShoeCardComponent } from '../../components/shoe-card/shoe-card.component';
import { FormsModule } from '@angular/forms';  
import { CommonModule } from '@angular/common';
import { ProductsService } from '../../services/products.service';
import { SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { NotificationService } from '../../services/notification.service';



@Component({
  selector: 'app-products',
  standalone: true,
  imports: [ShoeCardComponent,CommonModule,FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  
  products: any[] = [];
  dataCount: number = 0;
  currentPage = 1;
  itemsPerPage = 12;
  totalPages = 0;
  filteredProducts: any[] = [];
  genderFilters: Set<string> = new Set();
  brandFilters: Set<string> = new Set();
  priceRange: { min: number; max: number } = { min: 0, max: 1000 };
  sizeFilters: Set<number> = new Set();
  searchTerm: string = '';
  sortOption: string = 'az';
  
  constructor(
    private productsService: ProductsService,
    private searchService: SearchService,
    private activatedRoute: ActivatedRoute,
    private notification:NotificationService
  ) {}
  
  ngOnInit(): void {
    this.activatedRoute.queryParams.pipe(
      switchMap((params) => {
        this.searchTerm = params['search'] || '';
        return this.fetchProducts();
      })
    ).subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  
    this.searchService.currentSearchTerm.subscribe((term) => {
      this.searchTerm = term;
      this.fetchProducts().subscribe(
        (response) => {
          this.handleResponse(response);
        },
        (error) => {
          console.error('Error fetching products:', error);
        }
      );
    });
  }
  
  fetchProducts(): Observable<any> {
    const filterDto = {
      genderFilters: Array.from(this.genderFilters),
      brandFilters: Array.from(this.brandFilters),
      sizeFilters: Array.from(this.sizeFilters),
      priceRange: this.priceRange,
      searchTerm: this.searchTerm,
      sortOption: this.sortOption,
      currentPage: this.currentPage,
      itemsPerPage: this.itemsPerPage
    };
  
    return this.productsService.getFilteredProducts(filterDto);
  }
  
  handleResponse(response: any): void {
    console.log('Response from backend:', response);
    if (typeof response.totalCount === 'number' && response.totalCount >= 0) {
      this.dataCount = response.totalCount;
    } else {
      console.warn('Invalid TotalCount in response:', response.TotalCount);
      this.dataCount = 0;
    }
    this.products = response.products || [];
    this.calculateTotalPages();
  }
  
  calculateTotalPages(): void {
    if (this.dataCount > 0 && this.itemsPerPage > 0) {
      this.totalPages = Math.ceil(this.dataCount / this.itemsPerPage);
    } else {
      this.totalPages = 1; // Assume at least 1 page to prevent issues
      console.warn('Invalid data for calculating total pages:', {
        dataCount: this.dataCount,
        itemsPerPage: this.itemsPerPage,
      });
    }
  }
  
  
  // getPaginatedItems() {
  //   const start = (this.currentPage - 1) * this.itemsPerPage;
  //   const end = start + this.itemsPerPage;
  //   console.log('Start:', start, 'End:', end, 'Products:', this.products);
  //   return this.products.slice(start, end);
  // }
  
  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      console.log('Changing page to:', this.currentPage);
      this.fetchProducts().subscribe(
        (response) => {
          this.handleResponse(response);
        },
        (error) => {
          console.error('Error fetching products:', error);
        }
      );
    }
  }
  
  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.fetchProducts().subscribe(
        (response) => {
          this.handleResponse(response);
        },
        (error) => {
          console.error('Error fetching products:', error);
        }
      );
    }
  }
  
  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.fetchProducts().subscribe(
        (response) => {
          this.handleResponse(response);
        },
        (error) => {
          console.error('Error fetching products:', error);
        }
      );
    }
  }
  
  onGenderChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.checked) {
      this.genderFilters.add(input.id);
    } else {
      this.genderFilters.delete(input.id);
    }
    this.currentPage = 1;
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  onItemsPerPageChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.itemsPerPage = parseInt(select.value, 10); // Update the items per page
    this.currentPage = 1; // Reset to first page
  
    // Fetch products with updated pagination settings
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
  
  onBrandChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const brand = input.nextSibling?.textContent?.trim() || '';
    if (input.checked) {
      this.brandFilters.add(brand);
    } else {
      this.brandFilters.delete(brand);
    }
    this.currentPage = 1;
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
  
  onSizeChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const size = parseInt(input.id.replace('size', ''), 10);
    if (input.checked) {
      this.sizeFilters.add(size);
    } else {
      this.sizeFilters.delete(size);
    }
    this.currentPage = 1;
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
  
  onPriceChange(min: number, max: number): void {
    if (min < 0) {
      this.notification.showMessage('Minimum price cannot be less than 0!','alert-error');
      return;
    }
    if (max < 0) {
      this.notification.showMessage('Maximum price cannot be less than 0!', 'alert-error');
      return;
    }
  
    // Validation: Ensure minimum price is not greater than maximum price
    if (min > max) {
      this.notification.showMessage('Minimum price cannot be greater than maximum price!', 'alert-error');
      return;
    }
  
    // Update price range if validations pass
    this.priceRange = { min, max };
    this.currentPage = 1;
  
    // Fetch updated products
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
  
  onSortChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.sortOption = select.value;
    this.currentPage = 1;
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
        
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
    
  }
  
  getPagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
  clearFilters(): void {
     
    this.genderFilters.clear();
    this.brandFilters.clear();
    this.sizeFilters.clear();
  
    
    this.priceRange = { min: 0, max: 1000 };
    this.searchTerm = '';
    this.sortOption = 'az';
  
     
    this.currentPage = 1;
  
     
    this.fetchProducts().subscribe(
      (response) => {
        this.handleResponse(response);
        this.notification.showMessage('Filters cleared successfully!', 'alert-success');
      },
      (error) => {
        console.error('Error fetching products:', error);
        this.notification.showMessage('Failed to clear filters.', 'alert-error');
      }
    );
  }
}
