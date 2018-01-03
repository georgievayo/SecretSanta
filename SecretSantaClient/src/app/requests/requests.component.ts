import { ToastrService } from 'ngx-toastr';
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
  take = 10;
  skip;
  changed;
  order;

  constructor(private requestsService: RequestsService,
    private toastr: ToastrService) {
    const currentUser = localStorage.getItem('currentUser');
    this.username = JSON.parse(currentUser).userName;
    this.order = 'asc';
  }

  addOrder() {
    this.changed = !this.changed;
    if (this.changed) {
      this.order = 'desc';
    } else {
      this.order = 'asc';
    }

    this.getRequests();
  }

  ngOnInit() {
    this.getRequests();
  }

  getRequests() {
    this.requestsService.getUserRequests(this.username, 0, 10, this.order)
      .subscribe(req => {
        this.requests = req;
      });
  }

  previous() {
    this.skip -= this.take;
    if (this.skip < 0) {
      this.toastr.warning('This is the beggining of list.', 'Warning');
      this.skip = 0;
    }

    this.requestsService.getUserRequests(this.username, this.skip, this.take, this.order)
      .subscribe(requests => {
        this.requests = requests;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  next() {
    this.skip += this.take;
    this.requestsService.getUserRequests(this.username, this.skip, this.take, this.order)
      .subscribe(requests => {
        if (requests.length <= 0) {
          this.toastr.warning('There is no more users to show.', 'Warning');
          this.previous();
        }
        this.requests = requests;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  decline(username, requestId) {
    this.requestsService.deleteRequest(username, requestId)
      .subscribe(delRes => {
        this.toastr.success('The request was declined!', 'Success');
        this.getRequests();
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  accept(username, groupName, requestId) {
    this.requestsService.acceptRequest(username, groupName)
      .subscribe(res => {
        this.requestsService.deleteRequest(username, requestId)
          .subscribe(delRes => {
            this.toastr.success('The request was accepted!', 'Success');
            this.getRequests();
          },
          error => {
            this.toastr.error(error.statusText, 'Error');
          });
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }
}
