import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.getUsers(0, 10, 'asc').subscribe(users => console.log(users));
  }

}
