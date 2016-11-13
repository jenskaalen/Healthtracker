import { NgModule,  ComponentFactory } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { TestComponent } from './test.component';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [ BrowserModule, FormsModule, CommonModule ],
    declarations: [ TestComponent ],
    bootstrap:    [ TestComponent ]
})
export class AppModule { } 