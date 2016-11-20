import { LogDay } from './model/LogDay';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { LogDayService } from "./services/logDayService";
import { LogTypeService } from "./services/logTypeService";
import { LogEntry } from './model/LogEntry';
import { DataType } from './model/DataType';
import { LogType } from './model/LogType';

@Component({
    selector: 'test-component',
    templateUrl: 'test.component.html'
})
export class TestComponent implements OnInit, AfterViewInit {
    testText: string = "this is the initial text";
    currentPage: string = 'logging';
    logTypes: LogType[];
    logEntries: LogEntry[];
    registered: Date;

    data: LogEntry[];

    constructor(private logTypeService: LogTypeService, private logDayService: LogDayService) { }

    get headers(): string[] {
        return this.data.map(x => x.type.name)
    }

    // get data(): LogEntry[] {
    //     return [
    //         { id: 1, type: this.logTypes[0], value: 'nah' },
    //         { id: 2, type: this.logTypes[1], value: 'good' },
    //     ];
    // }

    ngOnInit() {
        this.registered = new Date();

        this.logTypeService.getAll().subscribe(res => {
            this.logTypes = res;

            this.logEntries = res.map((type: LogType) => {
                return { id: 0, type: type, value: null }
            });
        });

        this.logDayService.getAll().subscribe(res => {
            // this.data = res.map(day => day.entries)
        });
    }

    ngAfterViewInit() {
        this.registered = new Date();
    }

    createLogDay() {
        console.log('wry');

        let day: LogDay = {
            id: null,
            logEntries: this.logEntries,
            registered: this.registered
        };

        this.logDayService.add(day).subscribe(res => {
            alert('aa');
        });;
    }
}