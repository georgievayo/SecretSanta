import { ToastrService } from 'ngx-toastr';
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
  take = 8;
  skip;
  constructor(private groupsService: GroupsService,
    private toastr: ToastrService) {
  }

  ngOnInit() {
    this.skip = 0;
    this.getGroups();
  }

  getGroups() {
    this.groupsService.getUserGroups(this.currentUserUsername, this.skip, this.take)
      .subscribe(res => {
        this.groups = res;
      },
      error => this.toastr.error('Cannot get groups.', 'Error'));
  }

  previous() {
    this.skip -= this.take;
    if (this.skip < 0) {
      this.toastr.warning('This is the beggining of list.', 'Warning');
      this.skip = 0;
    }

    this.groupsService.getUserGroups(this.currentUserUsername, this.skip, this.take)
      .subscribe(groups => {
        this.groups = groups;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

  next() {
    this.skip += this.take;
    this.groupsService.getUserGroups(this.currentUserUsername, this.skip, this.take)
      .subscribe(groups => {
        if (groups.length <= 0) {
          this.toastr.warning('There is no more groups to show.', 'Warning');
          this.previous();
        }
        this.groups = groups;
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }

}
