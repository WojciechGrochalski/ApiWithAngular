import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BaseUrlModel} from "../Models/BaseUrlModel";
import {Remainder} from "../Models/Remainder";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {


  BaseUrl: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
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
}
