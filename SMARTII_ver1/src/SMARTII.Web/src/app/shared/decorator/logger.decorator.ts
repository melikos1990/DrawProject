
import { AuthenticationType } from 'src/app/model/authorize.model';
import { LogService } from '../service/log.service';
import { AppInjector } from 'src/global';
export function loggerClass(feature?: string) {

  // const inject = ReflectiveInjector.resolveAndCreate([
  //     LogService
  // ])
  return function (constructor: any) {

    // let log: LogService = inject.get(LogService);

    // const component = constructor.name;
    // log.logger.info(component);
  }

}

export function loggerMethod(feature?: string, authenticationType?: AuthenticationType) {

  return function loggerMethod(target: Object,
    method: string,
    descriptor: TypedPropertyDescriptor<any>) {

    const originalMethod = descriptor.value;

    descriptor.value = function (...args: any[]) {

      const log: LogService = AppInjector.get(LogService);

      log.logger.info(method);
      const result = originalMethod.apply(this, args);
      return result;
    };

    return descriptor;
  }
}
