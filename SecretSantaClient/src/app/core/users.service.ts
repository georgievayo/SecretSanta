import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class UsersService {
    constructor(private http: Http) { }

    signup(user) {
        this.http.post('http://localhost:2835/api/users', user).subscribe(res => console.log(res));
    }

    login(username: string, password: string) {
        // should be passHash
        const headers = new Headers({ 'Access-Control-Allow-Origin': '*' });
        const options = new RequestOptions({ headers: headers });
        const body = new URLSearchParams();
        body.set('username', username);
        body.set('password', password);
        body.set('grant_type', 'password');

        return this.http.post('http://localhost:2835/Token', body.toString())
            .subscribe((res) => {
                if (res.ok) {
                    const authToken = res.json();
                    localStorage.setItem('currentUser', JSON.stringify(authToken));
                }
            });
    }

    logout() {
        const user = JSON.parse(localStorage.getItem('currentUser'));
        localStorage.removeItem('currentUser');
    }

    getUsers(skip, take, order) {
        const options = this.getHeaders();
        return this.http.get(`http://localhost:2835/api/users?skip=${skip}&take=${take}&order=${order}`, options)
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
