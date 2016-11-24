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
    currentPage: string = 'logging';
    logTypes: LogType[];
    logEntries: LogEntry[];
    registered: Date;
    loggedDays: LogDay[];
    isLoading: boolean;

    data: LogEntry[];

    constructor(private logTypeService: LogTypeService, private logDayService: LogDayService) { }

    get headers(): string[] {
        return this.data.map(x => x.logType.name)
    }

    ngOnInit() {
        this.registered = new Date();

        this.logTypeService.getAll().subscribe(res => {
            this.logTypes = res;

            this.logEntries = res.map((type: LogType) => {
                return { id: 0, logType: type, numberValue: 0 }
            });
        });

        this.logDayService.getAll().subscribe(res => {
            this.loggedDays = res.map(day => day)
        });
    }

    ngAfterViewInit() {
        this.registered = new Date();
    }

    createLogDay() {
        console.log('wry');

        let day: LogDay = {
            id: 0,
            logEntries: this.logEntries,
            registered: this.registered
        };

        this.logDayService.add(day).do(() => {
            this.isLoading = true;
        }).debounceTime(1500).subscribe(res => {
                this.isLoading = false;
            });;
    }
}