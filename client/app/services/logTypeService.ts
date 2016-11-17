import { Injectable } from '@angular/core';
import { LogType } from "./../model/LogType";4
import { Http } from '@angular/http';
import { BaseService } from "./baseService";

@Injectable()
export class LogTypeService extends BaseService<LogType> {
    
    constructor(http: Http) { 
        super('LogType', http);
    }

}