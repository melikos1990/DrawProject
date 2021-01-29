
import { OrganizationType } from './organization.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectDataItem } from 'ptc-select2';

export class QuestionCategoryDetail {


    ID: number;
    BuID: number;
    ParentNodePath: SelectDataItem[];
    ParentNode: QuestionCategoryDetail;
    ParentIDPath: string;
    ParentNamePath: string;
    Children: QuestionCategoryDetail[];
    Name: string;
    IsEnable: boolean;
    Level: number;
    OrganizationType: OrganizationType;
    Order: number;

    Answers: QuestionClassificationAnswerViewModel[] = [];

    UpdateUserName: string;
    UpdateTime: Date;
    CreateUserName: string;
    CreateTime: Date;

}

export class QuestionClassificationGuideDetail {
    
    ID: number;
    NodeID: number;
    ClassificationID: number;
    ParentNodePath: SelectDataItem[]; 
    Children: QuestionCategoryDetail[];
    Name: string;
    IsEnable: boolean;
    Level: number;
    UpdateUserName: string;
    UpdateDateTime: Date;
    CreateUserName: string;
    CreateDateTime: Date;   
    Title: string;
    Content: string;
}


export class QuestionClassificationAnswerDetail {
    

    ID: number;
    NodeID: number;
    ParentNodePath: SelectDataItem[];
    ParentNode: QuestionCategoryDetail;
    ParentIDPath: string;
    ParentNamePath: string;
    Children: QuestionCategoryDetail[];
    Name: string;
    IsEnable: boolean;
    Level: number;
    Order: number;

    Answers: QuestionClassificationAnswerViewModel[] = [];

    UpdateUserName: string;
    UpdateDateTime: Date;
    CreateUserName: string;
    CreateDateTime: Date;
    
    Title: string;
    Content: string;
}

export class QuestionSelectInfo {
    Level: number;
    Name: string;
    ID: number;

}

export class QuestionClassificationAnswerListViewModel {
    ID: number;
    Title: string;
    Content: string;
}

export class QuestionClassificationAnswerViewModel {

    constructor(DataType: AnswerActionType = AnswerActionType.None) {
        this.DataType = DataType;
    }

    ID: number;
    Title: string;
    Content: string;
    DataType: AnswerActionType = AnswerActionType.None;
    ClassificationID: string;
    NodeID: number;
    UpdateUserName: string;
    UpdateTime: Date;

    static toFormGroup(data: QuestionClassificationAnswerViewModel) {
        return new FormGroup({
            Title: new FormControl(data.Title, [Validators.required]),
            Content: new FormControl(data.Content, [Validators.required]),
            DataType: new FormControl(data.DataType),
            ID: new FormControl(data.ID),
            UpdateUserName: new FormControl(data.UpdateUserName),
            UpdateTime: new FormControl(data.UpdateTime)
        })
    }

}

export class QuestionClassificationGuideViewModel {

    constructor(DataType: AnswerActionType = AnswerActionType.None) {
        this.DataType = DataType;
    }

    ID: number;
    Content: string;
    DataType: AnswerActionType = AnswerActionType.None;
    ClassificationID: string;
    NodeID: number;
    UpdateUserName: string;
    UpdateTime: Date;

    static toFormGroup(data: QuestionClassificationAnswerViewModel) {
        return new FormGroup({
            Title: new FormControl(data.Title, [Validators.required]),
            Content: new FormControl(data.Content, [Validators.required]),
            DataType: new FormControl(data.DataType),
            ID: new FormControl(data.ID),
            UpdateUserName: new FormControl(data.UpdateUserName),
            UpdateTime: new FormControl(data.UpdateTime)
        })
    }

}

export class QuestionClassificationAnswerSearchViewModel {
    
    constructor(
        nodeID?: number,
        keyword?: string,
        ID? : number,
    ){
        this.NodeID = !!nodeID ? nodeID : null;
        this.Keyword = !!keyword ? keyword : '';
        this.ID = !!ID ? ID : null;
    }

    Keyword: string;

    NodeID: number;

    ParnetIDPath: string;

    ID: number;

    ClassificationID: number;

}

export class QuestionClassificationGuideSearchViewModel {
    
    constructor(
        nodeID?: number,
        ID? : number,
        content? : string,
    ){
        this.NodeID = !!nodeID ? nodeID : null;
        this.Content = !!content ? content : '';
        this.ID = !!ID ? ID : null;
    }

    Content: string;

    NodeID: number;

    ParnetIDPath: string;

    ID: number;

    ClassificationID: number;

}

export class QuestionClassificationSearchViewModel {

    constructor(
        level?: number,
        buID?: number,
        filterID?: number,
        isEnabled?: boolean
    ) {
        this.BuID = !!buID ? buID : null;
        this.Level = !!level ? level : null;
        this.FilterID = !!filterID ? filterID : null;
        this.IsEnabled = !!isEnabled ? isEnabled : null;
    }

    QestionID: number;

    IsEnable: boolean;

    Level: number;

    BuID: number;

    FilterID: number;

    ParnetIDPath: string;

    IsEnabled?: boolean;

}


export class QuestionClassificationListViewModel {
    ID: string;
    OrganizationType: OrganizationType;
    Node_ID: number;
    IsEnable: boolean;
    Names: string[];
    AnswerNames: string[];
}


export enum AnswerActionType {
    None = -1,
    Update,
    Add,
    Remove
}
