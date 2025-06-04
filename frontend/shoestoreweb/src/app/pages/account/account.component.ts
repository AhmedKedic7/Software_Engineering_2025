import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user-profile.model';
import { CommonModule } from '@angular/common';
import { OrderSummaryComponent } from '../../components/order-summary/order-summary.component';
import { OrderService } from '../../services/order.service';
import { UserService } from '../../services/user.service';
import { GroqService } from '../../services/groq.service';


interface OrderItem {
  productId: string;
  quantity: number;
  price: number;
  product: {
    name: string;
    description: string;
  };
  imageUrl: string | null;
}
@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule,OrderSummaryComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss'
})
export class AccountComponent implements OnInit{
  user?: User; 
  orders: any[]=[];
  recommendations: any[] = [];
  showRecommendationsModal = false;
  paginatedOrders: any[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 5;  
  totalOrders: number = 0;
  formattedResponse: string | null = null;
  isDeleteModalOpen: boolean = false;
  addressToDelete: string | null = null;
  response: string | null = null;

  constructor(private authService: AuthService,
    private orderService: OrderService,
    private userService:UserService,
    private groqService:GroqService
  ) {}

ngOnInit(): void {
  
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

      this.orderService.getOrdersByUserId(loggedInUserId).subscribe(
        (response)=>{
          this.orders = response;
          console.log(this.orders)
          this.totalOrders = this.orders.length;
          this.updatePagination();
          console.log(response)
          this.fetchCompletion();
        },
        (error) => {
          console.log('Error fetching orders',error);
        }
      )
    } catch (error) {
      console.error('Error parsing userData from localStorage:', error);
    }
  } else {
    console.error('No user data found in localStorage.');
  }
  
}

fetchCompletion() {
  if (this.orders.length === 0) {
    console.error('No orders available to generate recommendations.');
    return;
  }

  // Create a prompt by extracting item names and descriptions from orders
  const orderDetails = this.orders.map((order) => {
    // Ensure there are order items
    if (order.orderItems && order.orderItems.length > 0) {
      // Map through the order items and access the product details
      return order.orderItems.map((orderItem: OrderItem) => {
        const itemName = orderItem.product?.name || 'Unknown Item';
        const itemDescription = orderItem.product?.description || 'No description available';
        return `• **${itemName}**: ${itemDescription}`;
      }).join('\n');
    }
    return '';  
  }).join('\n\n');  
  // Construct the prompt for the GroqService
  const prompt = `Here are my recent orders:\n${orderDetails}\nBased on these orders, can you recommend me the top 3 shoes?  Be short and concise.`;

  // Call GroqService to get recommendations
  this.groqService.getChatCompletion(prompt).subscribe({
    next: (res) => {
      this.response = res.choices?.[0]?.message?.content || 'No response';
      console.log('Recommendations:', this.response);
      this.formattedResponse = this.formatResponse(this.response || 'No response');
      this.showRecommendationsModal = true;
    },
    error: (err) => {
      console.error('Error fetching chat completion:', err);
    },
  });
}

formatResponse(response: string): string {
  // Add line breaks between recommendations for better readability
  let formatted = response.split('\n').map((item, index) => {
    if (index === 0) {
      return `<p><strong>${item}</strong></p>`;
    } else {
      return `<p>${item.trim()}</p>`;
    }
  }).join('');
  return formatted;
}


updatePagination() {
  const startIndex = (this.currentPage - 1) * this.itemsPerPage;
  const endIndex = startIndex + this.itemsPerPage;
  this.paginatedOrders = this.orders.slice(startIndex, endIndex);
}

closeRecommendationsModal() {
  this.showRecommendationsModal = false;
}

nextPage() {
  if (this.currentPage * this.itemsPerPage < this.totalOrders) {
    this.currentPage++;
    this.updatePagination();
  }
}

prevPage() {
  if (this.currentPage > 1) {
    this.currentPage--;
    this.updatePagination();
  }
}
get totalPages(): number {
  return Math.ceil(this.orders.length / this.itemsPerPage);
}

openDeleteModal(addressId: string): void {
  this.isDeleteModalOpen = true;
  this.addressToDelete = addressId;
}

closeDeleteModal(): void {
  this.isDeleteModalOpen = false;
  this.addressToDelete = null;
}

// deleteAddress(addressId:string):void {
//   const userId = this.authService.getUserData()?.userId;  
  
//   if (userId) {
//     this.userService.deleteAddress(userId,addressId).subscribe(
//       (response) => {
//         console.log('Address deleted successfully:', response);
//         this.ngOnInit()
//         // Remove the address from the local user object
//         if (this.user) {
//           this.user.addresses = this.user.addresses.filter(
//             (address) => address.addressId!== address.addressId
//           );
//         }
        
//       },
//       (error) => {
//         console.error('Error deleting address:', error);
//       }
//     );
//   } else {
//     console.error('User is not logged in');
//   }
// }

confirmDeleteAddress(): void {
  if (this.addressToDelete) {
    const userId = this.authService.getUserData()?.userId;

    if (userId) {
      this.userService.deleteAddress(userId, this.addressToDelete).subscribe(
        (response) => {
          console.log('Address deleted successfully:', response);
          this.ngOnInit();  
          this.closeDeleteModal();
        },
        (error) => {
          console.error('Error deleting address:', error);
        }
      );
    } else {
      console.error('User is not logged in');
      this.closeDeleteModal();
    }
  }
}

}
