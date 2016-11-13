import { Component } from '@angular/core';

@Component({
    selector: 'test-component',
    templateUrl: 'test.component.html'
})
export class TestComponent {
    testText: string = "this is the initial text";
    currentPage: string = 'logging';

    get headers(): string[] {
        return Object.getOwnPropertyNames(this.data);
        // return ['Activity', 'Milk', 'Sleep']
    }

    get data(): any[] {
        return [
            { activity: 'Hardcore', milk: 'Lots', sleep: 'Good' },
            { activity: 'Chill', milk: 'Some', sleep: 'Bad' }
        ];
    }
}