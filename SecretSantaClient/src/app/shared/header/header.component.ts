import { Subject } from 'rxjs/Subject';
import { UsersService } from './../../core/users.service';
import { Component, OnInit, DoCheck } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, DoCheck {
  hasLoggedUser: boolean;
  constructor(private usersService: UsersService) {
  }

  ngOnInit() {
  }

  ngDoCheck() {
    this.hasLoggedUser = localStorage.getItem('currentUser') !== null;
  }

  logout() {
    this.usersService.logout();
  }
}
