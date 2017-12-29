import { UsersService } from './../../core/users.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user;
  username;
  isCurrentUser;
  constructor(private usersService: UsersService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params
      .map(params => params['username'])
      .do(name => this.username = name)
      .flatMap(name => {
        const currentUser = JSON.parse(localStorage.getItem('currentUser')).userName;
        if (name === currentUser) {
          this.isCurrentUser = true;
        }

        return this.usersService.getUserProfile(name);
      })
      .subscribe(res => this.user = res);
  }

}
