import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { appRoutes } from './app.routes'; 
import { ProductsService } from './services/products.service';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule,FooterComponent,HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'project1';
  

}
