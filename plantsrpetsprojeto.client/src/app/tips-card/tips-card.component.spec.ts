import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TipsCardComponent } from './tips-card.component';

describe('TipsCardComponent', () => {
  let component: TipsCardComponent;
  let fixture: ComponentFixture<TipsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TipsCardComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(TipsCardComponent);
    component = fixture.componentInstance;
    component.tips = [{
      tipId: 0,
      plantInfoId: 0,
      tipDescription: "test",
      tipType: "test"
    }];

    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
