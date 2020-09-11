import { Component, Inject, Input, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CashService } from '../cash.service';
import { Data } from '@angular/router';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent {


  public listofcash: Cash[];
  public message: string=" ";
  public result: Cash;
  baseUrl: string;
  http: HttpClient;
 

  constructor(http: HttpClient, private cashService: CashService, @Inject('BASE_URL') baseUrl: string) {
    http.get<Cash[]>(baseUrl + 'Cash').subscribe(result => {
      this.listofcash = result;
    }, error => console.error(error));
    this.baseUrl = baseUrl
  }

  TakeLastCurrency(message: string) {

    this.cashService.GetLastCurrency(message).subscribe(response => {
      this.result = response;
    }, error => console.error(error));
 

  }
}

interface Cash {

  name: string;
  code: string;
  bidPrice: string;
  askPrice: string;
  data: string;
}
