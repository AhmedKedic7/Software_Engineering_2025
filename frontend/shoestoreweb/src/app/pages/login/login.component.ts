import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService,private notificationService:NotificationService) {}

  ngOnInit(): void {
    
    if (this.authService.isLoggedIn()) {
      console.log('User already logged in');
    }
  }

  onLogin() {
    
    this.authService.login(this.email, this.password);
   
  }
}