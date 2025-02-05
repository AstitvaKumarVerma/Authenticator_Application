import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendOtpAuthenticatorComponent } from './send-otp-authenticator.component';

describe('SendOtpAuthenticatorComponent', () => {
  let component: SendOtpAuthenticatorComponent;
  let fixture: ComponentFixture<SendOtpAuthenticatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SendOtpAuthenticatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SendOtpAuthenticatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
