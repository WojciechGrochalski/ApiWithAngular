import { Component, Inject, Input, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class CashService {

  baseUrl: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;

  }

 

  GetLastCurrency(iso: string, count: number) {
    return this.http.get<Cash[]>(this.baseUrl + 'cash/' + iso + '/' + count);
  }

  GetDataOnInit() {

    return this.http.get<Cash[]>(this.baseUrl+ 'cash/' );
    
  }
  GetChartData(iso: string, count: number) {
    return this.http.get<number[]>(this.baseUrl + 'cash/' + iso + '/' + count + '/DataChart');
     
  }

  GetChartAskPrice(iso: string, count: number){
    return this.http.get<number[]>(this.baseUrl + 'cash/' + iso + '/' + count + '/AskPrice');
     
    
  }
  GetChartBidPrice(iso: string, count: number) {
    return this.http.get<number[]>(this.baseUrl + 'cash/' + iso + '/' + count + '/BidPrice');
  }

}

export interface Cash {

  name: string;
  code: string;
  askPrice: number;
  bidPrice: number;
  data: string;
}
export interface Chart {

  data: string;
  askPrice: number;
  
  

}




