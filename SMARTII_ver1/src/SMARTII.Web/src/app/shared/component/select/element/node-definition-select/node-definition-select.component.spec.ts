import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NodeDefinitionSelectComponent } from './node-definition-select.component';

describe('NodeDefinitionSelectComponent', () => {
  let component: NodeDefinitionSelectComponent;
  let fixture: ComponentFixture<NodeDefinitionSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NodeDefinitionSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NodeDefinitionSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
