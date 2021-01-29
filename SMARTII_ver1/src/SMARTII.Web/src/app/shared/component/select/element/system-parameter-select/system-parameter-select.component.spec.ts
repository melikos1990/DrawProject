import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemParameterSelectComponent } from './system-parameter-select.component';

describe('SystemParameterSelectComponent', () => {
  let component: SystemParameterSelectComponent;
  let fixture: ComponentFixture<SystemParameterSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SystemParameterSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemParameterSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
