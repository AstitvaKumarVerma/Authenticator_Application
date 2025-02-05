import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TfaSetupDto } from '../../models/TfaSetupDto';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthInputModel } from '../models/auth.model';
import { UserToken } from '../models/user-token.model';
import { environment } from '../environments/environment';
import { LocalstorageService } from '../storage/localstorage.service';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationServiceService {

  private _authorizedAccess!: boolean;
  readonly AUTHENTICATION_KEY = environment.AUTHENTICATION_KEY;

  constructor(private http: HttpClient, private readonly localStorage: LocalstorageService,) { }

  public get AuthorizedAccess(): boolean {
    return this._authorizedAccess;
  }

  public set AuthorizedAccess(value: boolean) {
    this._authorizedAccess = value;
  }

  get userDetail(): UserToken {
    const user = this.localStorage.get(this.AUTHENTICATION_KEY, {});
    return new UserToken(user);
  }
  get isAuthenticated(): boolean {
    return this.userDetail.isAuthenticated ;
  }

  get userName(): string {
    return this.userDetail.userName;
  }

  get fullUserName(): string {
    return this.userDetail.fullUserName;
  }

  getFullUserName(): string {
    return this.userDetail.fullUserName;
  }

  get userID(): number {
    return this.userDetail.userID;
  }

  public getTfaSetup = (email: string) => {
    return this.http.get<any> (`${environment.baseApiUrl}GoogleAuthenticator/generateQrCode?email=${email}`);
  }

  public ValidateTwoFactorPIN(authInputModel: AuthInputModel): any {
    return this.http.post<AuthInputModel>(`${environment.baseApiUrl}GoogleAuthenticator/validateTwoFactorPinAsync`, authInputModel);
  }

  public disableTfa = (email: string) => {
    return this.http.delete<TfaSetupDto> (`${environment.baseApiUrl}GoogleAuthenticator/disableTfa?email=${email}`);
  }
}
