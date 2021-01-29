import { Component, OnInit, Injector, Input } from '@angular/core';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';
import { AspnetJsonResult } from 'src/app/model/common.model';
import { User } from 'src/app/model/authorize.model';

@Component({
  selector: 'app-verification-code',
  templateUrl: './verification-code.component.html',
  styleUrls: ['./verification-code.component.scss']
})
export class VerificationCodeComponent extends BaseComponent implements OnInit {


  @Input() model: User;
  
  keyValuePair: { KEY: string, VALUE: string };

  constructor(
    public injector: Injector,
    public http: HttpService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.btnRet();
  }

  btnRet(){


    this.http.get<AspnetJsonResult<{ KEY: string, VALUE: string }>>("Account/VerificationCode")
            .subscribe(res => {
              this.keyValuePair = res.element;
            })
  }

  isValid(){
    return this.model.VerificationCode ? (this.model.VerificationCode == this.keyValuePair.KEY) : false;
  }


}
