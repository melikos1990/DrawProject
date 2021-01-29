import * as fromRootAction from '../../store/actions'
import { of, Observable, concat } from "rxjs";

export const _loading$ = of(new fromRootAction.LoadingActions.visibleLoadingAction());
export const _unLoading$ = of(new fromRootAction.LoadingActions.invisibleLoadingAction());

export const _loadingWork$ = (behavior$: Observable<any>) => {
    return concat(_loading$, behavior$, _unLoading$);
}