import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtpAuthenticationSuccessComponent } from './otp-authentication-success.component';

describe('OtpAuthenticationSuccessComponent', () => {
  let component: OtpAuthenticationSuccessComponent;
  let fixture: ComponentFixture<OtpAuthenticationSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OtpAuthenticationSuccessComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OtpAuthenticationSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
