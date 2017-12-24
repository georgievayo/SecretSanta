import { SharedModule } from './../shared/shared.module';
import { RouterModule } from '@angular/router';
import { CoreModule } from './../core/core.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './users.component';
import { ProfileComponent } from './profile/profile.component';
import { UserShortComponent } from './user-short/user-short.component';
import { UserGroupsListComponent } from './user-groups-list/user-groups-list.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedModule
  ],
  declarations: [UsersComponent,
    ProfileComponent,
    UserShortComponent,
    UserGroupsListComponent
]
})
export class UsersModule { }