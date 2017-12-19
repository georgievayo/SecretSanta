import { GroupsService } from './../core/groups.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})

export class GroupsComponent implements OnInit {
  groups;
  @Input()
  currentUserUsername;
  constructor(private groupsService: GroupsService) {
  }

  ngOnInit() {
    this.groupsService.getUserGroups(this.currentUserUsername, 0, 5)
      .subscribe(res => this.groups = res);
  }

}
