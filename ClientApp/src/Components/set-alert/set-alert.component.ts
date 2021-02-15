import { Component, OnInit } from '@angular/core';
import {UserService} from "../../Services/User.service";
import {ActivatedRoute} from "@angular/router";
import {Remainder} from "../../Models/Remainder";
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

      this.iso = this.route.snapshot.paramMap.get('iso');
       let price= this.route.snapshot.paramMap.get('price');
       if(price=='More'){
         this.priceLessThan=false;
       }
       else{
         this.priceLessThan=true;
       }
  }

  AddAlert(value: string, date: string){
    let alert = new Remainder();
    if(this.priceLessThan){
      alert.Price='More';
      alert.AskPrice= +value;
    }
    else{
      alert.Price='Less';
      alert.BidPrice= +value;
    }
    alert.EndDateOfAlert=new Date(date);
    alert.Code=this.iso;
    alert.UserID=  this.userID;

    this.userService.SetAlert(alert).subscribe(res =>{
        this.flashMessagesService.show(res.message, {cssClass: 'alert-success', timeout: 5000})
    },
      error => {
        this.flashMessagesService.show(error.error.message, {cssClass: 'alert-danger', timeout: 3000})
      });
  }
}
