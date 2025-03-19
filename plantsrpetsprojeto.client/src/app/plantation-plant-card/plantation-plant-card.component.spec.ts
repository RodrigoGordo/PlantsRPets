import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantationPlantCardComponent } from './plantation-plant-card.component';

describe('PlantationPlantCardComponent', () => {
  let component: PlantationPlantCardComponent;
  let fixture: ComponentFixture<PlantationPlantCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantationPlantCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantationPlantCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
