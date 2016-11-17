import { NgModule,  ComponentFactory } from '@angular/core';
import { LogTypeService } from "./services/logTypeService";
import { LogDayService } from "./services/logDayService";
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { TestComponent } from './test.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

@NgModule({
    imports: [ BrowserModule, FormsModule, CommonModule, HttpModule ],
    declarations: [ TestComponent ],
    providers: [LogDayService, LogTypeService],
    bootstrap:    [ TestComponent ]
})
export class AppModule { } 