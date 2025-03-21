import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantationPlantDetailsComponent } from './plantation-plant-details.component';

describe('PlantationPlantDetailsComponent', () => {
  let component: PlantationPlantDetailsComponent;
  let fixture: ComponentFixture<PlantationPlantDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantationPlantDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantationPlantDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
