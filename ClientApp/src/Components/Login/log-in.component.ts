import {Component, Input, OnInit} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UserService} from "../../Services/User.service";
import {FlashMessagesService} from "angular2-flash-messages";
import {AuthModel} from "../../Models/AuthModel";


@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {


  ////
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;

  @Input() public disabled: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private flashMessagesService: FlashMessagesService) {

    //redirect to home if already logged in
    // if (this.userService.currentUserValue) {
    //   this.router.navigate(['/']);
    // }

  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  }

  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    let user = new AuthModel(this.f.username.value, this.f.password.value);
    this.loading = true;
    this.userService.login(user)
      .subscribe(res => {
          if (res) {
            this.router.navigate([this.returnUrl]);
          } else {
            this.flashMessagesService.show('Nieprawidłowe dane', {cssClass: 'alert-danger', timeout: 3000});
            this.loading = false;
          }
        },
        error => {
          this.flashMessagesService.show(error.error.message, {cssClass: 'alert-danger', timeout: 3000});
          this.loading = false;
        });
  }
}
