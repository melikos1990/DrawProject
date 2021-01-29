import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorNodeCreateComponent } from './vendor-node-create.component';

describe('VendorNodeCreateComponent', () => {
  let component: VendorNodeCreateComponent;
  let fixture: ComponentFixture<VendorNodeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VendorNodeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VendorNodeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
