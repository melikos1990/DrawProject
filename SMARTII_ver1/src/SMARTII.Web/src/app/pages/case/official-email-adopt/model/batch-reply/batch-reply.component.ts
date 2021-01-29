import { Component, OnInit, Input, Injector, ViewChild } from '@angular/core';
import { OfficialEmailReplyRengeViewModel } from 'src/app/model/case.model';
import { CaseTemplateSelectorComponent } from 'src/app/shared/component/modal/case-template-selector/case-template-selector.component';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FormBaseComponent } from 'src/app/pages/base/form-base.component';
import { DynamicQuestionSelectComponent } from 'src/app/shared/component/select/component/dynamic-question-select/dynamic-question-select.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State as fromCaseReducer } from '../../../store/reducers';
import * as fromRootActions from 'src/app/store/actions';
import * as fromOfficialEmailAdoptActions from 'src/app/pages/case/store/actions/official-email-adopt.actions';
import { EntrancePayload } from 'src/app/model/common.model';
import { CaseTemplateParseViewModel } from 'src/app/model/master.model';
import { caseTemplateKey } from 'src/global';
import { CaseService } from 'src/app/shared/service/case.service';

@Component({
  selector: 'app-batch-reply',
  templateUrl: './batch-reply.component.html',
  styleUrls: ['./batch-reply.component.scss']
})
export class BatchReplyComponent extends FormBaseComponent implements OnInit {

  @Input() buID: number;

  @ViewChild("dynamicSelect") dynamicSelect: DynamicQuestionSelectComponent;

  @Input() model: OfficialEmailReplyRengeViewModel = {} as OfficialEmailReplyRengeViewModel;
  form: FormGroup;

  selectedTemplate: string = "";
  onRefrach: any;

  mailoperatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  finishoperatorInfo: {
    input: any,
    startPos: number,
    endPos: number
  };

  constructor(
    public injector: Injector,
    public modalService: NgbModal,
    public store: Store<fromCaseReducer>,
    private caseService: CaseService
  ) {
    super(injector);
  }


  ngOnInit() {
    this.initializeForm();
  }

  btnConfirm() {

    let lastValue = this.dynamicSelect.lastHasValue;
    this.form.controls["QuestionID"].patchValue(lastValue);
    this.model.QuestionID = lastValue;

    if (this.model.QuestionID == null) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage("至少選擇一種問題分類")));
      return;
    }

    if (this.validForm(this.form) == false) {
      this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage()));
      return;
    }



    this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getLoopQuestionMessage('是否批量回覆?',
      () => {
        let payload = new EntrancePayload(this.model);

        payload.success = this.ajaxSuccess.bind(this);

        this.store.dispatch(new fromOfficialEmailAdoptActions.replyEmail(payload));
      }
    )));


  }

  btnClose() {
    this.modalService.dismissAll();
  }

  openTemplate(templateName: string) {
    const ref = this.modalService.open(CaseTemplateSelectorComponent, { size: 'lg', container: 'nb-layout', backdrop: 'static' });
    const instance = (<CaseTemplateSelectorComponent>ref.componentInstance);
    const key = templateName == 'EmailContent' ? 'EMAIL' : 'CASE_FINISH';
    instance.model = { BuID: this.buID, ClassificKey: key, Content: null };
    instance.btnAdd = this.btnAddCaseTemplate.bind(this, ref);
    this.selectedTemplate = templateName;
  }

  btnDefaultCaseTemplateModal() {
    const data = new CaseTemplateParseViewModel();
    data.NodeID = this.buID;
    data.IsDefault = true;
    data.ClassificKey = caseTemplateKey.EMAIL;
    this.caseService
      .getTemplate(data)
      .subscribe(x => {
        if (x.isSuccess) {
          this.model.EmailContent = (this.model.EmailContent) ? !!(this.mailoperatorInfo) ? this.insertTag(x.element.Content, this.mailoperatorInfo) : `${this.model.EmailContent}${String.prototype.newLine}${x.element.Content}` : x.element.Content;
        } else {
          this.store.dispatch(new fromRootActions.AlertActions.alertOpenAction(this.getFieldInvalidMessage(x.message)));
        }
      })

  }

  btnAddCaseTemplate(modal: NgbModalRef, data) {

    let tmpdata = this.selectedTemplate == 'EmailContent' ? this.mailoperatorInfo : this.finishoperatorInfo;
    this.model[this.selectedTemplate] = !this.model[this.selectedTemplate] ? data.Content : !!(tmpdata) ? this.insertTag(data.Content, tmpdata) : this.model[this.selectedTemplate] += data.Content;

    modal.dismiss();
  }


  ajaxSuccess() {
    this.btnClose();
    this.onRefrach && this.onRefrach();
  }


  initializeForm() {

    this.form = new FormGroup({
      QuestionID: new FormControl(null, [
        Validators.required
      ]),
      EmailContent: new FormControl(null, [
        Validators.required
      ]),
      FinishContent: new FormControl(null, [
        Validators.required
      ]),
      MessageIDs: new FormControl(this.model.MessageIDs, [
        Validators.required
      ])
    })
  }

  mailclickInput = (el) => {
    this.mailoperatorInfo = this.resetInputInfo(el);
  }

  finishclickInput = (el) => {
    this.finishoperatorInfo = this.resetInputInfo(el);
  }

  insertTag(tag: string, el: any) {
    if (!el) return;

    let { input, startPos } = el;

    let oldValue: string = input.value;

    let newValue = oldValue.slice(0, startPos) + tag + oldValue.slice(startPos);

    return newValue;

  }

  private resetInputInfo = (el) => ({
    input: el,
    startPos: el.selectionStart,
    endPos: el.selectionEnd
  })

}
