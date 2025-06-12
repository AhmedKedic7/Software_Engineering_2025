import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { BrowserModule } from '@angular/platform-browser';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductsComponent } from './pages/products/products.component';
import { LoginComponent } from './pages/login/login.component';
import { ProductComponent } from './pages/product/product.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { CartComponent } from './pages/cart/cart.component';
import { AccountComponent } from './pages/account/account.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserTableComponent } from './components/user-table/user-table.component';
import { ProductModalComponent } from './components/product-modal/product-modal.component';
import { AdminDashboardComponent } from './pages/admin-dashboard/admin-dashboard.component';
import { CheckoutComponent } from './pages/checkout/checkout.component';
import { DataTablesModule } from 'angular-datatables';
import { UserAdminDashboardComponent } from './pages/user-admin-dashboard/user-admin-dashboard.component';
import { FormsModule } from '@angular/forms';
import { OrderAdminDashboardComponent } from './pages/order-admin-dashboard/order-admin-dashboard.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'cart', component: CartComponent },
  { path: 'table', component: UserTableComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'product/:id', component: ProductComponent },
  { path: 'account', component: AccountComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'product', component: ProductComponent },
  { path: 'useradmin', component: UserAdminDashboardComponent },
  { path: 'orderadmin', component: OrderAdminDashboardComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
    NgbModule,
    BrowserAnimationsModule,
    DataTablesModule,
    ProductModalComponent,
    FormsModule,
  ],
  providers: [],
})
export class AppModule {}
