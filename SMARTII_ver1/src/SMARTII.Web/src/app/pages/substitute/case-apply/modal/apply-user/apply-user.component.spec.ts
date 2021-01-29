import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyUserComponent } from './apply-user.component';

describe('ApplyUserComponent', () => {
  let component: ApplyUserComponent;
  let fixture: ComponentFixture<ApplyUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplyUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplyUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
