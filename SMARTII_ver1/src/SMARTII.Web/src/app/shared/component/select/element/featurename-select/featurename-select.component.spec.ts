import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturenameSelectComponent } from './featurename-select.component';

describe('FeaturenameSelectComponent', () => {
  let component: FeaturenameSelectComponent;
  let fixture: ComponentFixture<FeaturenameSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeaturenameSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturenameSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
