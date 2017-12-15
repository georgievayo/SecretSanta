import { Http, RequestOptions, Headers } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class RequestsService {

constructor(private http: Http) { }

getUserRequests(username, skip, take, order) {
    const options = this.getHeaders();
    return this.http.get(`http://localhost:2835/api/users/${username}/requests?skip=${skip}&take=${take}&order=${order}`, options)
        .map(res => res.json());
}

deleteRequest(username, requestId) {
    const options = this.getHeaders();
    return this.http.delete(`http://localhost:2835/api/users/${username}/requests/${requestId}`, options)
        .map(res => res.json());
}

acceptRequest(username, groupName) {
    const options = this.getHeaders();
    return this.http.post(`http://localhost:2835/api/groups/${groupName}/participants`, JSON.stringify(username), options)
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
