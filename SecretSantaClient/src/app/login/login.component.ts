import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
  }

  login(username: string, password: string) {
    this.usersService.login(username, password);
    this.router.navigateByUrl('/home');
  }
}
