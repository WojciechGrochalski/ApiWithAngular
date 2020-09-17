import { Component, Inject, Input, Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CashService } from '../cash.service';
import { BrowserModule } from '@angular/platform-browser';





@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent {




  public listofcash: Cash[];
  public result: Cash[];
  public chartData: string[] = [];
  public askPrice: number[] = [];
  public bidPrice: number[] = [];



  constructor(http: HttpClient, private cashService: CashService) { }




  ngOnInit(): void {
    this.cashService.GetDataOnInit().subscribe(response => {
      this.listofcash = response;
    }, error => console.error(error));


  }

  TakeLastCurrency(iso: string, count: number) {

    this.cashService.GetLastCurrency(iso, count).subscribe(response => {
      this.result = response;
    }, error => console.error(error));

    this.cashService.GetChartData(iso, count).subscribe(response => {
      this.chartData = response;

    }, error => console.error(error));

    this.cashService.GetChartAskPrice(iso, count).subscribe(response => {
      this.askPrice = response;



    }, error => console.error(error));

    this.cashService.GetChartBidPrice(iso, count).subscribe(response => {

      this.bidPrice = response;
    }, error => console.error(error));

  }
}

interface Cash {

  name: string;
  code: string;
  bidPrice: number;
  askPrice: number;
  data: string;
}



  //title = 'Average Temperatures of Cities';
  //type = 'LineChart';
  //data = [
  //  ["Jan", 7.0, -0.2, -0.9, 3.9],
  //  ["Feb", 6.9, 0.8, 0.6, 4.2],
  //  ["Mar", 9.5, 5.7, 3.5, 5.7],
  //  ["Apr", 14.5, 11.3, 8.4, 8.5],
  //  ["May", 18.2, 17.0, 13.5, 11.9],
  //  ["Jun", 21.5, 22.0, 17.0, 15.2],
  //  ["Jul", 25.2, 24.8, 18.6, 17.0],
  //  ["Aug", 26.5, 24.1, 17.9, 16.6],
  //  ["Sep", 23.3, 20.1, 14.3, 14.2],
  //  ["Oct", 18.3, 14.1, 9.0, 10.3],
  //  ["Nov", 13.9, 8.6, 3.9, 6.6],
  //  ["Dec", 9.6, 2.5, 1.0, 4.8]
  //];
  //columnNames = ["Month", "Tokyo", "New York", "Berlin", "Paris"];
  //options = {
  //  hAxis: {
  //    title: 'Month'
  //  },
  //  vAxis: {
  //    title: 'Temperature'
  //  },
  //};
  //width = 550;
  //height = 400;
