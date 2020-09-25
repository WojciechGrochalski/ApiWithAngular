import { Component, Inject, Input, Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CashService } from '../cash.service';
import *as apex from 'ng-apexcharts';




@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent {

  series: apex.ApexAxisChartSeries;
  title: apex.ApexTitleSubtitle;
  chart: apex.ApexChart;
  yaxis: apex.ApexYAxis;
  xaxis: apex.ApexXAxis;

  public listofcash: Cash[];
  public result: Cash[];
  public chartData: number[];
  public askPrice: number[] = [];
  public bidPrice: number[] = [];

 

  constructor(http: HttpClient, private cashService: CashService) {

    this.title = {
      text: 'Waluty'
    };

    this.series = [{
      name: "Moja Waluta",
      data: []
    }];

    this.chart = {
      type: 'line',
      toolbar: {
        show: false
      }
    };
  
  }

  ngOnInit(): void {
    this.cashService.GetDataOnInit().subscribe(response => {
      this.listofcash = response;
    }, error => console.error(error));


  }

  TakeLastCurrency(iso: string, count: number) {

    this.cashService.GetLastCurrency(iso, count).subscribe(response => {
      this.result = response;

    }, error => console.error(error));

   
    this.cashService.GetChartAskPrice(iso, count).subscribe(response => {

      this.askPrice = response;
 
    
    }, error => console.error(error));

    this.cashService.GetChartBidPrice(iso, count).subscribe(response => {

      this.bidPrice = response;
      this.UpdateChart(this.askPrice, response, this.chartData);
    }, error => console.error(error));

    this.cashService.GetChartData(iso, count).subscribe(response => {

      this.chartData = response;
      response = response.reverse()
      this.xaxis = {
        categories: response
      }

    }, error => console.error(error));
    
    this.title = {
      text: iso
    };
  

  }



  UpdateChart(askValue: number[], bidValue: number[], date: number[]): void {
  
    askValue = askValue.reverse();
    bidValue = bidValue.reverse();
    this.series = [
    {
      name: "Cena kupna",
      data: askValue
      },
      {
        name: "Cena sprzedaży",
        data: bidValue
      }
    ];
    this.yaxis = {
      title: {
        text: "PLN"
      },
      min: Math.min.apply(null, this.bidPrice) - 0.1,
      max: Math.max.apply(null, this.askPrice) + 0.1
    };
  }


}

interface Cash {

  name: string;
  code: string;
  bidPrice: number;
  askPrice: number;
  data: string;
}
interface Chart {

  data: string;
  askPrice: number;
}



