import { Component, Inject, Input, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CashService {

 
 
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) { }

  GetLastCurrency( message: string ) {
    return this.http.get<Cash>('https://localhost:8081/Cash'+'/'+message);
  }

}

export interface Cash {

  name: string;
  code: string;
  bidPrice: string;
  askPrice: string;
  data: string;
}

