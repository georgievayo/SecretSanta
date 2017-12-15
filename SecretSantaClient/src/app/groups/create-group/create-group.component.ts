import { GroupsService } from './../../core/groups.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css']
})
export class CreateGroupComponent implements OnInit {

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
  }

  create(groupName) {
    this.groupsService.createGroup(groupName)
      .subscribe(res => console.log(res));
  }
}
