import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderAdminDashboardComponent } from './order-admin-dashboard.component';

describe('OrderAdminDashboardComponent', () => {
  let component: OrderAdminDashboardComponent;
  let fixture: ComponentFixture<OrderAdminDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderAdminDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderAdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
