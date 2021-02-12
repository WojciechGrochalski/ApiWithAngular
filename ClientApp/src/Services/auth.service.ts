import {Inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable, Subscription} from 'rxjs';
import { map} from 'rxjs/operators';
import {CreatedUser} from "../Models/CreatedUser";
import {Token} from "../Models/Token";
import {AuthModel} from "../Models/AuthModel";
import {LoginUser} from "../Models/LoginUser";



@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<CreatedUser>;
  public currentUser: Observable<CreatedUser>;
  BaseUrl: string = '';


  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BaseUrl = baseUrl;
    this.currentUserSubject = new BehaviorSubject<CreatedUser>(JSON.parse(sessionStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): CreatedUser {
    return this.currentUserSubject.value;
  }

  VerifyUser(token: string): Observable<any> {
    let jwtToken = new Token(token);
    return this.http.post(this.BaseUrl + 'User/verify-email', jwtToken, {responseType: 'text'});
  }

  register(user: CreatedUser) {
    return this.http.post<any>(this.BaseUrl + 'User', user);
  }

  login(user: AuthModel) {
    return this.http.post<any>(this.BaseUrl + 'User/login', user)
      .pipe(map(user => {
        if (user) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          sessionStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
        return user;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    sessionStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
