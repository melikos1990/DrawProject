import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';



@Pipe({
  name: 'datetime'
})
export class DatetimePipe implements PipeTransform {

  private defaultOptions = 'YYYY/MM/DD HH:mm:ss';

  transform(value: any, args?: any): any {

    args = args || this.defaultOptions;

    if (value === undefined || value === null) {
      return '';
    }

    const datetime = moment(value).format(args);

    return datetime || '';
  }

}
