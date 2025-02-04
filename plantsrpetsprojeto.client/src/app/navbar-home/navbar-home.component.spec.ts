import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarHomeComponent } from './navbar-home.component';

describe('NavbarHomeComponent', () => {
  let component: NavbarHomeComponent;
  let fixture: ComponentFixture<NavbarHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NavbarHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
