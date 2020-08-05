import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CashService {

   path: string;
  @Inject('BASE_URL') baseUrl: string;
  constructor(private httpClient: HttpClient  ) {}

  send( message: string): Observable<any>  {

    return this.httpClient.post('http://localhost:5001/Cash', message);
  }
}
