import { SelectDataItem } from 'ptc-select2';
import { TemplateRef } from '@angular/core';

export enum ActionType {
  Read = 0,
  Add = 1,
  Update = 2,
  Delete = 3,
  Admin = 4,
}

export class AspnetJsonResultBase {
  isSuccess: boolean;
  message: string;
  extend: any;
}

export class AspnetJsonResult<T> extends AspnetJsonResultBase {

  constructor() {
    super();
  }
  element: T;
}

export class InvokerPayload {
  cb: (obj?: any) => void;
}

export class InvokerPairPayload {
  failed: (obj?: any) => void;
  success: (obj?: any) => void;
}

export class EntrancePayload<T> extends InvokerPairPayload {
  constructor(data?: T) {
    super();
    this.data = data;
  }
  data: T;
  public dataExport?: TemplateRef<any>;
  success: (obj?: any) => void;
  failed: (obj?: any) => void;

}

export class ResultPayload<T> extends InvokerPayload {
  constructor(
    public data: T,
    public cb: (obj?: any) => void,
    public dataExport?: TemplateRef<any>,
    public msg?: string
  ) {
    super();
  }
}


export class Select2Respones<T extends SelectDataItem> {
  items: T[];
}
