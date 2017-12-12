import { Subject } from 'rxjs/Subject';
import { UsersService } from './../../core/users.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  hasLoggedUser: boolean;
  constructor(private usersService: UsersService) {
  }

  ngOnInit() {
  }
}
