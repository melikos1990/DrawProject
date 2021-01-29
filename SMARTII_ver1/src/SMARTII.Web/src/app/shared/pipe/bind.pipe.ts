import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bind'
})
export class BindPipe implements PipeTransform {

  transform(value: any, args?: {this: any, trigger: any}): any {

    if (!value || typeof value != "function") {
      console.warn("BindPipe 接收參數為 null 或 不是 Function");
      return null;
    }

    const target = value;

    return this.reBind(target, args);

  }

  private reBind = (cb, val)  => cb.bind(val.this);

}
