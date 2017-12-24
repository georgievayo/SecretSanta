import { RequestsService } from './../core/requests.service';
import { Subject } from 'rxjs/Subject';
import { UsersService } from './../core/users.service';
import { Component, OnInit, DoCheck } from '@angular/core';

@Component({
  selector: 'app-requests',
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.css']
})
export class RequestsComponent implements OnInit {
  username: string;
  requests;
  constructor(private requestsService: RequestsService) {
    const currentUser = localStorage.getItem('currentUser');
    this.username = JSON.parse(currentUser).userName;
  }

  ngOnInit() {
    this.requestsService.getUserRequests(this.username, 0, 10, 'asc')
      .subscribe(req => this.requests = req);
  }

  decline(username, requestId) {
    console.log(username);
    console.log(requestId);
    this.requestsService.deleteRequest(username, requestId)
      .subscribe(res => console.log(res));
  }

  accept(username, groupName, requestId) {
    this.requestsService.acceptRequest(username, groupName)
    .subscribe(res => console.log(res));
    this.requestsService.deleteRequest(username, requestId)
    .subscribe(res => res);
  }
}
