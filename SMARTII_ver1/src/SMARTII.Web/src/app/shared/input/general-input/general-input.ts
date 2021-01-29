import { NgInputBase } from 'ptc-dynamic-form';


export class GeneralInput extends NgInputBase {
  private key = 'GeneralInputComponent';
  public disabled: boolean;
  constructor(id?, name?, lable?, validator = null) {
    super();

    this.id = id;
    this.name = name;
    this.lable = lable;
    this.validator = validator;
    this.component = this.key;

    this.groupClass = 'col-sm-12 mt-3';
    this.lableClass = 'col-sm-4';
    this.inputClass = 'col-sm-8';
    this.containerClass = 'col-sm-6';
  }

  type: 'text' | 'number' | 'password' = 'text';
}
