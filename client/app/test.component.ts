import { Component, OnInit } from '@angular/core';
import { LogDayService } from "./services/logDayService";
import { LogTypeService } from "./services/logTypeService";
import { LogEntry } from './model/LogEntry';
import { DataType } from './model/DataType';
import { LogType } from './model/LogType';

@Component({
    selector: 'test-component',
    templateUrl: 'test.component.html'
})
export class TestComponent implements OnInit {
    testText: string = "this is the initial text";
    currentPage: string = 'logging';
    logTypes: LogType[];

    data: LogEntry[];

    constructor(private logTypeService: LogTypeService, private logDayService: LogDayService) {}

    get headers(): string[] {
        return this.data.map(x => x.type.name)
    }

    // get data(): LogEntry[] {
    //     return [
    //         { id: 1, type: this.logTypes[0], value: 'nah' },
    //         { id: 2, type: this.logTypes[1], value: 'good' },
    //     ];
    // }

    ngOnInit(){        
        this.logTypeService.getAll().subscribe(res => {
            this.logTypes = res;
        });

        this.logDayService.getAll().subscribe(res => {
            // this.data = res.map(day => day.entries)
        });

            // this.logTypes = [
            //     { id: 1, name: 'Milk', dataType: DataType.Text },
            //     { id: 1, name: 'Sleep', dataType: DataType.Text }]
    }
}