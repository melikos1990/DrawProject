import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { CaseSourceViewModel, CaseAssignmentOverviewViewModel, CaseAssignmentComplaintNoticeViewModel, CaseAssignmentComplaintInvoiceViewModel, CaseAssignmentViewModel, CaseAssignmentResumeViewModel, CaseAssignmentCommunicateViewModel, CaseComplainedUserViewModel, CaseResumeListViewModel, CaseAssignmentUserViewModel } from 'src/app/model/case.model';
import { OrganizationType, BusinesssUnitParameters, ConcatableUserViewModel, BusinessLayouts, UserListViewModel, CallCenterNodeDetailViewModel } from 'src/app/model/organization.model';
import { UserDetailViewModel, User } from 'src/app/model/authorize.model';
import { exhaustMap } from 'rxjs/operators';
import { fromPromise } from 'rxjs/internal-compatibility';
import { CaseTemplateListViewModel, CaseTemplateParseViewModel, CaseTemplateParseResultViewModel } from 'src/app/model/master.model';





@Injectable({
  providedIn: 'root'
})
export class CaseService {

  //public sorceTempSubject: BehaviorSubject<{ [key: string]: CaseSourceViewModel }> = new BehaviorSubject<{ [key: string]: CaseSourceViewModel }>(null);
  public sorceTempSubject: ReplaySubject<{ [key: string]: CaseSourceViewModel }> = new ReplaySubject<{ [key: string]: CaseSourceViewModel }>();

  public lockUsersSubject: ReplaySubject<{ Key: string, Value: User[] }> = new ReplaySubject<{ Key: string, Value: User[] }>();

  public listenOnSourceFlter(obj: { [key: string]: CaseSourceViewModel }, sourcekey: string): boolean {
    return !!obj && !!obj[sourcekey];
  }

  constructor(private http: HttpClient) { }


  checkCase(caseID: string, buID: number, sourceID: string): Observable<AspnetJsonResult<string>> {
    return this.http.get<AspnetJsonResult<string>>('Case/Case/CheckCase'.toHostApiUrl(), {
      params: { caseID: caseID, buID: this.tryToString(buID), sourceID: sourceID }
    });
  }
  getPreventCaseList(buID: number): Observable<AspnetJsonResult<CaseSourceViewModel[]>> {
    return this.http.get<AspnetJsonResult<CaseSourceViewModel[]>>('Case/Case/GetPreventionCaseList'.toHostApiUrl(), {
      params: { buID: this.tryToString(buID) }
    });
  }
  getNearlyCaseList(buID: number, model: ConcatableUserViewModel): Observable<AspnetJsonResult<CaseSourceViewModel[]>> {
    return this.http.post<AspnetJsonResult<CaseSourceViewModel[]>>('Case/Case/GetNearlyCaseList'.toHostApiUrl(), model, {
      params: { buID: this.tryToString(buID) },
    });
  }
  getOrganizationParentPath(nodeID: number, organizationType: OrganizationType) {
    return this.http.get<string[]>('Common/Organization/GetOrganizationParentPath'.toHostApiUrl(), {
      params: {
        nodeID: this.tryToString(nodeID),
        organizationType: this.tryToString(+organizationType)
      }
    });
  }
  getOwnerUserFromStore(nodeJobID: number) {
    return this.http.get<UserListViewModel>('Common/Organization/GetOwnerUserFromStore'.toHostApiUrl(), {
      params: {
        nodeJobID: this.tryToString(nodeJobID),

      }
    });
  }
  getOwnerUserFromNode(nodeID: number, organizationType: OrganizationType) {
    return this.http.get<UserListViewModel>('Common/Organization/GetOwnerUserFromNode'.toHostApiUrl(), {
      params: {
        nodeID: this.tryToString(nodeID),
        organizationType: this.tryToString(+organizationType)
      }
    });
  }
  getBUParameters(buID: number) {
    return this.http.get<BusinesssUnitParameters>('Common/Organization/GetBUParameters'.toHostApiUrl(), {
      params: {
        buID: this.tryToString(buID),

      }
    });
  }
  getBUParametersByNodeKey(nodeKey: string) {
    return this.http.get<BusinesssUnitParameters>('Common/Organization/GetBUParametersByNodeKey'.toHostApiUrl(), {
      params: {
        nodeKey
      }
    });
  }
  getCaseIDs(sourceID: string) {
    return this.http.get<string[]>('Case/Case/GetCaseIDs'.toHostApiUrl(), {
      params: {
        sourceID: sourceID

      }
    });
  }
  getAssignmentAggregate(caseID: string) {
    return this.http.get<AspnetJsonResult<CaseAssignmentOverviewViewModel[]>>('Case/Case/GetCaseAssignmentAggregate'.toHostApiUrl(), {
      params: {
        caseID: caseID
      }
    });
  }
  getResumeList(caseID: string) {
    return this.http.get<AspnetJsonResult<CaseResumeListViewModel[]>>('Case/Case/GetCaseResumeList'.toHostApiUrl(), {
      params: {
        caseID: caseID
      }
    });
  }
  getPreviewComplaintInvoice(buID: number, caseID: string, invoiceID: string) {
    return this.http.post("Case/Case/GetPreviewComplaintInvoice".toHostApiUrl(), null, {
      responseType: 'blob',
      observe: 'response',
      params: {
        buID: this.tryToString(buID),
        caseID: caseID,
        invoiceID: invoiceID
      }
    })
      .pipe(
        exhaustMap(c => {
          const blob = new Blob([c.body], { type: c.body.type });
          console.log('response ==>', c.statusText);
          console.log('blob ==>', blob);
          return fromPromise((<any>blob).arrayBuffer());
        }),
        exhaustMap((x: any) => {
          console.log('x ==>', x);
          return of(new Uint8Array(x))
        })
      );
  }
  getAssignmentCommunicate(communicateID: number) {
    return this.http.get<AspnetJsonResult<CaseAssignmentCommunicateViewModel>>('Case/Case/GetCaseAssignmentCommunicate'.toHostApiUrl(), {
      params: {
        communicateID: this.tryToString(communicateID)
      }
    });
  }
  getAssignmentNotice(noticeID: number) {
    return this.http.get<AspnetJsonResult<CaseAssignmentComplaintNoticeViewModel>>('Case/Case/GetCaseAssignmentNotice'.toHostApiUrl(), {
      params: {
        noticeID: this.tryToString(noticeID)
      }
    });
  }
  getAssignmentIvoice(identityID: number) {
    return this.http.get<AspnetJsonResult<CaseAssignmentComplaintInvoiceViewModel>>('Case/Case/GetCaseAssignmentInvoice'.toHostApiUrl(), {
      params: {
        identityID: this.tryToString(identityID)
      }
    });
  }
  getAssignment(caseID: string, assignmentID: number) {
    return this.http.get<AspnetJsonResult<CaseAssignmentViewModel>>('Case/Case/GetCaseAssignment'.toHostApiUrl(), {
      params: {
        caseID: caseID,
        assignmentID: this.tryToString(assignmentID)
      }
    });
  }

  getResumes(caseID: string, assignmentID: number) {
    return this.http.get<AspnetJsonResult<CaseAssignmentResumeViewModel[]>>('Case/CaseAssignment/GetResumes'.toHostApiUrl(), {
      params: {
        caseID: caseID,
        assignmentID: this.tryToString(assignmentID)
      }
    });
  }
  getFastFinishedReasons(buID?: number) {
    return this.http.get<AspnetJsonResult<CaseTemplateListViewModel[]>>('Case/Case/GetFastFinishedReasons'.toHostApiUrl(), {
      params: {
        buID: this.tryToString(buID),
      }
    });
  }

  getQuestionClassificationGuides(isSelfSetting: boolean, questionClassificationID?: number) {
    return this.http.get<AspnetJsonResult<CaseTemplateListViewModel[]>>('Case/Case/GetQuestionClassificationGuides'.toHostApiUrl(), {
      params: {
        isSelfSetting: this.tryToString(isSelfSetting),
        questionClassificationID: this.tryToString(questionClassificationID)
      }
    });
  }

  getBULayouts(buID: number, isEnabled: boolean) {
    return this.http.get<BusinessLayouts>('Common/Organization/GetBULayouts'.toHostApiUrl(), {
      params: {
        buID: this.tryToString(buID),
        isEnabled: this.tryToString(isEnabled)
      }
    });
  }

  parseTemplateUseExist(template: string, caseID: string) {

    let body = new HttpParams();
    body = body.set('', template);

    return this.http.post<AspnetJsonResult<string>>('Common/Master/ParseCaseTemplateUseExist'.toHostApiUrl(), body, {
      params: {
        key: caseID
      },
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }

    });
  }

  getCallCenterNode(nodeID: number) {
    return this.http.get<AspnetJsonResult<CallCenterNodeDetailViewModel>>('Common/Organization/GetCallCenterNode'.toHostApiUrl(), {
      params: {
        nodeID: this.tryToString(nodeID),
      }
    });
  }

  parseTemplate(model: CaseTemplateParseViewModel) {
    return this.http.post<AspnetJsonResult<CaseTemplateParseResultViewModel>>('Common/Master/ParseCaseTemplate'.toHostApiUrl(), model);
  }

  getTemplate(model: CaseTemplateParseViewModel) {
    return this.http.post<AspnetJsonResult<CaseTemplateParseResultViewModel>>('Common/Master/GetCaseTemplate'.toHostApiUrl(), model);
  }

  getNodeAllUsers(model: CaseAssignmentUserViewModel[]) {
    return this.http.post<AspnetJsonResult<CaseComplainedUserViewModel[]>>('Common/Organization/GetNodeAllUsers'.toHostApiUrl(), model);
  }


  tryToString(params: any) {
    console.log(params);
    console.log("--------1--------");
    if (params === null || params === undefined) {
      return null;
    }
    return params.toString();
  }



}
