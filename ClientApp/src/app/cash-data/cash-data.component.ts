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
  public chartData: Chart[];
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
      type: 'line'
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

    //this.cashService.GetChartData(iso, count).subscribe(response => {

    //  this.chartData = response;
    //}, error => console.error(error));

    this.cashService.GetChartAskPrice(iso, count).subscribe(response => {

      this.askPrice = response;
      this.UpdateChart(response);
    
    }, error => console.error(error));

    this.cashService.GetChartBidPrice(iso, count).subscribe(response => {

      this.bidPrice = response;
    }, error => console.error(error));
    this.series = [{
      name: iso,
      data: []
    }];
    
    this.title = {
      text: iso
    };


  }



  UpdateChart(input: number[]): void {
  
    input = input.reverse();
    this.series = [{
      name: "Moja Waluta",
      data: input
    }];
    this.yaxis = {
      min: Math.min.apply(null, this.askPrice) - 0.01,
      max: Math.max.apply(null, this.askPrice) + 0.01
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



