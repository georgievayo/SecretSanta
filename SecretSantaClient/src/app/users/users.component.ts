import { ToastrService } from 'ngx-toastr';
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
  skip;
  take = 4;
  shownButtons;
  constructor(private usersService: UsersService, private toastr: ToastrService) { }

  ngOnInit() {
    this.order = 'asc';
    this.shownButtons = false;
    this.skip = 0;
  }

  addOrder(order) {
    this.order = order;
  }

  search(pattern) {
    this.usersService.getUsers(0, this.take, this.order, pattern)
      .subscribe(users => {
        if (users.length > 0) {
          this.shownButtons = true;
        }
        this.users = users;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  previous (pattern) {
    this.skip -= this.take;
    if (this.skip < 0) {
      this.toastr.warning('This is the beggining of list.', 'Warning');
      this.skip = 0;
    }

    this.usersService.getUsers(this.skip, this.take, this.order, pattern)
      .subscribe(users => {
        this.users = users;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  next(pattern) {
    this.skip += this.take;
    this.usersService.getUsers(this.skip, this.take, this.order, pattern)
      .subscribe(users => {
        if (users.length <= 0) {
          this.toastr.warning('There is no more users to show.', 'Warning');
          this.previous(pattern);
        }
        this.users = users;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

}
