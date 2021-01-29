import * as fromRootAction from '../../store/actions'
import { of, Observable, concat } from "rxjs";

export const _route$ = (navigate : string , params? : any) => of(new fromRootAction.RouteActions.changeRouteAction({
    url : navigate ,
    params : params
}));
