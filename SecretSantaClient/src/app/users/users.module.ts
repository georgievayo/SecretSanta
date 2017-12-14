import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './users.component';
import { ProfileComponent } from './profile/profile.component';
import { UserShortComponent } from './user-short/user-short.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [UsersComponent,
    ProfileComponent,
    UserShortComponent
]
})
export class UsersModule { }