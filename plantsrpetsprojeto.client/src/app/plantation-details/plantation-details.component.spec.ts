import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantationDetailsComponent } from './plantation-details.component';

describe('PlantationDetailsComponent', () => {
  let component: PlantationDetailsComponent;
  let fixture: ComponentFixture<PlantationDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantationDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
