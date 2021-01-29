import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import * as fromUserPatameterActions from '../store/actions/user-parameter.actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { PageAuthFavoritePairCollection, PageAuthFavorite, AuthenticationType } from 'src/app/model/authorize.model';
import { FormGroup, FormControl } from '@angular/forms';
import { ActionType, EntrancePayload } from 'src/app/model/common.model';
import { UserParameterlViewModel, Menu } from 'src/app/model/master.model';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';
import { takeUntil, skip, exhaustMap } from 'rxjs/operators';
import { State as fromMasterReducer } from '../store/reducers';
import { MENU_ITEMS } from 'src/app/shared/data/menu';
import { FavoriteFeatureDndComponent } from 'src/app/shared/component/other/favorite-feature-dnd/favorite-feature-dnd.component';
import * as fromRootActions from 'src/app/store/actions';


export const PREFIX = 'ItemComponent';

@Component({
  selector: 'app-user-parameter',
  templateUrl: './user-parameter.component.html',
  styleUrls: ['./user-parameter.component.scss']
})
export class UserParameterComponent extends FormBaseComponent implements OnInit {

  @ViewChild('pairCollection') pairCollection: FavoriteFeatureDndComponent;

  public options = {};
  public model: UserParameterlViewModel = new UserParameterlViewModel();

  form: FormGroup;
  NavigateOfNewbie: boolean;
  favoriteData: PageAuthFavoritePairCollection = new PageAuthFavoritePairCollection();
  userID: string;

  constructor(
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }


  ngOnInit() {
    this.initializeForm();
    this.subscription();
  }

  initializeForm() {
    this.form = new FormGroup({
      Picture: new FormControl(),
      NavigateOfNewbie: new FormControl(!(this.model.NavigateOfNewbie) ? this.model.NavigateOfNewbie = true : this.model.NavigateOfNewbie, null),

    });
  }

  /**
   * 取得所有功能節點
   */
  getAll(): PageAuthFavorite[] {
    return MENU_ITEMS
      .toFlatten<Menu>(x => x.children)
      .filter(node => !!node)
      .map(node => {
        let auth = new PageAuthFavorite();
        auth.Order = 0;
        auth.AuthenticationType = AuthenticationType.All;
        auth.Feature = node.component.split('Component')[0];
        auth.Title = node.title;
        return auth;
      });
  }

  /**
   * 回填取得資訊
   */
  getExist = () => this.model ? [... this.model.FavoriteFeature] : null;
  refillPayload() {
    if (this.model && this.model.ImagePath) {
      this.fileOptions(this.model.ImagePath);
    }
  }

  subscription() {

    this.loadPage();

    /**
     *  訂閱
     */
    this.store
      .select((state: fromMasterReducer) => state.system.userparameter.detail)
      .pipe(
        skip(1),
        takeUntil(this.destroy$))
      .subscribe(item => {
        this.model = { ...item };

        this.favoriteData.Right = this.getExist();

        this.favoriteData.Left = this.getAll();

        this.favoriteData = { ...this.favoriteData };

        this.refillPayload();

      });
  }

  /**
   * 取得UserParameter
   */
  loadPage() {

    this.getCurrentUser()
      .pipe(
        takeUntil(this.destroy$),
        exhaustMap(x => {
          this.userID = x.UserID;
          const payload = new EntrancePayload<{ userID: string }>({
            userID: x.UserID
          });
          return of(this.store.dispatch(new fromUserPatameterActions.getUserParameterDetailAction(payload)))
        })
      ).subscribe();
  }

  /**
   * 設定 FileInput Plugin
   */
  fileOptions(paths: string) {
    let object = {};
    object = {
      caption: paths.split('fileName=')[1],
      key: paths,
      downloadUrl: paths.toHostApiUrl(),
      url: `/File/DeleteUserImage`.toHostApiUrl(),
      extra: {
        id: this.model.UserID,
        key: paths
      }
    };

    const previews = paths.toHostApiUrl() + "&timespen=" + new Date().valueOf();

    this.options = {
      preferIconicPreview: true,
      overwriteInitial: true,
      initialPreview: previews,
      initialPreviewConfig: [object],
      allowedFileExtensions: ["jpg", "jpeg", "png"],
      fileActionSettings: {
        showRemove: true,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
  }

  /**
   * 儲存使用者設定
   */
  btnEdit() {

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.model.FavoriteFeature = this.pairCollection.right;

        let payload: EntrancePayload<UserParameterlViewModel> = new EntrancePayload(this.model);

        payload.success = () => {
          const payload = new EntrancePayload<{ userID: string }>({
            userID: this.userID
          });
          this.store.dispatch(new fromUserPatameterActions.getUserParameterDetailAction(payload))
        };


        this.store.dispatch(new fromUserPatameterActions.updateUserParameterAction(payload))
      }
    )));

  }

  btnBack() {
    if (window.history.length > 1) {
      window.history.back();
    } else {
      window.close();
    }
  }
}
