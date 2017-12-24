import { RequestsService } from './../../core/requests.service';
import { GroupsService } from './../../core/groups.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-groups-list',
  templateUrl: './user-groups-list.component.html',
  styleUrls: ['./user-groups-list.component.css']
})
export class UserGroupsListComponent implements OnInit {

  @Input()
  username;
  groups;
  constructor(private groupsService: GroupsService, private requestsService: RequestsService) { }

  ngOnInit() {
    this.groupsService.getUserGroups(this.username, 0, 10)
      .subscribe(res => this.groups = res);
  }

  invite(groupName) {
    this.requestsService.sendRequest(this.username, groupName)
      .subscribe(res => console.log(res));
  }
}
