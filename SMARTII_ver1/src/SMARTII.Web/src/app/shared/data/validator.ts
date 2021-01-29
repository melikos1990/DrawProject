import { FormControl } from '@angular/forms';



export function NumberValidator(control: FormControl): { [key: string]: any } {
  const value: string = control.value || '';
  const valid = String(value).match(/^[0-9]+(\.?[0-9]+)?$/);
  return value ? (valid ? null : { number: true }) : null;
}

export function PositiveNumberValidator(control: FormControl): { [key: string]: any } {
  const value: string = control.value || '';
  const valid = String(value).match(/^[1-9]\d*$/);
  return value ? (valid ? null : { positiveNumber: true }) : null;
}

export function passwordRule(c: FormControl) {

  const password = !!c.value ? c.value : "";
  const _regex = /[$-/:-?{-~!"^_`\[\]]/g; // "

  const _lowerLetters = /[a-z]+/.test(password);
  const _upperLetters = /[A-Z]+/.test(password);
  const _numbers = /[0-9]+/.test(password);
  const _symbols = _regex.test(password);

  const _flags = [_lowerLetters, _upperLetters, _numbers, _symbols];

  let _passedMatches = 0;
  for (const _flag of _flags) {
    _passedMatches += _flag === true ? 1 : 0;
  }

  return (_passedMatches >= 3 && password.length >= 8) ? null : {
    passwordRule: {
      valid: false
    }
  };

}

export function AccountValidator(control: FormControl): { [key: string]: any } {
  const value: string = control.value || '';
  const valid = String(value).match(/^[^\/|\\|\[|\]|\:|;|\||\=|,|\+|\*|\?|<|>|@|"]+$/);
  return value ? (valid ? null : { account: true }) : null;
}

export function TelephoneNumberValidator(control: FormControl): { [key: string]: any } {
  const value: string = control.value || '';
  const valid = String(value).match(/^09[0-9]{8}$/);
  return value ? (valid ? null : { telephoneNumber: true }) : null;
}
