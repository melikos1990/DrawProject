import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import 'src/static';
import { map, tap, subscribeOn } from 'rxjs/operators';
import { subscribeTo } from 'rxjs/internal-compatibility';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private http: HttpClient) {

  }

  get<T>(path: string, params: any = null): Observable<T> {
    return this.http.get<T>(path.toHostApiUrl(), {
      params: params
    });
  }

  post<T>(path: string, params: any = null, body: any, headers: any = null): Observable<T> {
    return this.http.post<T>(path.toHostApiUrl(), body, {
      params: params,
      headers: headers
    });
  }

  download(path: string, method: string, params?, headers?: any) {

    let url = path.toHostApiUrl();
    let action;

    switch (method.toLocaleLowerCase()) {
      case 'get':
        action = this.http.get(url, { params, headers, responseType: "blob", observe: 'response' });
        break;
      case 'post':
        action = this.http.post(url, params, { headers, responseType: "blob", observe: 'response' })
        break;
    }

    return action.pipe(
      tap((res: HttpResponse<Blob>) => {
        const link = document.createElement('a');
        const blob = new Blob([res.body], { type: res.body.type });

        let fileEncode = res.headers.get('Content-disposition').split('filename=')[1].replace(/\"/g, "");
        let fileName = fileEncode.decoding();
        link.setAttribute('href',  window.URL.createObjectURL(blob));
        link.setAttribute("download", fileName);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }));
  }


}
