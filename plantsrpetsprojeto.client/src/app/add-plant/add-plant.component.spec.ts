import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPlantComponent } from './add-plant.component';

describe('AddPlantComponent', () => {
  let component: AddPlantComponent;
  let fixture: ComponentFixture<AddPlantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddPlantComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPlantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
