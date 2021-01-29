import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobUserInformationComponent } from './job-user-information.component';

describe('JobUserInformationComponent', () => {
  let component: JobUserInformationComponent;
  let fixture: ComponentFixture<JobUserInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobUserInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobUserInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
