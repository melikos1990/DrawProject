import { NgInputBase } from 'ptc-dynamic-form';


export class CaseItemTableInput extends NgInputBase {
  private key = 'CaseItemTableInputComponent';
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

  public nodeKey: string;

}
