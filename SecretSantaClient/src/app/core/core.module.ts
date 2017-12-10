import { UsersService } from './users.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';

@NgModule({
  imports: [
    HttpModule
  ],
  declarations: [],
  providers: [UsersService]
})
export class CoreModule { }