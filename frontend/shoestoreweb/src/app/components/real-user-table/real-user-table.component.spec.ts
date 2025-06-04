import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RealUserTableComponent } from './real-user-table.component';

describe('RealUserTableComponent', () => {
  let component: RealUserTableComponent;
  let fixture: ComponentFixture<RealUserTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RealUserTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RealUserTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
