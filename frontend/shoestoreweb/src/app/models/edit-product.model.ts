export interface EditProductRequest {
  productId: string;
  version: number; // version to be incremented
  name: string;
  description: string;
  size: string;
  price: number;
  quantityInStock: number;
  brandId: number;
  colorId: number;
  gender: string;
  images: ImageRequest[];
}

export interface ImageRequest {
  imageUrl: string;
  isMain: boolean; // New field to indicate if the image is the main one
}
