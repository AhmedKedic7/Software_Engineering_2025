export interface CreateOrderRequest {
    userId: string;
    addressId:string;
    orderItems: OrderItem[];
    totalPrice: number;
    contactName: string;
    phone: string;
    email: string;
    shippingAddress: string;
  }
  
  export interface OrderItem {
    productId: string;
    quantity: number;
    price: number;
    productName: string;
    imageUrl: string | null;
  }
  
  export interface OrderResponse {
    orderId: string;
    createdAt: string;
    orderItems: OrderItem[];
    shippingAddress: string;
    contactName: string;
    phone: string;
    email: string;
    totalPrice: number;
  }
  