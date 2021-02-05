import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {BaseUrlModel} from "../Models/BaseUrlModel";
import {Token} from "../Models/Token";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  BaseUrl: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BaseUrl = baseUrl;
  }

  VerifyUser(token: string) : Observable<any> {
    let jwtToken=new Token(token);
    return this.http.post(this.BaseUrl + 'User/verify-email',jwtToken,{responseType: 'text'});
  }

  SendBaseUrl(): Observable<any>{
    let url = new BaseUrlModel(this.BaseUrl);
    return this.http.post(this.BaseUrl + 'User/baseUrl', url );

  }
}
