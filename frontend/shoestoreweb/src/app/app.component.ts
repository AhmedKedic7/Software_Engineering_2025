import { Component } from '@angular/core';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routes'; 
import { ProductsService } from './services/products.service';
import { NotificationComponent } from './components/notification/notification.component';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HeaderComponent,FooterComponent,RouterModule,NotificationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'project1';
  

}
