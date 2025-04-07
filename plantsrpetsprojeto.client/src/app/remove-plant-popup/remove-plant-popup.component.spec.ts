import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemovePlantPopupComponent } from './remove-plant-popup.component';

describe('RemovePlantPopupComponent', () => {
  let component: RemovePlantPopupComponent;
  let fixture: ComponentFixture<RemovePlantPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RemovePlantPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemovePlantPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
