import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VenderNodeTreeComponent } from './vender-node-tree.component';

describe('VenderNodeTreeComponent', () => {
  let component: VenderNodeTreeComponent;
  let fixture: ComponentFixture<VenderNodeTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VenderNodeTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VenderNodeTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
