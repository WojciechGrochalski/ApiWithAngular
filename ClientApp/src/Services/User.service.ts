import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {BaseUrlModel} from "../Models/BaseUrlModel";
import {Token} from "../Models/Token";
import {User} from "../Models/User";
import {map} from "rxjs/operators";
import {AuthModel} from "../Models/AuthModel";
import {Remainder} from "../Models/Remainder";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  BaseUrl: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BaseUrl = baseUrl;
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }
  VerifyUser(token: string) : Observable<any> {
    let jwtToken=new Token(token);
    return this.http.post(this.BaseUrl + 'User/verify-email',jwtToken,{responseType: 'text'});
  }

  SendBaseUrl(): Observable<any>{
    let url = new BaseUrlModel(this.BaseUrl);
    return this.http.post(this.BaseUrl + 'User/baseUrl', url );

  }

  register(user: User) {
    return this.http.post<any>(this.BaseUrl + 'User', user);
  }
  login(user:AuthModel) {
    return this.http.post<any>(this.BaseUrl + 'User/login',user)
      .pipe(map(user => {
        if (user ) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
        return user;
      }));
  }
  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
  GetSubscribtion(userID: number){
    return this.http.get<any>(this.BaseUrl + 'User/sub/'+userID);
  }
  SetAlert(alert :Remainder){
    return this.http.post<any>(this.BaseUrl + 'User/addAlert', alert);
  }
}
