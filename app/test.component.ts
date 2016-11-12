import { Component } from '@angular/core';

@Component({
    selector: 'test-component',
    templateUrl: 'test.component.html'
})
export class TestComponent {
    testText: string = "this is the initial text";
}