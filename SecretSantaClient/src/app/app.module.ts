import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { GroupsModule } from './groups/groups.module';
import { RequestsModule } from './requests/requests.module';
import { UsersModule } from './users/users.module';
import { AppRoutes } from './app.routing';
import { SharedModule } from './shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './signup/signup.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SignupComponent,
    LoginComponent
],
  imports: [
    BrowserModule,
    SharedModule,
    UsersModule,
    RequestsModule,
    GroupsModule,
    AppRoutes,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
