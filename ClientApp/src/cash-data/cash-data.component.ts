import {Component, Inject, Input, Injectable, ViewChild} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import *as apex from 'ng-apexcharts';
import {CashService} from '../app/cash.service';
import { Cash } from '../Models/Cash';

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

  public cash_list: Cash[];
  public result: Cash[];
  public chartData: string[] = [];
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

  async ngOnInit(): Promise<void> {
    try {
      this.cash_list = await this.cashService.GetDataOnInit().toPromise();
    } catch (e) {
      console.error(e);

    }
    return;
  }

  async TakeLastCurrency(iso: string, count: string) {
  let amoutOfCurrency: number = +count;
    try {
      this.result = await this.cashService.GetLastCurrency(iso, amoutOfCurrency).toPromise();

      this.askPrice = await this.cashService.GetChartAskPrice(iso, amoutOfCurrency).toPromise();

      this.bidPrice = await this.cashService.GetChartBidPrice(iso, amoutOfCurrency).toPromise();

      this.chartData = await this.cashService.GetChartData(iso, amoutOfCurrency).toPromise();
    } catch (e) {
      console.error(e);
    }
    this.UpdateChart(this.askPrice, this.bidPrice, this.chartData);
    this.title = {
      text: iso
    };

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
      min: Math.min.apply(null, this.bidPrice) - Math.min.apply(null, this.bidPrice) / 100,
      max: Math.max.apply(null, this.askPrice) + Math.max.apply(null, this.askPrice) / 100
    };
    console.log('update Chart');

  }


}

//interface Cash {

//  name: string;
//  code: string;
//  bidPrice: number;
//  askPrice: number;
//  data: string;
//}

//interface Chart {

//  data: string;
//  askPrice: number;
//}



