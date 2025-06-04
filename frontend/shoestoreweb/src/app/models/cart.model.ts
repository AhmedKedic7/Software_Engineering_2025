export interface CartContract {
    cartId: string;
    userId: string;
    createdAt: Date;
    cartItems: CartItemContract[];
  }
  
  export interface CartItemContract {
    cartItemId: string;
    productId: string;
    quantity: number;
    product: ProductContract | null;
  }
  
  export interface ProductContract {
    productId: string;
    brandId: number;
    name: string;
    gender: string;
    size: number;
    description: string;
    price: number;
    createdAt: Date;
    updatedAt?: Date;
    deletedAt?: Date;
    colorId: number;
    brandName: string;
    colorName: string;
    images: ImageContract[];
  }
  
  export interface ImageContract {
    imageId?: string;
    imageUrl: string;
    isMain: boolean;
    productId: string;
  }