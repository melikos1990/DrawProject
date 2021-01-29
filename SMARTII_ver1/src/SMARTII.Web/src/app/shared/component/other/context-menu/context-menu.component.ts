import { Component, OnInit, Injector, OnDestroy } from '@angular/core';
import { LayoutBaseComponent } from 'src/app/pages/base/layout-base.component';
import { State as fromRootReducers } from 'src/app/store/reducers';
import { Store } from '@ngrx/store';
import { Subscription, pipe } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-context-menu',
  templateUrl: './context-menu.component.html',
  styleUrls: ['./context-menu.component.scss']
})
export class ContextMenuComponent extends LayoutBaseComponent implements OnInit, OnDestroy {
  typesOfShoes: string[] = ['Boots', 'Clogs', 'Loafers', 'Moccasins', 'Sneakers'];
  model$: Subscription;

  display: boolean;
  cbDist: [{ [T: string]: (obj?) => void }] = [{}];
  position: {
    x: number,
    y: number
  };
  title: string;

  constructor(
    public store: Store<fromRootReducers>,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.subscription();
  }


  subscription() {

    const debouncedInput = this.store.select(x => x.app.contextMenu).pipe(debounceTime(500));

    this.model$ = debouncedInput
      .subscribe((menu: any) => {
        this.display = menu.display;
        this.cbDist = menu.cbDist;
        this.position = menu.position;
        this.title = menu.title;
        this.isResetSettings();
      });

  }

  isResetSettings() {
    if (this.display === false) {
      this.cbDist = [{}];
      this.position = {
        x: 0,
        y: 0
      }
    }
  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
  }


}
