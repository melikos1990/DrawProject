import { ReplaySubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { globalLang } from 'src/app.config';

@Injectable({
  providedIn: 'root'
})
export class CultureService {

  language$ = new ReplaySubject<LangChangeEvent>(1);
  translate = this.translateService;


  constructor(private translateService: TranslateService) { }

  setInitState() {
    this.setLang(globalLang);
  }

  setLang(lang: string) {
    this.translateService.onLangChange.subscribe(result => {
      this.language$.next(result);
    });
    this.translateService.use(lang);
  }
}
