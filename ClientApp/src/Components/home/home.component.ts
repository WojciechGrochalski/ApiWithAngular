import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../Services/User.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit  {

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.SendBaseUrl().subscribe();
  }
}
