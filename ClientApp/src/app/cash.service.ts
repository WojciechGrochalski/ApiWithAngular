import { Component, Inject, Input, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CashService {

  path: string;
  @Injectable()
  @Inject('BASE_URL') baseUrl: string;
  constructor(private http: HttpClient) { }

  GetLastCurrency(baseUrl: string) {
    return this.http.get<Cash>(baseUrl + 'Cash');
  }

}

export interface Cash {

  name: string;
  code: string;
  bidPrice: string;
  askPrice: string;
  data: string;
}

