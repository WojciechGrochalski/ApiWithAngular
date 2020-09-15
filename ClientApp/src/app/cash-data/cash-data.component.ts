import { Component, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CashService } from '../cash.service';


@Component({
  selector: 'app-cash-data',
  templateUrl: './cash-data.component.html',
  styleUrls: ['./cash-data.component.css']
})
export class CashDataComponent {


  public listofcash: Cash[];
  public message = ' ';
  public result: Cash[];

  http: HttpClient;


  constructor(http: HttpClient, private cashService: CashService, @Inject('BASE_URL') baseUrl: string) {
    // http.get<Cash[]>(baseUrl + 'Cash').subscribe(result => {
    //  this.listofcash = result;
    // }, error => console.error(error));
    // this.baseUrl = baseUrl
  }

  // tslint:disable-next-line:use-lifecycle-interface
  ngOnInit(): void {
    this.cashService.GetDataOnInit().subscribe(response => {
      this.listofcash = response;
    }, error => console.error(error));
  }

  TakeLastCurrency(message: string, count: number) {

    this.cashService.GetLastCurrency(message, count).subscribe(response => {
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
