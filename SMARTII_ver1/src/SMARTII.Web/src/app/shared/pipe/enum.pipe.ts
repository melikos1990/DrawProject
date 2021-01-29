import { Pipe, PipeTransform } from '@angular/core';
import * as enumText from 'src/app/shared/data/enum-text'

@Pipe({
  name: 'enum'
})
export class EnumPipe implements PipeTransform {

  transform(value: string, args?: string): any {

    
    if (value === undefined || value === null) {
      return '';
    }

    const payload = enumText[args];

    if (!payload) return value;
    
    return payload[value];


  }

}
