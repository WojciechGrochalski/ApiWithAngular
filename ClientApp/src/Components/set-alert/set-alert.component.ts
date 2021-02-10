import { Component, OnInit } from '@angular/core';
import {UserService} from "../../Services/User.service";
import {ActivatedRoute} from "@angular/router";
import {Remainder} from "../../Models/Remainder";
import {User} from "../../Models/User";
import {FlashMessagesService} from "angular2-flash-messages";

@Component({
  selector: 'app-set-alert',
  templateUrl: './set-alert.component.html',
  styleUrls: ['./set-alert.component.css']
})
export class SetAlertComponent implements OnInit {

  priceLessThan:boolean;
  iso:string;
  userID:number;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private flashMessagesService: FlashMessagesService  ) { }

  ngOnInit() {
    this.userID= +JSON.parse(localStorage.getItem('currentUser')).id;
    console.log(this.userID)
    let price="";
      this.iso = this.route.snapshot.paramMap.get('iso');
       price= this.route.snapshot.paramMap.get('price');
      console.log("iso: ",this.iso);

       console.log("price: ",price);
       if(price=='less'){
         this.priceLessThan=false;
       }
       else{
         this.priceLessThan=true;
       }
  }

  AddAlert(value:number){
    let alert = new Remainder();
    if(this.priceLessThan){
      alert.AskPrice= +value;
      alert.Price='More';
    }
    else{
      alert.Price='Less';
      alert.BidPrice= +value;
      console.log('bidPrice '+alert.BidPrice)
    }
    alert.Currency=this.iso;
    alert.UserID=  this.userID;

    this.userService.SetAlert(alert).subscribe(res =>{
        this.flashMessagesService.show(res.message, {cssClass: 'alert-success', timeout: 5000})
    },
      error => {
        this.flashMessagesService.show(error.error.message, {cssClass: 'alert-danger', timeout: 3000})
      });
  }
}
