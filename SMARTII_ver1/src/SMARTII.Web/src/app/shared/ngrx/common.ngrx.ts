import { Actions, ofType } from '@ngrx/effects';
import { map, tap } from 'rxjs/operators';


export const _entry$ = <T>(action: Actions, type: string) => {
  return action.pipe(
    ofType(type),
    map((action: any) => <T>action.payload),
    tap(x => console.log(type)))
}
