import { Component, Inject, Input, Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import *as apex from 'ng-apexcharts';
import { CashService } from '../app/cash.service';




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
  public chartData: string[]=[];
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

   async TakeLastCurrency(iso: string, count: number) {

    this.cashService.GetLastCurrency(iso, count).subscribe(response => {
      this.result = response;

    }, error => console.error(error));

   
    this.cashService.GetChartAskPrice(iso, count).subscribe(response => {

      this.askPrice = response;
 
    
    }, error => console.error(error));

    this.cashService.GetChartBidPrice(iso, count).subscribe(response => {

      this.bidPrice = response;
     
    }, error => console.error(error));

    this.cashService.GetChartData(iso, count).subscribe(response => {

      this.chartData = response;

    }, error => console.error(error));
    
   
     await new Promise(resolve => setTimeout(resolve, 100));

     this.UpdateChart(this.askPrice, this.bidPrice, this.chartData);
     this.title = {
       text: iso
     };
     await new Promise(resolve => setTimeout(resolve, 300));
  }



  UpdateChart(askValue: number[], bidValue: number[], date: string[]): void {
  
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
    this.xaxis = {
      categories: date
    }
    this.yaxis = {
      title: {
        text: "PLN"
      },
      min: Math.min.apply(null, this.bidPrice) - Math.min.apply(null, this.bidPrice)/100,
      max: Math.max.apply(null, this.askPrice) + Math.max.apply(null, this.askPrice)/100
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


