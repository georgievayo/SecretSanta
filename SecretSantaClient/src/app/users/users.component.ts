import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users;
  order;
  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.order = 'asc';
  }

  addOrder(order) {
    this.order = order;
  }

  search(skip, take, pattern) {
    this.usersService.getUsers(skip, take, this.order, pattern)
      .subscribe(users => {
       this.users = users;
      });
  }

}
