import { CreateGroupComponent } from './../groups/create-group/create-group.component';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [
    HeaderComponent,
    FooterComponent,
    CreateGroupComponent
],
exports: [
  CommonModule,
  RouterModule,
  HeaderComponent,
  FooterComponent,
  CreateGroupComponent
]
})
export class SharedModule { }