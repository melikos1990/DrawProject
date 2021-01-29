import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { takeUntil, filter } from 'rxjs/operators';
import { ActivatedRoute, Params } from '@angular/router';
import { State as fromReducers } from "../../store/reducers"
import { Store } from '@ngrx/store';
import { QuestionCategoryDetail } from 'src/app/model/question-category.model';
import * as fromQuestionCategoryActions from "../../store/actions/question-category.actions";
import { DraggableListComponent } from 'src/app/shared/component/other/draggable-list/draggable-list.component';
import * as fromRootActions from 'src/app/store/actions';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';

@Component({
  selector: 'app-question-order',
  templateUrl: './question-order.component.html',
  styleUrls: ['./question-order.component.scss']
})
export class QuestionOrderComponent extends FormBaseComponent implements OnInit {

  @ViewChild(DraggableListComponent) dndList: DraggableListComponent;

  dndAjaxOpt: any;

  sortCompare = (accumulator, currentValue) => accumulator.extend.Order - currentValue.extend.Order;

  parentID: number;
  buID: number;
  buName: string;
  detail: QuestionCategoryDetail = new QuestionCategoryDetail();

  constructor(
    public injector: Injector,
    private active: ActivatedRoute,
    private store: Store<fromReducers>
  ) {
    super(injector);
  }

  ngOnInit() {
    this.listnerStore();
  }

  ngOnDestroy() {
    this.store.dispatch(new fromQuestionCategoryActions.Clear());
    super.ngOnDestroy();
  }

  listnerStore() {
    this.active.params.pipe(takeUntil(this.destroy$)).subscribe(this.loadPage.bind(this));

    this.store.select(x => x.master.questionCategory.detail)
      .subscribe(detail => {
        this.detail = Object.assign(new QuestionCategoryDetail(), detail);
      })
  }

  loadPage(params: Params) {
    this.parentID = params["parentID"];
    this.buID = params["buID"];
    this.buName = params["buName"]

    this.dndAjaxOpt = {
      url: "Common/Master/GetQuestionClassificationList",
      method: "Post",
      body: {
        BuID: this.buID,
        parentID: this.parentID,
        Level: !!(this.parentID) ? null : 1 //如果沒有選擇問題分類 預設為第一層, 有選擇則帶入該問題份類底下
      }
    };


    // 如果 未選擇 問題分類就不取得title資訊
    if (!this.parentID) return;

    this.store.dispatch(new fromQuestionCategoryActions.GetDetail({
      nodeID: this.buID,
      id: this.parentID,
      organizationType: this.organizationType.HeaderQuarter
    }))


  }

  btnBack() { history.back(); }

  btnEditOrder() {

    let data = this.dndList.completeData;

    if (!data || data.length == 0) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("無資料可排序")));
      return;
    }

    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否儲存?',
      () => {
        this.store.dispatch(new fromQuestionCategoryActions.EditOrder(data))
      }
    )));
  }


}
