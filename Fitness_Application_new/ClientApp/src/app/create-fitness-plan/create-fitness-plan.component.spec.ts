import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateFitnessPlanComponent } from './create-fitness-plan.component';

describe('CreateFitnessPlanComponent', () => {
  let component: CreateFitnessPlanComponent;
  let fixture: ComponentFixture<CreateFitnessPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateFitnessPlanComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateFitnessPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
