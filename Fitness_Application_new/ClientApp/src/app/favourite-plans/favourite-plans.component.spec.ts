import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavouritePlansComponent } from './favourite-plans.component';

describe('FavouritePlansComponent', () => {
  let component: FavouritePlansComponent;
  let fixture: ComponentFixture<FavouritePlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FavouritePlansComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(FavouritePlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
