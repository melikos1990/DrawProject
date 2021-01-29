import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobInformationComponent } from './job-information.component';

describe('JobInformationComponent', () => {
  let component: JobInformationComponent;
  let fixture: ComponentFixture<JobInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
