import { Http, Headers, RequestOptions } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class GroupsService {

constructor(private http: Http) { }


createGroup(groupName) {
    const options = this.getHeaders();
    const group = {
        name: groupName
    };
    return this.http.post(`http://localhost:2835/api/groups`, group, options)
        .map(res => res.json());
}

getUserGroups(username, skip, take) {
    const options = this.getHeaders();
    return this.http.get(`http://localhost:2835/api/users/${username}/groups?skip=${skip}&take=${take}`, options)
        .map(res => res.json());
}

getGroup(groupName) {
    const options = this.getHeaders();
    return this.http.get(`http://localhost:2835/api/groups/${groupName}`, options)
        .map(res => res.json());
}

startProcess(groupName) {
    const options = this.getHeaders();
    return this.http.put(`http://localhost:2835/api/groups/${groupName}/connections`, null, options);
}

getConnectedPerson(username, groupName) {
    const options = this.getHeaders();
    return this.http.get(`http://localhost:2835/api/users/${username}/groups/${groupName}/connections`, options);
}

removeParticipant(username, groupName) {
    const options = this.getHeaders();
    return this.http.delete(`http://localhost:2835/api/groups/${groupName}/participants/${username}`, options);
}

private getHeaders() {
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');

    const currentUser = JSON.parse(localStorage.getItem('currentUser'));

    if (currentUser && currentUser.access_token) {
        headers.append('Authorization', 'Bearer ' + currentUser.access_token);
    }

    return new RequestOptions({ headers: headers });
    }
}
