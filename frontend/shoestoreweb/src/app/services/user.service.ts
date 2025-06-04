import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `${environment.url}/Users`;

  private readonly API_URL = 'https://api.groq.com/openai/v1/chat/completions';
  private readonly API_KEY = '';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl); // Ensure this matches your API's response format
  }
  addAddress(address: {
    userId: string;
    addressLine: string;
  }): Observable<any> {
    return this.http.post<any>(this.apiUrl, address);
  }

  deleteAddress(userId: string, addressId: string): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/users/${userId}/addresses/${addressId}`,
      {}
    );
  }
  getRecommendations(userId: string, limit: number = 5): Observable<any> {
    return this.http.post(
      this.API_URL,
      { userId, limit },
      { headers: { Authorization: `Bearer ${this.API_KEY}` } }
    );
  }
}
