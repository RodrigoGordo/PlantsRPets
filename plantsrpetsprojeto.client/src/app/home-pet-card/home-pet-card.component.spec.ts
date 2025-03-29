import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomePetCardComponent } from './home-pet-card.component';

describe('HomePetCardComponent', () => {
  let component: HomePetCardComponent;
  let fixture: ComponentFixture<HomePetCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomePetCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomePetCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
