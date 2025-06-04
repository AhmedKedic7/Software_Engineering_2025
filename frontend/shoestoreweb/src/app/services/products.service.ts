import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
// import { CreateProductContract } from '../components/product-modal/product-modal.component';
import { SoldProduct } from '../models/sold-product.model';
import { EditProductRequest } from '../models/edit-product.model';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  private apiUrl = `${environment.url}/Products`;

  constructor(private http: HttpClient) {}

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getProductById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  addProduct(product: any): Observable<any> {
    return this.http
      .post(this.apiUrl, product)
      .pipe(tap(() => this.getProducts()));
  }

  getFilteredProducts(filterDto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/filter`, filterDto);
  }

  getSoldProduct(productId: string): Observable<SoldProduct> {
    return this.http.get<SoldProduct>(
      `${this.apiUrl}/sold-products/${productId}`
    );
  }

  updateProduct(editProductRequest: EditProductRequest): Observable<any> {
    return this.http.post<SoldProduct>(
      `${this.apiUrl}/update`,
      editProductRequest
    );
  }

  updateDeletedAt(productId: string, version: number, deletedAt: Date | null) {
    const body = deletedAt;
    return this.http.patch(`${this.apiUrl}/${productId}/${version}`, body);
  }

  getLatestProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/latest`);
  }

  lockProduct(productId: string, adminId: string): Observable<any> {
    const body = { adminId };
    return this.http.put(`${this.apiUrl}/${productId}/lock`, body);
  }
  unlockProduct(productId: string, adminId: string): Observable<any> {
    const body = { adminId: adminId };
    return this.http.put(`${this.apiUrl}/${productId}/unlock`, body);
  }
}
