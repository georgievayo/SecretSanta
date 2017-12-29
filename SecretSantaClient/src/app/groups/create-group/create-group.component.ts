import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GroupsService } from './../../core/groups.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css']
})
export class CreateGroupComponent implements OnInit {

  constructor(private groupsService: GroupsService,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit() {
  }

  create(groupName) {
    this.groupsService.createGroup(groupName)
      .subscribe(res => {
        this.toastr.success(`The group ${groupName} was created.`, 'Success');
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }
}
