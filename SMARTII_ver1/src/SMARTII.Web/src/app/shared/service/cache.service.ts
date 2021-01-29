import { Injectable } from '@angular/core';
import { PtcAjaxOptions } from 'ptc-server-table';

@Injectable({
  providedIn: 'root'
})
export class CacheService {

  public object: any;
  public objectTable: PtcAjaxOptions;

  getObjectCache<T>(): T {
    return this.object;
  }

  getObjectTableCache() {
    return this.objectTable;
  }
}