import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemovePlantationPopupComponent } from './remove-plantation-popup.component';

describe('RemovePlantationPopupComponent', () => {
  let component: RemovePlantationPopupComponent;
  let fixture: ComponentFixture<RemovePlantationPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RemovePlantationPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemovePlantationPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
