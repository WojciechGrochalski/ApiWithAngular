import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHandler, HttpHeaders, HttpRequest} from "@angular/common/http";
import {BaseUrlModel} from "../Models/BaseUrlModel";
import {Remainder} from "../Models/Remainder";
import {Observable} from "rxjs";
import {Token} from "../Models/Token";
import {NewTokens} from "../Models/NewTokens";
import {JwtHelperService} from "@auth0/angular-jwt";
import {tap} from "rxjs/operators";

const jwtHelper= new JwtHelperService();

@Injectable({
  providedIn: 'root'
})
export class UserService {


  BaseUrl: string = '';

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string ) {
    this.BaseUrl = baseUrl;
  }

  SendBaseUrl(): Observable<any>{
    let url = new BaseUrlModel(this.BaseUrl);
    return this.http.post(this.BaseUrl + 'User/baseUrl', url );
  }

  GetSubscription(userID: number){
    return this.http.get<any>(this.BaseUrl + 'User/sub/'+userID);
  }
  SetAlert(alert :Remainder){
    return this.http.post<any>(this.BaseUrl + 'User/addAlert', alert);
  }
  VerifyUser(token: string): Observable<any> {
    let jwtToken = new Token(token);
    return this.http.post(this.BaseUrl + 'User/verify-email', jwtToken);
  }
  VerifyPasswordToken(token: string): Observable<any> {
   let jwtToken = new Token(token);
    return this.http.post(this.BaseUrl + 'User/verify-resetpassword', jwtToken);
  }
 RefreshToken() {
    return this.http.get<NewTokens>(this.BaseUrl + 'User/refreshToken').pipe(tap
    ((tokens:any)=>{
      this.storeJwtToken(tokens);
    }));
  }
  private storeJwtToken(tokens: any) {
    localStorage.setItem("accessToken", tokens.accessToken);
    localStorage.setItem("refreshToken", tokens.refreshToken);
  }

  CheckThatTokenNotExpired(token: string):boolean{
    const isExpired =jwtHelper.isTokenExpired(token);
    if (isExpired) {
      return true;
    }
    return false;
  }
}
