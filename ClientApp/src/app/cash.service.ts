import { Component, Inject, Input, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CashService {

  baseUrl: string = 'https://localhost:5001/Cash';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) { }



  GetLastCurrency(message: string, count: number) {
    return this.http.get<Cash[]>(this.baseUrl + '/' + message + '/' + count);
  }

  GetDataOnInit() {

    return this.http.get<Cash[]>(this.baseUrl );
    
  }

}

export interface Cash {

  name: string;
  code: string;
  bidPrice: string;
  askPrice: string;
  data: string;
}

