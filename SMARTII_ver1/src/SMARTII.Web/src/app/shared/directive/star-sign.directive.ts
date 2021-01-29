import { Directive, ElementRef } from '@angular/core';
@Directive({
    selector: '[starsign]'
})
export class StarsignLableDirective {
    constructor(private el: ElementRef) {
        this.setup();
    }
    setup() {
        const parent = this.el.nativeElement.innerHTML;
        var starSingText = "* ";
        var starSingTextRed = starSingText.fontcolor("red");
        this.el.nativeElement.innerHTML = starSingTextRed + parent;
    }
}