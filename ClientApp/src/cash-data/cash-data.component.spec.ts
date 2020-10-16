import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CashDataComponent } from './cash-data.component';

describe('CashDataComponent', () => {
  let component: CashDataComponent;
  let fixture: ComponentFixture<CashDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CashDataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CashDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
