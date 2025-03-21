import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PetRewardPopupComponent } from './pet-reward-popup.component';

describe('PetRewardPopupComponent', () => {
  let component: PetRewardPopupComponent;
  let fixture: ComponentFixture<PetRewardPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PetRewardPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PetRewardPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
