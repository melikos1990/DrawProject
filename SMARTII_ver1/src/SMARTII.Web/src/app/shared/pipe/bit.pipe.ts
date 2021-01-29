import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bit'
})
export class BitPipe implements PipeTransform {


  private defaultOptions = { true: 'true', false: 'false' };

  transform(value: string | boolean, args?: { true: string, false: string }): any {
    
    args = args || this.defaultOptions;

    if (value === undefined || value === null) {
      return '';
    }

    const target = value.toString().toLowerCase();

    if (target === 'true') return args.true;
    if (target === 'false') return args.false;

    return '';

  }

}
