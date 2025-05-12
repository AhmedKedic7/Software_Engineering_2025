import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { ShoeCardComponent } from '../../components/shoe-card/shoe-card.component';
import { FormsModule } from '@angular/forms';  
import { CommonModule } from '@angular/common';
import { ProductsService } from '../../services/products.service';



@Component({
  selector: 'app-products',
  standalone: true,
  imports: [HeaderComponent,FooterComponent,ShoeCardComponent,CommonModule,FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  products: any[] = []; 
  dataCount:number=0;
  currentPage = 1;
  itemsPerPage = 12; 
  totalPages = 0;
  //for filtering 
  filteredProducts: any[] = [];
  genderFilters: Set<string> = new Set();
  brandFilters: Set<string> = new Set();
  priceRange: { min: number; max: number } = { min: 0, max: 1000 };
  sizeFilters: Set<number> = new Set();


  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {
    this.productsService.getProducts().subscribe(
      (data) => {
        this.products = data || [];
        this.filteredProducts = [...this.products]; 
        this.dataCount=this.filteredProducts.length;
        this.totalPages = Math.ceil(this.dataCount / this.itemsPerPage);
        console.log('Fetched products:', this.products);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
  //get items per page
  getPaginatedItems() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.filteredProducts.slice(start, end);
  }

  //method to change pages
  changePage(page: number) {
    // event.preventDefault(); 
    this.currentPage = page;
  }

  nextPage() {
    // event.preventDefault(); 
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage() {
    // event.preventDefault();
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

    // Handle gender filter change
  onGenderChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.checked) {
      this.genderFilters.add(input.id);
    } else {
      this.genderFilters.delete(input.id);
    }
    this.applyFilters();
  }

  // Handle brand filter change
  onBrandChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const brand = input.nextSibling?.textContent?.trim() || '';
    if (input.checked) {
      this.brandFilters.add(brand);
    } else {
      this.brandFilters.delete(brand);
    }
    this.applyFilters();
  }

  // Handle size filter change
  onSizeChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const size = parseInt(input.id.replace('size', ''), 10);
    if (input.checked) {
      this.sizeFilters.add(size);
    } else {
      this.sizeFilters.delete(size);
    }
    this.applyFilters();
  }

  // Handle price filter change
  onPriceChange(min: number, max: number): void {
    this.priceRange = { min, max };
    this.applyFilters();
  }

  // Apply all filters
  applyFilters(): void {
    this.filteredProducts = this.products.filter((product) => {
      const matchesGender =
        this.genderFilters.size === 0 || this.genderFilters.has(product.gender);

      const matchesBrand =
        this.brandFilters.size === 0 || this.brandFilters.has(product.brandName);

      const matchesPrice =
        product.price >= this.priceRange.min && product.price <= this.priceRange.max;

      const matchesSize =
      this.sizeFilters.size === 0 ||
      Array.from(this.sizeFilters).some((filterSize) => {
        // Check if product size is an array or a single value
        return Array.isArray(product.size)
          ? product.size.includes(filterSize)
          : product.size === filterSize;
      });

      return matchesGender && matchesBrand && matchesPrice && matchesSize;
    });

    this.dataCount = this.filteredProducts.length;
    this.totalPages = Math.ceil(this.dataCount / this.itemsPerPage)
  }
  
  onSearchProducts(searchParams: { text: string, category: string }): void {
    const { text, category } = searchParams;
    this.filteredProducts = this.products.filter(product => {
      const matchesText = text ? product.name.toLowerCase().includes(text.toLowerCase()) : true;
      const matchesCategory = category === 'all-shoes' || product.category === category;
      return matchesText && matchesCategory;
    });
  }
}
