import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../Services/User.service";
import {FlashMessagesService} from "angular2-flash-messages";
import {Cash} from "../../Models/Cash";
import {CashService} from "../../Services/cash.service";


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  public cash_list: Cash[];
  message:string;
  public askPrice: number[] = [];
  public bidPrice: number[] = [];
  alert:boolean;
  subscribtion:boolean;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private router: Router,
              private flashMessagesService: FlashMessagesService,
              private cashService: CashService  ) { }

  async ngOnInit() {
    try {
      this.cash_list = await this.cashService.GetDataOnInit().toPromise();
    } catch (e) {
      console.error(e);

    }
    // let token= this.route.snapshot.queryParamMap.get('token');
    //   console.log("token: ",token);
    //
    //   this.userService.VerifyUser(token).subscribe(res => {
    //     this.message=res;
    //     this.router.navigate([''])
    //     this.flashMessagesService.show('Profil został zaktualizowany', {cssClass: 'alert-success', timeout: 3000})
    //     });
  }

  SetAlert(){
    this.subscribtion=false;
    this.alert=true;
  }
  SetSubscribtion(){
    this.subscribtion=true;
    this.alert=false;
  }

  GetSubscribtion(){
    let user=JSON.parse(localStorage.getItem('currentUser'));
    console.log(user);
    this.userService.GetSubscribtion(user.id).subscribe( res =>
    {
      this.flashMessagesService.show(res.message, {cssClass: 'alert-success', timeout: 3000})
    });
  }
  SetPriceAlert(iso: string, price:string){
    this.router.navigate(['/set-alert/'+iso+'/'+price]);
  }



}
