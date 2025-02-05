import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http'
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicesService {
  isLoggedIn!: boolean;
  constructor(private http:HttpClient) { }

  login(data:any){
    return this.http.post<any>(`${environment.baseApiUrl}Authenticator/login`,data);
  }

  userRegistration(data:any){
    return this.http.post<any>(`${environment.baseApiUrl}Authenticator/user/registration`,data);
  }

  sendOtp(data:any){
    return this.http.post<any>(`${environment.baseApiUrl}Authenticator/send-otp-to-email`,data)
  }

  verifyOtp(data:any){
    return this.http.post<any>(`${environment.baseApiUrl}Authenticator/otpVerification`,data);
  }

  disableAuthentication(data:any){
    return this.http.post<any>(`${environment.baseApiUrl}Authenticator/disable-authentication`,data);
  }
}
