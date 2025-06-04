export interface Address {
    addressId: string;
    addressLine: string;
  }
  
  export interface User {
    userId: string;
    fullName:string;
    email: string;
    createdAt: Date;
    addresses: Address[];
    phoneNumber:string;
  }