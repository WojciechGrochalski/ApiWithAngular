import { Component, Inject, Input, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class CashService {

  baseUrl: string = 'https://localhost:5001/Cash';

  constructor(private http: HttpClient) { }

 

  GetLastCurrency(iso: string, count: number) {
    return this.http.get<Cash[]>(this.baseUrl + '/' + iso + '/' + count);
  }

  GetDataOnInit() {

    return this.http.get<Cash[]>(this.baseUrl );
    
  }
  GetChartData(iso: string, count: number) {
    return this.http.get<string[]>(this.baseUrl + '/' + iso + '/' + count +'/DataChart');
  }

  GetChartAskPrice(iso: string, count: number){
    return this.http.get<number[]>(this.baseUrl + '/' + iso + '/' + count + '/AskPrice');
     
    
  }
  GetChartBidPrice(iso: string, count: number) {
    return this.http.get<number[]>(this.baseUrl + '/' + iso + '/' + count + '/BidPrice');
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
  bidPrice: number;
  

}




