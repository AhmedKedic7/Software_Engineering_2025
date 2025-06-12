import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  phoneNumber: string = '';

  constructor(
    private authService: AuthService,
    private notificationservice: NotificationService
  ) {}

  ngOnInit(): void {}

  onRegister(): void {
    if (
      !this.firstName ||
      !this.lastName ||
      !this.email ||
      !this.password ||
      !this.confirmPassword ||
      !this.phoneNumber
    ) {
      this.notificationservice.showMessage(
        'All fields are required!',
        'alert-error'
      );
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Basic email validation
    if (!emailRegex.test(this.email)) {
      this.notificationservice.showMessage(
        "Invalid email format! Ensure it includes '@'",
        'alert-error'
      );
      return;
    }

    // Validate passwords
    if (this.password !== this.confirmPassword) {
      this.notificationservice.showMessage(
        'Passwords do not match!',
        'alert-error'
      );
      return;
    }

    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
    if (!passwordRegex.test(this.password)) {
      this.notificationservice.showMessage(
        'Password must be at least 8 characters long and contain both letters and numbers.',
        'alert-error'
      );
      return;
    }

    const user = {
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      phoneNumber: this.phoneNumber,
    };

    this.authService.register(user);
  }
}
