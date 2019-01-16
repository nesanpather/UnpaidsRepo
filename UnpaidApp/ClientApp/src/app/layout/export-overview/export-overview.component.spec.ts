import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportOverviewComponent } from './export-overview.component';

describe('ExportOverviewComponent', () => {
  let component: ExportOverviewComponent;
  let fixture: ComponentFixture<ExportOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
