import { ActivatedRoute } from '@angular/router';
import { GroupsService } from './../../core/groups.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {
  group;
  groupName;
  constructor(private groupsService: GroupsService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params
    .map(params => params['name'])
    .do(name => this.groupName = name)
    .flatMap(name => {
      return this.groupsService.getGroup(name);
    })
    .subscribe(res => this.group = res);
  }

}
