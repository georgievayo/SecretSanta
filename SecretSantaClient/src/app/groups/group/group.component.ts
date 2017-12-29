import { ToastrService } from 'ngx-toastr';
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
  isOwner = false;
  receiver;
  currentUserUsername;

  constructor(private groupsService: GroupsService,
    private toastr: ToastrService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.currentUserUsername = JSON.parse(localStorage.getItem('currentUser')).userName;
    this.route.params
      .map(params => params['name'])
      .do(name => this.groupName = name)
      .flatMap(name => {
        return this.groupsService.getGroup(name);
      })
      .subscribe(res => {
        this.group = res;
        if (this.group.Participants) {
          this.isOwner = true;
        }
        this.getReceiver();
      });
  }

  start() {
    this.groupsService.startProcess(this.groupName)
      .subscribe(res => {
        this.toastr.success('The process was started!', 'Success');
        this.getReceiver();
      },
      error => this.toastr.error(error._body, 'Error'));
  }

  getReceiver() {
    this.groupsService.getConnectedPerson(this.currentUserUsername, this.groupName)
      .subscribe(secretSanta => {
        this.receiver = secretSanta.json().Receiver;
      },
      error => this.toastr.warning('The process was not started!', 'Warning'));
  }

  remove(username) {
    this.groupsService.removeParticipant(username, this.groupName)
      .subscribe(res => {
        this.toastr.success(`${username} was removed from group.`, 'Success');
      },
      error => {
        this.toastr.error(error.statusText, 'Error');
      });
  }
}
