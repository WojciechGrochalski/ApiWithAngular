import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../Services/User.service";
import {FlashMessagesService} from "angular2-flash-messages";


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  UserIsVerify:boolean=false;
  message:string;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private router: Router,
              private flashMessagesService: FlashMessagesService ) { }

  ngOnInit() {
    let token= this.route.snapshot.queryParamMap.get('token');
      console.log("token: ",token);

      this.userService.VerifyUser(token).subscribe(res => {
        this.message=res;
        this.UserIsVerify=true;
        this.router.navigate([''])
        this.flashMessagesService.show('Profil został zaktualizowany', {cssClass: 'alert-success', timeout: 3000})
        });

  }

}
