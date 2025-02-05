import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { GoogleAuthenticatorComponent } from './GoogleAuthenticator/google-authenticator/google-authenticator.component';
import { MicrosoftAuthenticatorComponent } from './microsoft-authenticator/microsoft-authenticator.component';
import { SendOtpAuthenticatorComponent } from '../OtpAuthenticator/send-otp-authenticator/send-otp-authenticator.component';
import { LoginComponent } from './login/login.component';
import { SuccessComponent } from './GoogleAuthenticator/success/success.component';
import { OtpAuthenticationSuccessComponent } from '../OtpAuthenticator/otp-authentication-success/otp-authentication-success.component';
import { authGuard } from './guard/auth.guard';

export const routes: Routes = [
    {
        path:'',
        component: LoginComponent
    },
    {
        path:'home',
        component: HomeComponent, 
        canActivate: [authGuard]
    },
    {
        path:'google-authentication',
        component: GoogleAuthenticatorComponent, 
        canActivate: [authGuard]
    },
    {
        path:'microsoft-authentication',
        component: MicrosoftAuthenticatorComponent, 
        canActivate: [authGuard]
    },
    {
        path:'sendOtp-authentication',
        component: SendOtpAuthenticatorComponent, 
        canActivate: [authGuard]
    },
    {
        path:'google-authentication-successfully',
        component: SuccessComponent, 
        canActivate: [authGuard]
    },
    {
        path:'otp-authentication-successfully',
        component: OtpAuthenticationSuccessComponent, 
        canActivate: [authGuard]
    }
];
