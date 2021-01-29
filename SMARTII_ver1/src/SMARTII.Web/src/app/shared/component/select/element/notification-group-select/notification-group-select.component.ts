import { Component, OnInit, Input, forwardRef, Injector, ApplicationRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { HttpService } from 'src/app/shared/service/http.service';
import { BaseComponent } from 'src/app/pages/base/base.component';


export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => NotificationGroupSelectComponent),
  multi: true
};


@Component({
  selector: 'app-notification-group-select',
  templateUrl: './notification-group-select.component.html',
  styleUrls: ['./notification-group-select.component.scss'],
  providers: [INPUT_CONTROL_VALUE_ACCESSOR]

})
export class NotificationGroupSelectComponent extends BaseComponent implements OnInit {

  @Input() multiple: boolean = false;
  @Input() placeholder: string = '';
  @Input() noDataText: string = '';
  @Input() searchText: string = '';
  @Input() disabled: boolean;

  @Input() nodeID: number;

  private fetched: boolean = false;
  public innerValue: any;
  public isDisabled: boolean = false;
  public items: any[] = [];

  public event: () => void;

  public onTouchedCallback: () => void = () => { };
  public onChangeCallback: (_: any) => void = (_: any) => { };

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v != this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(this.innerValue);
    }
  }

  constructor(
    public http: HttpService,
    public injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.http.post("Common/Notification/GetNotificationGroup", { nodeID: this.nodeID }, {}).subscribe((resp: any) => {
      this.items = resp.items;
      this.fetched = true;
      this.event && this.event();
    });
  }
  writeValue(obj: any): void {

    if (this.fetched) {
      if (obj !== this.innerValue) {
        this.innerValue = obj;
      }
    } else {
      this.event = () => {
        if (obj != this.innerValue) {
          this.innerValue = obj;
        }
      }
    }

  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }


}



