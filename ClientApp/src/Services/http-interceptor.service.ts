import {Inject, Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserService} from "./User.service";


@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {

  BaseUrl: string = '';

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private userService: UserService) {
    this.BaseUrl = baseUrl;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log(request.url);
    if (request.url.includes("cash") || request.url.includes("baseUrl") ||request.url.includes("login")) {
      return next.handle(request);
    } else {
      if (request.url.includes("refreshToken")) {
        console.log("refresh")
        let refreshToken = localStorage.getItem('refreshToken');
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${refreshToken}`,
          }
        });
        return next.handle(request);
      }
      else {
        console.log("acc")
        let accessToken = localStorage.getItem('accessToken');
        if (accessToken) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${accessToken}`,
            }
          });
          return next.handle(request);
        }
      }
    }

  }
}





