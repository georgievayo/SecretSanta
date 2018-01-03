import { Router } from '@angular/router';
import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  constructor(private usersService: UsersService,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit() {
  }

  signup(username, email, password, displayName, address, age, phoneNumber, interests) {
    const user = {
      username: username,
      password: password,
      email: email,
      displayName: displayName,
      age: age,
      address: address,
      phoneNumber: phoneNumber,
      interests: interests
    };

    this.usersService.signup(user)
      .subscribe(res => {
        this.toastr.success('Successful sign up!', 'Success');
        this.router.navigateByUrl('/');
      },
        error => this.toastr.error(error.json().Message, 'Error',
        { tapToDismiss: true, timeOut: 1000, positionClass: 'toast-top-right'}));
  }

}
