import { Component, Injector, Optional, Inject, ViewChild } from '@angular/core';
import { FormBaseComponent } from '../../base/form-base.component';
import { PREFIX_TOKEN } from 'src/app/shared/injection-token';
import { HashtagComponent } from 'src/app/shared/component/other/hashtag/hashtag.component';
import { CaseWarningSelectComponent } from 'src/app/shared/component/select/element/case-warning-select/case-warning-select.component';
import { BuSelectComponent } from 'src/app/shared/component/select/element/bu-select/bu-select.component';
import { DynamicFinishReasonComponent } from 'src/app/shared/component/other/dynamic-finish-reason/dynamic-finish-reason.component';
import { BuNodeDefinitionLevelSelectorComponent } from 'src/app/shared/component/select/component/bu-relation-select/bu-nodedef-level-select/bu-nodedef-level-select.component';
import { CaseCallCenterSearchViewModel } from 'src/app/model/search.model';



@Component({
    selector: 'app-search-base',
    template: ''
})
export class SearchBaseComponent extends FormBaseComponent {

    @ViewChild('allNodeSelector') allNodeSelector: BuNodeDefinitionLevelSelectorComponent;

    constructor(
        public injector: Injector,
        @Optional() @Inject(PREFIX_TOKEN) public prefix?: string
    ) {
        super(injector, prefix);
    }


    removeConcatAndComplained<T extends object>(prefix: string, model: T) {
        for (let key in model) {
            if (key.startsWith(prefix) && !key.endsWith("UnitType")) {
                model[key] = null;
            }
        }
    }

    

    getTags(tagRef: HashtagComponent) {
        let tags = tagRef ? (tagRef.tags != null && tagRef.tags.length > 0 ? tagRef.tags : null) : null;
        let result: { caseTagText, caseTagID }[] = [];
        
        if (tags) {

            result = tags.map(data => ({
                caseTagText: data.text,
                caseTagID: data.id
            }))

        }

        return result;
    }


    getWarning(warningSelect: CaseWarningSelectComponent, filter: (data: { id, text }) => boolean) {
        let caseWarning = null;
        if (warningSelect && warningSelect.items && warningSelect.items.length > 0) {
            let data = warningSelect.items.find(filter);
            caseWarning = data ? data : null;
        }

        return caseWarning;
    }


    getBu(buSelect: BuSelectComponent, filter: (data: { id, text }) => boolean) {
        let node = null;
        if (buSelect.items && buSelect.items.length > 0) {
            let data = buSelect.items.find(filter);
            node = data ? data : null;
        }
        return node;
    }


    getReason(finishReason: DynamicFinishReasonComponent) {
        let result = null;

        if (finishReason) {
            let datas = finishReason.selected;
            
            if (!datas || Object.keys(datas).length <= 0) return;


            let reasons = [];
            for (let key in datas) {

                let reasonClassification = finishReason.finishReasonClassification.find(x => x.ClassificationID == parseInt(key));

                let reasonDatas = datas[key];
                let items = reasonClassification.FinishReasons.filter(x => reasonDatas.some(g => g == x.ID));

                reasons = reasons.concat(items);
            }

            result = reasons;
        }

        return result;
    }


    protected btnReport($event){};

    protected btnRender($event){};

}