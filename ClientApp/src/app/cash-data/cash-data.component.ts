import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {CashService} from '../cash.service';


@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent  {

  public listofcash: Cash[];
  public message: string;

  constructor(http: HttpClient, private cashService: CashService, @Inject('BASE_URL') baseUrl: string) {
    http.get<Cash[]>(baseUrl + 'Cash').subscribe(result => {
      this.listofcash = result;
    }, error => console.error(error));
  }

  SendToBackend(message: string) {
    this.cashService.send(message )
      .subscribe(data => console.log("Succeeded, result = " + data),
        (err) => console.error("Failed! " + err));
  }
}

interface Cash {

name: string;
code: string;
bidPrice: string;
askPrice: string;
data: string;
}
