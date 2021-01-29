import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActionType } from 'src/app/model/common.model';
import { ItemDetailViewModel } from 'src/app/model/master.model';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State as fromMasterReducer } from '../../store/reducers';
import * as fromItemActions from '../../store/actions/item.actions';
import { ActivatedRoute } from '@angular/router';
import { NgInputBase } from 'ptc-dynamic-form';
import { AuthorizeMethod } from 'src/app/shared/decorator/authorize.decorator';
import { loggerMethod } from 'src/app/shared/decorator/logger.decorator';
import { AuthenticationType } from 'src/app/model/authorize.model';
import * as fromRootActions from 'src/app/store/actions';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { tryGetProviderKey } from 'src/global';
import { takeUntil, skip } from 'rxjs/operators';


export const PREFIX = 'ItemComponent';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.scss']
})
export class ItemDetailComponent extends FormBaseComponent implements OnInit {

  @ViewChild('buSelector') buSelector: BuSelectComponent;

  public inputs: NgInputBase[] = [];
  public form: FormGroup;
  public particular = {};
  public uiActionType: ActionType;
  public model: ItemDetailViewModel = new ItemDetailViewModel();
  public options: {} = {
    preferIconicPreview: true,
    fileActionSettings: {
      showRemove: this.uiActionType == ActionType.Update,
      showUpload: false,
      showClose: false,
      uploadAsync: false,
    }
  };

  model$: Subscription;
  layout$: Subscription;
  layout: string;
  nodeKey: string;
  titleTypeString:string;

  constructor(
    private active: ActivatedRoute,
    private store: Store<fromMasterReducer>,
    public injector: Injector) {
    super(injector, PREFIX);
  }

  ngOnInit() {
    this.initializeForm();
    this.subscription();

  }

  ngAfterViewInit() {
    if (this.uiActionType == 1) {
      setTimeout(() => {
        this.buSelector.onChange(this.model.NodeID);
      }, 1000)
    }
  }

  subscription() {
    this.active.params.subscribe(this.loadPage.bind(this));
    this.model.IsEnabled = true;
    this.model$ =
      this.store
        .select((state: fromMasterReducer) => state.master.item.detail)
        .pipe(
          skip(1),
          takeUntil(this.destroy$))
        .subscribe(item => {
          this.model = { ...item };
          this.particular = item ? { ...item.Particular } : null;
          this.deserialize(this.layout);
          if (this.model && this.model.ImagePath) {
            this.fileOptions(this.model.ImagePath);
          }
        });

    this.layout$ =
      this.store
        .select((state: fromMasterReducer) => state.master.item.itemDetailLayout)
        .pipe(
          takeUntil(this.destroy$))
        .subscribe(layout => {
          this.layout = layout;
          this.deserialize(layout);
        });
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Read | AuthenticationType.Add)
  btnAdd($event) {

    debugger;

    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否新增?',
      () => {

        this.model.Particular = this.particular;
        this.store.dispatch(new fromItemActions.addAction(this.model));
      }
    )));
  }

  @loggerMethod()
  @AuthorizeMethod(PREFIX, AuthenticationType.Delete | AuthenticationType.Add)
  btnEdit($event) {


    if (this.validForm(this.form) === false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }
    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {

        this.model.BUName = tryGetProviderKey(this.model.NodeKey);
        this.model.Particular = this.particular;
        this.store.dispatch(new fromItemActions.editAction(this.model));
      }
    )));

  }

  onBuChange($event) {
    if (!($event)) {
      return;
    }
    this.model.NodeKey = $event.NodeKey;
    this.model.BUName = tryGetProviderKey(this.model.NodeKey);
    this.clearParticularCtrl(); // 移除前一個BU的動態欄位驗證
    this.store.dispatch(new fromItemActions.getItemDetailTemplateAction({ nodeKey: $event.NodeKey }));
  }

  @loggerMethod()
  btnBack($event) {
    history.back();
  }


  fileOptions(paths: string[]) {
    const previews = paths.map(x => x.toHostApiUrl())
    const previewConfigs = paths.map(path => {
      return {
        caption: path.split('fileName=')[1],
        key: path,
        downloadUrl: path.toHostApiUrl(),
        url: `/File/DeleteItemImage`.toHostApiUrl(),
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


  deserialize(jsonLayout) {
    this.inputs = [];
    try {

      if (jsonLayout) {
        const objects = <Array<NgInputBase>>JSON.parse(jsonLayout);
        if (objects) {
          this.inputs = objects.map(data => {
            data["disabled"] = this.uiActionType == this.actionType.Read ? true : null;
            return data;
          });
        } else {
          this.inputs = [];
        }
      } else {
        this.inputs = [];
      }
    } catch (e) {
      console.log(e);
    }
  }

  initializeForm() {
    this.form = new FormGroup({
      NodeID: new FormControl(this.model.NodeID, [
        Validators.required,
      ]),
      Description: new FormControl(this.model.Description, [
        Validators.maxLength(100),
      ]),
      Code: new FormControl(this.model.Code, [
        Validators.maxLength(50),
      ]),
      Name: new FormControl(this.model.Name, [
        Validators.required,
        Validators.maxLength(50),
      ]),
      Picture: new FormControl(),
      IsEnabled: new FormControl(),
      CreateUserName: new FormControl(),
      CreateDateTime: new FormControl(),
      UpdateUserName: new FormControl(),
      UpdateDateTime: new FormControl(),
    });

  }

  loadPage(params) {
    this.uiActionType = parseInt(params['actionType']);

    const payload = {
      ID: params['id']
    };

    switch (this.uiActionType) {
      case ActionType.Add:
        this.model.NodeID = parseInt(params['BuID']);
        this.store.dispatch(new fromItemActions.loadEntryAction());
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.ADD');
        break;
      case ActionType.Update:
        this.store.dispatch(new fromItemActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.EDIT');
        break;
      case ActionType.Read:
        this.store.dispatch(new fromItemActions.loadDetailAction(payload));
        this.titleTypeString = this.translateService.instant('COMMON.ENUM.AUTH_TYPE.READ');
        break;
    }
  }

  ngOnDestroy() {
    this.model$ && this.model$.unsubscribe();
    this.layout$ && this.layout$.unsubscribe();
  }

  clearParticularCtrl(){
    this.inputs.forEach(input => {
      this.form.removeControl(input.name);
    })
  }

}
