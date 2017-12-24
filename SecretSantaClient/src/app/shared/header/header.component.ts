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
  username: string;
  constructor(private usersService: UsersService) {
  }

  ngOnInit() {
  }

  ngDoCheck() {
    const currentUser = localStorage.getItem('currentUser');
    this.hasLoggedUser = currentUser !== null;
    if (this.hasLoggedUser) {
      this.username = JSON.parse(currentUser).userName;    
    }
  }

  logout() {
    this.usersService.logout();
  }
}
