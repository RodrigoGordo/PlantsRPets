import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantPeriodWarningComponent } from './plant-period-warning.component';

describe('PlantPeriodWarningComponent', () => {
  let component: PlantPeriodWarningComponent;
  let fixture: ComponentFixture<PlantPeriodWarningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantPeriodWarningComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantPeriodWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
