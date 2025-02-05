import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { QRCodeModule } from 'angularx-qrcode';
import { AuthenticationServiceService } from '../../ApiServices/authentication-service.service';
import { CommonModule } from '@angular/common';
import { Subscription } from "rxjs";
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { LocalstorageService } from '../../storage/localstorage.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { CommonErrorMessages } from '../../common/CommonErrorMessages';
import { CommonSuccessMessages } from '../../common/CommonSuccessMessages';
import { APIResponse } from '../../models/api-response.model';
import { MatInputModule } from '@angular/material/input';
import { AuthInputModel } from '../../models/auth.model';
import { NavBarComponent } from '../../nav-bar/nav-bar.component';

@Component({
  selector: 'app-google-authenticator',
  standalone: true,
  imports: [ QRCodeModule, NavBarComponent, CommonModule, FormsModule, ReactiveFormsModule,MatInputModule, ToastrModule],
  templateUrl: './google-authenticator.component.html',
  styleUrl: './google-authenticator.component.css'
})

export class GoogleAuthenticatorComponent {

  @ViewChild('codeInput') codeInputRef!: ElementRef;
  authForm!: FormGroup;
  qrCode: any;
  qrCodeSrc: any;
  userdata: any;
  validateAuthCodeSubscription!: Subscription;
  returnUrl!: string;
  userEmail!: string;
  isShow : boolean = false;

  constructor(
    private router: Router,
    private authService: AuthenticationServiceService,
    private formBuilder: FormBuilder,
    private localStorage: LocalstorageService,
    private _route: ActivatedRoute,
    private toaster: ToastrService,
  ) {
    this.userdata = this.localStorage.get(environment.AUTHENTICATION_KEY, {});
  }

  ngOnInit() {
    this.userEmail = localStorage.getItem("UserEmail") ?? '';
    const isGoogleAuthenticated = this.userdata?.is2FA;
  
    if(isGoogleAuthenticated){
      this.isShow = true;
    }else{
      this.isShow = false;
      this.createFormGroup();
      this.GenerateSetupCode();
    }
  }

  async GenerateSetupCode() 
  {
    if (this.userdata != null) 
    {
      await this.authService.getTfaSetup(this.userEmail).subscribe((data: APIResponse<any>) => {
        switch (data.statusCode) {
          case 5:
            this.qrCode = data.result.manualEntryKey;
            this.qrCodeSrc = data.result.qrCodeSetupImageUrl;
            break;
          
          case 4:
            this.toaster.error(CommonErrorMessages.SomethigWentWrong);
            break;

          case 7:
            this.toaster.error(CommonErrorMessages.UnableToGenerateQr);
            break;

          default:
            this.toaster.error(CommonErrorMessages.BadRequest);
            break;
        }
      });
    } 
    else
    {
      this.toaster.error(CommonErrorMessages.SomethigWentWrong);
    }
  }

  createFormGroup() {
    this.authForm = new FormGroup({
      Code: new FormControl("", [Validators.required, Validators.pattern("^[0-9]*$")])
    });
    // this.authForm = await this.formBuilder.group({
    //   Code: ["", [Validators.required, Validators.pattern("^[0-9]*$")]],
    // });
  }

  
  ValidateAuthCode() {
    if (this.validateAuthCodeSubscription) {
      this.validateAuthCodeSubscription.unsubscribe();
    }
    this.toaster.toastrConfig.preventDuplicates = true;
    const Code = this.codeInputRef.nativeElement.value;

    var authInputModel = new AuthInputModel();
    authInputModel.Email = this.userEmail;
    authInputModel.Pin = Code;

    this.validateAuthCodeSubscription = this.authService.ValidateTwoFactorPIN(authInputModel)
      .subscribe((data: APIResponse<any>) => {
        switch (data.statusCode) {
          case 2:
            this.userdata.is2FA = true;
            this.localStorage.put(environment.AUTHENTICATION_KEY, this.userdata);
            this.toaster.success(CommonSuccessMessages.TFA_Activated);
            this.authService.AuthorizedAccess = true;
            this.router.navigate(['google-authentication-successfully']);
            break;

          case 3:
            this.toaster.error(CommonErrorMessages.IncorrectPin);
            break;

          case 4:
            this.toaster.error(CommonErrorMessages.SomethigWentWrong);
            break;

          default:
            this.toaster.error(CommonErrorMessages.BadRequest);
            break;
        }
      }, 
      (error: any) => {
        // Handle any errors during subscription (optional)
        this.toaster.error(CommonErrorMessages.SomethigWentWrong);
      });
  }

  reset(): void {
    this.authForm.reset();
  }
}
