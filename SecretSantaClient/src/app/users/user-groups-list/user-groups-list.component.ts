import { ToastrService } from 'ngx-toastr';
import { RequestsService } from './../../core/requests.service';
import { GroupsService } from './../../core/groups.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-groups-list',
  templateUrl: './user-groups-list.component.html',
  styleUrls: ['./user-groups-list.component.css']
})
export class UserGroupsListComponent implements OnInit {
  currentUserUsername;
  @Input()
  username;
  groups;
  constructor(private groupsService: GroupsService,
    private requestsService: RequestsService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.currentUserUsername = JSON.parse(localStorage.getItem('currentUser')).userName;
    this.groupsService.getUserGroups(this.currentUserUsername, 0, 10)
      .subscribe(res => this.groups = res,
      error => this.toastr.error(error.statusText, 'Error'));
  }

  invite(groupName) {
    this.requestsService.sendRequest(this.username, groupName)
      .subscribe(res => {
        this.toastr.success(`You have sent a request to ${this.username} for ${groupName} group.`, 'Success');
      },
      error => this.toastr.error('Something went wrong. You cannot send this request', 'Error'));
  }
}
