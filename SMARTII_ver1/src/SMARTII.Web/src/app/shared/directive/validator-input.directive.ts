import { Directive, ViewContainerRef, OnInit, ComponentFactoryResolver, Renderer2, ElementRef, Injector } from '@angular/core';
import { ValidatorInputComponent } from '../component/validator/validator-input/validator-input.component';
import { FormControlName } from '@angular/forms';

@Directive({
  selector: '[validator]'
})
export class ValidatorInputDirective implements OnInit {


  constructor(
    private renderer: Renderer2,
    private element: ElementRef,
    private resolver: ComponentFactoryResolver,
    private viewContainerRef: ViewContainerRef,
    private formControlName: FormControlName) {


  }
  ngOnInit(): void {


    // 避免dynamic form 透過動態render 時 , 並未組入到 div 的 style
    setTimeout(() => {

      const parent = this.element.nativeElement.parentNode;
      const divElement = this.renderer.createElement('div');
      const classes = (<HTMLElement>this.element.nativeElement).classList;

      classes.forEach(className => this.transformStyles(className, divElement));
      this.transformDOM(parent, divElement);
      this.createComponent();

    }, 0);

  }

  transformStyles(className, div) {
    const name = className.toLowerCase();
    if (name.indexOf('col-') !== -1 ||
      name.indexOf('offset-') !== -1) {
      this.renderer.setStyle(div , 'padding' , 0);
      this.renderer.addClass(div, className);
      this.renderer.removeClass(this.element.nativeElement, className);
    }
  }

  transformDOM(parent, div) {
    this.renderer.insertBefore(parent, div, this.element.nativeElement);
    this.renderer.removeChild(parent, this.element.nativeElement);
    this.renderer.appendChild(div, this.element.nativeElement);
  }


  createComponent() {
    const factory = this.resolver.resolveComponentFactory(ValidatorInputComponent);
    const componentRef = this.viewContainerRef.createComponent(factory);
    componentRef.instance.controlName = this.formControlName;
  }

}
