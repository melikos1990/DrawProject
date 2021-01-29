import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PtcTreeComponent } from './ptc-tree.component';

describe('PtcTreeComponent', () => {
  let component: PtcTreeComponent;
  let fixture: ComponentFixture<PtcTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PtcTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PtcTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
