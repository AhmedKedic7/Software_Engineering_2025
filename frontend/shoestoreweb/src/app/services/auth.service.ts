import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject,Observable } from 'rxjs';
import { Router } from '@angular/router';
import { NotificationService } from './notification.service';

interface LoginResponse {
  userId: string;
  firstName: string;
  lastName: string;
  isAdmin: boolean;
  token: string;
  phoneNumber:string;
  email:string;
}

interface RegisterUser {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5098/api/Auth';
  
  // Store token and user data in memory
  private token: string | null = this.isBrowser() ? localStorage.getItem('token') : null;
  private userData: LoginResponse | null = this.isBrowser() ? JSON.parse(localStorage.getItem('userData') || 'null') : null;

  // Observables to track login status and user data
  private tokenSubject = new BehaviorSubject<string | null>(this.token);
  private userSubject = new BehaviorSubject<LoginResponse | null>(this.userData);

  constructor(private http: HttpClient, private router: Router,private notification:NotificationService) {}

  // Utility function to check if the environment is the browser
  private isBrowser(): boolean {
    return typeof window !== 'undefined' && typeof window.localStorage !== 'undefined';
  }

  register(user: RegisterUser): void {
    console.log(user);
    this.http.post(`${this.apiUrl}/register`, user).subscribe(
      (response) => {
        console.log('Registration successful:', response);
        this.notification.showMessage("Registration succesful!","alert-success")
        // Automatically log in after successful registration
        this.login(user.email, user.password);
      },
      (error) => {
        console.error('Registration failed:', error);
      }
    );
  }
  login(email: string, password: string) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, password }).subscribe({
      next: (response) => {
        if (response && response.token) {
          // Store the token and user data in memory and localStorage (only in the browser)
          this.token = response.token;
          this.userData = response;
  
          if (this.isBrowser()) {
            localStorage.setItem('token', this.token); // Persist token in localStorage
            localStorage.setItem('userData', JSON.stringify(this.userData)); // Persist user data in localStorage
          }
  
          this.tokenSubject.next(this.token);
          this.userSubject.next(this.userData);
          console.log('Login successful, token saved in memory and localStorage.');
          this.notification.showMessage('Successfully logged in!', "alert-success");
          this.router.navigate(['/home']);
        }
      },
      error: (error) => {
        // Ensure only the "Invalid email or password!" message is displayed on error
        if (error.status === 401) {
          this.notification.showMessage('Invalid email or password!', 'alert-error');
        } else {
          this.notification.showMessage('An error occurred during login!', 'alert-error');
        }
        console.error('Login failed:', error);
      }
    });
  }

  getUserDetails(userId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/profile/${userId}`);
  }

   
  getToken(): string | null {
    return this.token;
  }

  
  getUserData(): LoginResponse | null {
    return this.userData;
  }

   
  getTokenObservable() {
    return this.tokenSubject.asObservable();
  }

  
  getUserObservable() {
    return this.userSubject.asObservable();
  }

   
  isLoggedIn(): boolean {
    return !!this.token;
  }

   
  logout(userId:string) {
    this.http.post(`${this.apiUrl}/logout/${userId}`, {}).subscribe(
      () => {
        this.token = null;
        this.userData = null;
        if (this.isBrowser()) {
          localStorage.removeItem('token');
          localStorage.removeItem('userData');
        }
        this.tokenSubject.next(null);
        this.userSubject.next(null);
        console.log('Logged out and database updated');
        this.router.navigate(['/login']);
      },
      (error) => {
        console.error('Logout failed:', error);
      }
    );
  }

  isAdmin(): boolean {
    return this.userData?.isAdmin ?? false;
  }

  
}

