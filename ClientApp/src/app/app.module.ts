import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { FlashMessagesModule } from 'angular2-flash-messages';

import { NgApexchartsModule } from 'ng-apexcharts';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavMenuComponent } from '../Components/nav-menu/nav-menu.component';
import { HomeComponent } from '../Components/home/home.component';
import { CashDataComponent } from '../Components/cash-data/cash-data.component';
import {UserProfileComponent} from "../Components/user-profile/user-profile.component";
import {ForgetPasswordComponent} from "../Components/forget-password/forget-password.component";
import {LogInComponent} from "../Components/Login/log-in.component";
import {RegisterComponent} from "../Components/register/register.component";



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CashDataComponent,
    UserProfileComponent,
    ForgetPasswordComponent,
    LogInComponent,
    RegisterComponent

  ],
  imports: [
    ReactiveFormsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgApexchartsModule,
    FlashMessagesModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: '', component: HomeComponent },
      { path: 'cash-data', component: CashDataComponent },
      { path: 'user-profile', component: UserProfileComponent },
      { path: 'login', component: LogInComponent},
      { path: 'register', component: RegisterComponent},
      { path: 'forgot-password', component: ForgetPasswordComponent},
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
