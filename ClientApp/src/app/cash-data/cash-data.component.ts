import { Component,  Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent  {

  public cash: Cash[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Cash[]>(baseUrl + 'cashmodel').subscribe(result => {
      this.cash = result;
    }, error => console.error(error));
  }

  //ngOnInit() {
  //}

}

interface Cash {


name: string;
code: string;
bidPrice: string;
askPrice: string;
data: string;
}
