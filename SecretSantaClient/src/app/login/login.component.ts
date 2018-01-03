import { ToastrService } from 'ngx-toastr';
import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private usersService: UsersService,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit() {
  }

  login(username: string, password: string) {
    this.usersService.login(username, password)
      .subscribe((res) => {
        if (res.ok) {
          this.toastr.success('You are logged in!', 'Success');
          const authToken = res.json();
          localStorage.setItem('currentUser', JSON.stringify(authToken));
          this.router.navigateByUrl('/home');
        }
      },
      (error) => {
        console.log(error.json());
        this.toastr.error('Incorrect username or password!', 'Error');
      });
  }
}
