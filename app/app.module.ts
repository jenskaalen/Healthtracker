import { NgModule, ComponentFactory } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TestComponent } from './test.component';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [ BrowserModule, FormsModule ],
    declarations: [ TestComponent ],
    bootstrap:    [ TestComponent ]
})
export class AppModule { }