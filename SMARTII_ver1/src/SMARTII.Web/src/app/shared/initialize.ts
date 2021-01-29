import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { empty, Subject } from 'rxjs';
import { AuthenticationService } from './service/authentication.service';


export const initializeServiceFactory = (config: InitializeService) => () => config.load();

@Injectable()
export class InitializeService {


  public done$: Subject<boolean> = new Subject();

  constructor(
    private authService: AuthenticationService,
    private http: HttpClient) { }


  public load() {

    this.done$.next(false);

    return new Promise((resolve, reject) => {
      this.http.get("common/system/GetLanguageFiles".toHostApiUrl())
        .pipe(catchError(error => {
          console.log('連線失敗');
          resolve(true);
          this.done$.next(true);
          return empty();
        }))

        .subscribe((data) => {
          for (let key in data) {
            let value = data[key];
            this.authService.setLanguages(key, value);
          }
          this.done$.next(true);
          resolve(true);
        });

    });
  }
}
