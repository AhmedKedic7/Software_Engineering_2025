import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private notificationsSource = new BehaviorSubject<{ message: string; type: string }[]>([]);
  notifications$ = this.notificationsSource.asObservable();

  showMessage(
    message: string,
    alertType: 'alert-success' | 'alert-error' | 'alert-info' | 'alert-warning',
    duration = 3000
  ) {
    const notification = { message, type: alertType };
    const currentNotifications = this.notificationsSource.value;

     
    this.notificationsSource.next([...currentNotifications, notification]);

     
    setTimeout(() => this.removeNotification(notification), duration);
  }

  removeNotification(notification: { message: string; type: string }) {
    const currentNotifications = this.notificationsSource.value;
    this.notificationsSource.next(
      currentNotifications.filter((n) => n !== notification)
    );
  }
}
