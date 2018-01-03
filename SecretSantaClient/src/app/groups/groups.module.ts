import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsComponent } from './groups.component';
import { GroupComponent } from './group/group.component';
import { CreateGroupComponent } from './create-group/create-group.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    GroupComponent,
]
})
export class GroupsModule { }