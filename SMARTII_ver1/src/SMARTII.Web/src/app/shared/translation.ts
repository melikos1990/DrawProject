import { Injectable } from '@angular/core';
import _default from '../../assets/i18n/zh-tw';
import { Culture } from '../../assets/i18n';
import { TranslateLoader } from '@ngx-translate/core';
import { AuthenticationService } from './service/authentication.service';
import { from, of } from 'rxjs';
import { pluck } from 'rxjs/operators';
export function GenericClass<Props>(): new () => Props {
  return class { } as any;
}

function concatIfExistsPath(path: string, suffix: string): string {
  return path ? `${path}.${suffix}` : suffix;
}

function transformObjectToPath<T extends object | string>(
  suffix: string,
  objectToTransformOrEndOfPath: T,
  path = ''
): T {
  return typeof objectToTransformOrEndOfPath === 'object'
    ? Object.entries(objectToTransformOrEndOfPath).reduce(
      (objectToTransform, [key, value]) => {
        objectToTransform[key] = transformObjectToPath(
          key,
          value,
          concatIfExistsPath(path, suffix)
        );

        return objectToTransform;
      },
      {} as T
    )
    : (concatIfExistsPath(path, suffix) as T);
}


@Injectable()
export class Translations extends GenericClass<Culture>() {
  constructor() {
    super();
    Object.assign(this, transformObjectToPath('', _default));
  }
}

export class WebpackTranslateLoader implements TranslateLoader {

  constructor(private authService: AuthenticationService) { }

  getTranslation(lang: string) {

    const cacheLangSetting = this.authService.getLanguages(lang);

    const language$ =
      (true) ?
        from(import(`../../assets/i18n/${lang}.ts`)).pipe(pluck('default')) :
        of(cacheLangSetting);

    return language$;
  }
}
