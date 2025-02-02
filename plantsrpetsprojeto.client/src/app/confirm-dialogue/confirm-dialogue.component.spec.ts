import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDialogueComponent } from './confirm-dialogue.component';

describe('ConfirmDialogueComponent', () => {
  let component: ConfirmDialogueComponent;
  let fixture: ComponentFixture<ConfirmDialogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConfirmDialogueComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
