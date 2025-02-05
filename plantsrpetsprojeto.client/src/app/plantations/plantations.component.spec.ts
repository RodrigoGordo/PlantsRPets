import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantationsComponent } from './plantations.component';

describe('PlantationsComponent', () => {
  let component: PlantationsComponent;
  let fixture: ComponentFixture<PlantationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlantationsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlantationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
