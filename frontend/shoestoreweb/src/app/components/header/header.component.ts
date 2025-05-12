import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  searchText: string = '';
  selectedCategory: string = 'all-shoes';

  // Event emitter to pass search parameters to the parent component (ProductPageComponent)
  @Output() searchProducts = new EventEmitter<{ text: string, category: string }>();

  onSearch(event: Event): void {
    event.preventDefault(); // Prevent default form submission behavior
    this.searchProducts.emit({ text: this.searchText, category: this.selectedCategory });
  }
}
