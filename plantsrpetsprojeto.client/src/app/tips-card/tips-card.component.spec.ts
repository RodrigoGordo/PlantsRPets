import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TipCardsComponent } from './tips-card.component';

describe('TipsCardComponent', () => {
  let component: TipCardsComponent;
  let fixture: ComponentFixture<TipCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TipCardsComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(TipCardsComponent);
    component = fixture.componentInstance;
    component.tips = [{
      tipType: 'Watering',
      tipDescription: 'Sample watering tip content...',
    }];

    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
