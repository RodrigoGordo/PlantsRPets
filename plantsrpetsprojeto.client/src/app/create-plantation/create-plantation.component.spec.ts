import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlantationComponent } from './create-plantation.component';

describe('CreatePlantationComponent', () => {
  let component: CreatePlantationComponent;
  let fixture: ComponentFixture<CreatePlantationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatePlantationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlantationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
