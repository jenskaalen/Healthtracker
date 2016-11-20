import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';

@Injectable()
export class BaseService<T> {
    public baseApi = 'http://localhost:5000/api/'

    constructor(protected entityName: string, protected http: Http) { }

    public getById(id): Observable<T> {
        return this.http.get(`${this.baseApi}${this.entityName}?id=${id}`)
        .map(res => res.json());
    }

    public getAll(): Observable<T[]> {
        return this.http.get(`${this.baseApi}${this.entityName}`)
        .map(res => res.json());
    }

    public delete(id: number): Observable<Response> {
        return this.http.delete(`${this.baseApi}${this.entityName}?id=${id}`)
        .map(res => res.json());
}

    public update(entity:T): Observable<T> {
        return this.http.put(`${this.baseApi}${this.entityName}`, entity)
        .map(res => res.json());
    }

    public add(entity:T): Observable<T> {
        return this.http.post(`${this.baseApi}${this.entityName}`, entity)
        .map(res => res.json());
    }
}

// import { Injectable } from '@angular/core';

// export interface BaseService<T> {
//     add(entity:T): T;
//     update(entity:T): T;
//     delete(id: number);
//     getAll():T[];
//     getById(id):T;
// }