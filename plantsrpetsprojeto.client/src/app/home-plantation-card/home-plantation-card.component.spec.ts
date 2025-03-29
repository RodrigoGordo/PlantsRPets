import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomePlantationCardComponent } from './home-plantation-card.component';

describe('HomePlantationCardComponent', () => {
  let component: HomePlantationCardComponent;
  let fixture: ComponentFixture<HomePlantationCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomePlantationCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomePlantationCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
