import { Http, Headers, RequestOptions } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class GroupsService {

constructor(private http: Http) { }


createGroup(groupName) {
    const options = this.getHeaders();
    return this.http.post(`http://localhost:2835/api/groups`, JSON.stringify(groupName), options)
        .map(res => res.json());
}

getUserGroups(username, skip, take) {
    const options = this.getHeaders();
    return this.http.get(`http://localhost:2835/api/users/${username}/groups?skip=${skip}&take=${take}`, options)
        .map(res => res.json());
}

getGroup(groupName) {
    const options = this.getHeaders();
    console.log(groupName);
    return this.http.get(`http://localhost:2835/api/groups/${groupName}`, options)
        .map(res => res.json());
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
