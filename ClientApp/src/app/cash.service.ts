import { Component, Inject, Input, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CashService {

  baseUrl = 'https://localhost:8081/Cash';

  constructor(private http: HttpClient) { }

  GetLastCurrency(message: string, count: number) {
    return this.http.get<Cash[]>('https://localhost:8081/Cash' + '/' + message + '/' + count);
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

