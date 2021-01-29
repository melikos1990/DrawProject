import { InjectionToken } from '@angular/core';
import { IMenuSelect } from './service/menu-select.service';
import { NgInputBase } from 'ptc-dynamic-form';

export const DEFAULT_TIMEOUT = new InjectionToken<number>('defaultTimeout');
export const DYNAMIC_PAYLOADS = new InjectionToken<{ key: string, payload: NgInputBase }[]>('DYNAMIC_PAYLOADS');
export const MENU_SELECT = new InjectionToken<IMenuSelect>('MENU_SELECT');
export const PREFIX_TOKEN = new InjectionToken<string>('PREFIX');
export const DEFAULT_REFESH_TOKEN_URL_CHARATOR = new InjectionToken<number>('defaultRefreshTokenUrl');
export const DEFAULT_LOGIN_URL_CHARATOR = new InjectionToken<number>('defaultLoginUrl');
