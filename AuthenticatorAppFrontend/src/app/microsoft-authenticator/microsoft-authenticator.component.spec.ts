import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MicrosoftAuthenticatorComponent } from './microsoft-authenticator.component';

describe('MicrosoftAuthenticatorComponent', () => {
  let component: MicrosoftAuthenticatorComponent;
  let fixture: ComponentFixture<MicrosoftAuthenticatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MicrosoftAuthenticatorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MicrosoftAuthenticatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
