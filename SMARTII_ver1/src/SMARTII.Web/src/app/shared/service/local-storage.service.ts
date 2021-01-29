import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }


  get<T>(key: string) {
    let value = window.localStorage.getItem(key);

    return <T>JSON.parse(value);
  }

  set<T>(key: string, data: T) {

    window.localStorage.setItem(key, JSON.stringify(data));

  }

  remove(key: string) {
    window.localStorage.removeItem(key);
  }

  clear() {
    window.localStorage.clear();
  }
}
