import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../Services/User.service";
import {FlashMessagesService} from "angular2-flash-messages";
import {Cash} from "../../Models/Cash";
import {CashService} from "../../Services/cash.service";
import {AuthService} from "../../Services/auth.service";
import {error} from "@angular/compiler/src/util";
import {EMPTY, pipe, throwError} from "rxjs";
import {catchError, tap} from "rxjs/operators";


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  public cash_list: Cash[];
  message:string;
  askPrice: number[] = [];
 bidPrice: number[] = [];
  alert:boolean;
  subscription:boolean;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private router: Router,
              private _authService:AuthService,
              private flashMessagesService: FlashMessagesService,
              private cashService: CashService  ) { }

  async ngOnInit() {
    try {
      this.cash_list = await this.cashService.GetDataOnInit().toPromise();
      let message = this.route.snapshot.paramMap.get('message');
      if(message) {
        this.flashMessagesService.show(message, {cssClass: 'alert-success', timeout: 5000})
      }
    } catch (e) {
      console.error(e);

    }
  }

  SetAlert(){
    this.subscription=false;
    this.alert=true;
  }
  SetSubscription(){
    this.subscription=true;
    this.alert=false;
  }

  AddSubscription(){
    let user=JSON.parse(localStorage.getItem('currentUser'));
      this.userService.GetSubscription(user.id).subscribe(res => {
        this.flashMessagesService.show(res.message, {cssClass: 'alert-success', timeout: 3000})
      })
    }



  SetPriceAlert(iso: string, price:string){
    this.router.navigate(['/set-alert/'+iso+'/'+price]);
  }
Logout(){
    this._authService.logout();
  this.router.navigate(['/']);
}


}
