import { Observable, iif } from "rxjs";
import { flatMap, catchError, tap } from 'rxjs/operators';
import { AspnetJsonResultBase } from 'src/app/model/common.model';


export const _isHttpSuccess$ = (result : AspnetJsonResultBase) => result.isSuccess ;

export const _httpflow$ = (
    onSuccess$: (result: any) => Observable<any>,
    onFailed$: (result: any) => Observable<any>,
    behavior$: Observable<any>,
    considier: (result: any) => boolean) => {

    return behavior$.pipe(
        tap(res => console.log("effect API 回傳結果物件 ======> ",res)),
        flatMap(result => {
            return iif(() => considier(result), onSuccess$(result), onFailed$(result))
        }),
        catchError(error => onFailed$(error))
    )

}
