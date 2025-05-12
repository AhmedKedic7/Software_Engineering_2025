import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
// import { CreateProductContract } from '../components/product-modal/product-modal.component';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private apiUrl = 'http://localhost:5098/api/Products';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl); // Ensure this matches your API's response format
  }
  
  getProductById(id:string):Observable<any>{
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // addProduct(product: CreateProductContract): Observable<CreateProductContract> {
  //   return this.http.post<CreateProductContract>(this.apiUrl, product);
  // }
}
