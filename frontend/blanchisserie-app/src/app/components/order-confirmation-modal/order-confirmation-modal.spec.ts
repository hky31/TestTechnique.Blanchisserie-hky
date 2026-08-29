import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderConfirmationModal } from './order-confirmation-modal';

describe('OrderConfirmationModal', () => {
  let component: OrderConfirmationModal;
  let fixture: ComponentFixture<OrderConfirmationModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderConfirmationModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderConfirmationModal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
