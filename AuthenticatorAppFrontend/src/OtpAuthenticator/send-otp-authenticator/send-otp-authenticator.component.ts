import { Component } from '@angular/core';
import { ServicesService } from '../../app/ApiServices/services.service';
import { error } from 'console';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';
import { CommonErrorMessages } from '../../app/common/CommonErrorMessages';
import { NavBarComponent } from '../../app/nav-bar/nav-bar.component';
import { CommonModule } from '@angular/common';
import { LocalstorageService } from '../../app/storage/localstorage.service';
import { environment } from '../../app/environments/environment';

@Component({
  selector: 'app-send-otp-authenticator',
  standalone: true,
  imports: [CommonModule, NavBarComponent, FormsModule,ReactiveFormsModule, RouterLink],
  templateUrl: './send-otp-authenticator.component.html',
  styleUrl: './send-otp-authenticator.component.css'
})

export class SendOtpAuthenticatorComponent {
  UserEmail :any;
  otpForm!: FormGroup;
  isShow : boolean = false;
  userdata: any;

  constructor(private service: ServicesService,
    private formBuilder: FormBuilder,
    private router: Router, 
    private toaster: ToastrService,
    private localStorage: LocalstorageService)
    { 
      this.userdata = this.localStorage.get(environment.AUTHENTICATION_KEY, {});
    }

  ngOnInit(): void {
    this.UserEmail = localStorage.getItem('UserEmail');
    const IsAuthenticated = this.userdata?.is2FA;
    if (!IsAuthenticated) {
      this.isShow = false; // No data found, assume 2FA is disabled

      this.otpForm = this.formBuilder.group({
        otp: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]]
      });
    }
    else{
      this.isShow = true;
    }
  }

  sendOtp()
  {
    let email = {
      userEmail: this.UserEmail
    }
    this.service.sendOtp(email).subscribe((res:any) =>{
      if(res != null){
        if(res.status == 200){
          this.userdata.is2FA = true;
          this.toaster.success(res.message);
          this.localStorage.put(environment.AUTHENTICATION_KEY, this.userdata);
        }else if(res.status == 500){
          this.toaster.error(res.message);
        }else if(res.status == 404){
          this.toaster.error(res.message);
        }
      }
      else{
        this.toaster.error(CommonErrorMessages.SomethigWentWrong)
      }
    })
  }

  verifyOtp() {
    if (this.otpForm.valid) 
    {
      let data = {
        userEmail: this.UserEmail,
        otp: this.otpForm.value.otp
      }

      this.service.verifyOtp(data).subscribe((res:any) => {
        if(res != null){
          if(res.status == 200){
            this.router.navigate(['otp-authentication-successfully']);
            this.toaster.success(res.message);
          }else if(res.status == 202) {
            this.toaster.error(res.message);
          }else if(res.status == 500) {
            this.toaster.error(res.message);
          }
        }
      })
    }
    else{
      this.toaster.error(CommonErrorMessages.SomethigWentWrong);
    }
  }
}
