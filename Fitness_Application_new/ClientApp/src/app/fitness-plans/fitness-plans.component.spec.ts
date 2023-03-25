import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FitnessPlansComponent } from './fitness-plans.component';

describe('FitnessPlansComponent', () => {
  let component: FitnessPlansComponent;
  let fixture: ComponentFixture<FitnessPlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FitnessPlansComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FitnessPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
