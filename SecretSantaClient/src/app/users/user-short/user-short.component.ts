import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-short',
  templateUrl: './user-short.component.html',
  styleUrls: ['./user-short.component.css']
})
export class UserShortComponent implements OnInit {

  @Input()
  user;
  constructor() { }

  ngOnInit() {
  }

}
