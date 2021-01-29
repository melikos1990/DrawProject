import { Component, OnInit, Injector } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { KMDetailViewModel, KMClassificationNodeViewModel } from 'src/app/model/master.model';
import { takeUntil, withLatestFrom } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { State as fromMasterReducer, kmDetailSelector } from '../../store/reducers';
import * as fromKMActions from '../../store/actions/km.actions';
import { FormGroup, FormControl, Validators, MaxLengthValidator } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';

const PREFIX = 'KmComponent';

@Component({
  selector: 'app-km-detail',
  templateUrl: './km-detail.component.html',
  styleUrls: ['./km-detail.component.scss']
})
export class KmDetailComponent extends FormBaseComponent implements OnInit {
  public options = {};
  public form: FormGroup;

  public uiActionType: ActionType;

  model: KMDetailViewModel = new KMDetailViewModel();
  titleTypeString: string = "";
  constructor(
    private store: Store<fromMasterReducer>,
    private active: ActivatedRoute,
    public injector: Injector) {
    super(injector, PREFIX)
  }

  ngOnInit() {
    this.subscription();
    this.initializeForm();
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {
        this.store.dispatch(new fromKMActions.addAction(this.model));
      }
    )));

  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Update)
  btnEdit($event) {

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromKMActions.editAction(this.model));
      }
    )));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));


    this.store
      .select(kmDetailSelector(this.uiActionType))
      .pipe(
        takeUntil(this.destroy$))
      .subscribe(detail => {

        // 由於分類在新增時 , 為前畫面所帶入
        // 因此利用clone來把分類代號與名稱做替換
        this.model = {
          ...this.model, ...detail
        };

        // 如果重整畫面 , 將會遺失selectItems , 屆時須從params 取得


        this.setFileOptions();

      });


  }
  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = { ID: params['id'] };

    if (!this.model.ClassificationID) {
      this.model.ClassificationID = parseInt(params['classificationID']);
      this.model.ClassificationName = params['classificationName'];
      this.model.PathName = params['pathName'];
    }

    switch (this.uiActionType) {
      case ActionType.Add:
        this.store.dispatch(new fromKMActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromKMActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromKMActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }

  }

  setFileOptions() {

    const paths = this.model.FilePaths || [];
    const previews = paths.map(path => path.toHostApiUrl())
    const previewConfigs = paths.map(path => {

      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteKMFile`.toHostApiUrl(),
        extra: {
          id: this.model.ID,
          key: path
        }
      }
    });

    this.options = {
      preferIconicPreview: true,
      initialPreview: previews,
      initialPreviewConfig: previewConfigs,
      fileActionSettings: {
        showRemove: this.uiActionType == ActionType.Update,
        showUpload: false,
        showClose: false,
        uploadAsync: false,
      }
    };
  }


  initializeForm() {
    this.form = new FormGroup({
      Title: new FormControl(this.model.Title, [
        Validators.required,
        Validators.maxLength(30),
      ]),
      Content: new FormControl(this.model.Content, [
        Validators.required,
        Validators.maxLength(2048),
      ]),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

}
