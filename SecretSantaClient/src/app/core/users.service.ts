import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class UsersService {
    constructor(private http: Http, private router: Router) { }

    signup(user) {
        return this.http.post('http://localhost:2835/api/users', user);
    }

    login(username: string, password: string) {
        const user = {
            username: username,
            password: password
        };

        return this.http.post('http://localhost:2835/api/login', user);
    }

    logout() {
        const options = this.getHeaders();
        return this.http.delete(`http://localhost:2835/api/login`, options);
    }

    getUsers(skip, take, order, pattern) {
        const options = this.getHeaders();
        return this.http.get(`http://localhost:2835/api/users?skip=${skip}&take=${take}&order=${order}&search=${pattern}`, options)
            .map(res => res.json());
    }

    getUserProfile(username) {
        const options = this.getHeaders();
        return this.http.get(`http://localhost:2835/api/users/${username}`, options)
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
