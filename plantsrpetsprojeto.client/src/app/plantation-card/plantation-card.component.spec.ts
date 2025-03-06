import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantationCardComponent } from './plantation-card.component';

describe('PlantationCardComponent', () => {
  let component: PlantationCardComponent;
  let fixture: ComponentFixture<PlantationCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantationCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantationCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
