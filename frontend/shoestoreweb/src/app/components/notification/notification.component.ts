import { Component } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss'
})
export class NotificationComponent {
  notifications: { message: string; type: string }[] = [];

constructor(private notificationService: NotificationService) {}

ngOnInit() {
  this.notificationService.notifications$.subscribe(
    (notifications) => (this.notifications = notifications)
  );
}

dismissNotification(notification: { message: string; type: string }) {
  this.notificationService.removeNotification(notification);
}

   
}
