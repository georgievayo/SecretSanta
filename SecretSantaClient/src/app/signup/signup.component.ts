import { UsersService } from './../core/users.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  constructor(private usersService: UsersService) { }

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
    console.log(user);
    this.usersService.signup(user);
  }

}
