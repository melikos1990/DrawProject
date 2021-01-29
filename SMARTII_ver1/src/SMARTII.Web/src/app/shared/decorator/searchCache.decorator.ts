import { CacheService } from '../service/cache.service';
import { AppInjector } from 'src/global';
import { SessionStorageService } from '../service/session-storage.service';

export const defaultCacheKeys = {
    model: "model",
    ajax: "ajax",
}

export type CacheConfig = {
    [key: string]: string
}

export function SearchCacheMethod(prfix: string, config: CacheConfig = defaultCacheKeys) {
    return function (target: Object,
        method: string,
        descriptor: TypedPropertyDescriptor<any>) {

        const originalMethod = descriptor.value;

        descriptor.value = function (...args: any[]) {

            const sessionStorage: SessionStorageService = AppInjector.get(SessionStorageService);

            for(let key in config){
                let propName = config[key];
                let data = !this[propName] ? null : this[propName];
                sessionStorage.set(`${prfix}-${key}`, data);
            }


            const result = originalMethod.apply(this, args);
            return result;
        };

        return descriptor;
    }
}