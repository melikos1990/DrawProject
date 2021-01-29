import { AuthenticationService } from '../service/authentication.service';
import { AppInjector } from 'src/global';
import { AuthenticationType } from 'src/app/model/authorize.model';


export function AuthorizeClass(feature?: string) {


  return function (constructor: any) {

    let auth: AuthenticationService = AppInjector.get(AuthenticationService);

    const targetNgOnInit: Function = constructor.prototype.ngOnInit;

    function ngOnInit(...args): void {


      auth.onClassAuthorization(feature).subscribe(hasAuth => {

        if (hasAuth == false) {
          throw new Error(`無功能權限  功能別 : ${feature}`);
        } else {
          if (targetNgOnInit) {
            targetNgOnInit.apply(this, args);
          }
        }
      });


    }

    constructor.prototype.ngOnInit = ngOnInit;

  }

}

export function AuthorizeMethod(feature?: string, authenticationType?: AuthenticationType) {

  return function (target: Object,
    method: string,
    descriptor: TypedPropertyDescriptor<any>) {

    var originalMethod = descriptor.value;

    descriptor.value = function (...args: any[]) {

      let auth: AuthenticationService = AppInjector.get(AuthenticationService);

      auth.onMethodAuthorization(feature, authenticationType).subscribe(hasAuth => {

        if (hasAuth == false) {
          throw new Error(`無功能權限 , 功能別 : ${feature}  操作權限 : ${JSON.stringify(authenticationType)}`);
        } else {
          let result = originalMethod.apply(this, args);
          return result;
        }
      });

    };

    return descriptor;


  }
}

