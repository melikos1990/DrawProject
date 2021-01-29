import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdPasswordComponent } from './ad-password.component';

describe('AdPasswordComponent', () => {
  let component: AdPasswordComponent;
  let fixture: ComponentFixture<AdPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
