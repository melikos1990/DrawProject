import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'replace'
})
export class ReplacePipe implements PipeTransform {

    private defaultOptions: { replaceStr: string, insteadStr: string, isRegExp: boolean } = {
        replaceStr: null,
        insteadStr: '',
        isRegExp: false
    }

    transform(value: string, args: { replaceStr: string, insteadStr: string, isRegExp: boolean } = this.defaultOptions): any {

        if (value === undefined || value === null) {
            return '';
        }

        if(!args.replaceStr) return value;

        let replaceStr = args.isRegExp ? new RegExp(args.replaceStr, 'g') : args.replaceStr;
        
        
        return value.replace(replaceStr, args.insteadStr);

    }

}
