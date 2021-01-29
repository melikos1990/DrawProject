import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionStorageService {

  constructor() { }


  get<T>(key: string) {
    let value = window.sessionStorage.getItem(key);

    return <T>JSON.parse(value);
  }

  set<T>(key: string, data: T) {

    window.sessionStorage.setItem(key, JSON.stringify(data));

  }

  remove(key: string) {
    window.sessionStorage.removeItem(key);
  }

  clear() {
    window.sessionStorage.clear();
  }


}
