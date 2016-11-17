import { Injectable } from '@angular/core';
import { LogDay } from "./../model/LogDay";
import { BaseService } from "./baseService";
import { Http } from '@angular/http';

@Injectable()
export class LogDayService extends BaseService<LogDay> {
    
    constructor(protected http: Http) { 
        super('logday', http);
    }
}