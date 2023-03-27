import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFitnessPlansComponent } from './edit-fitness-plans.component';

describe('EditFitnessPlansComponent', () => {
  let component: EditFitnessPlansComponent;
  let fixture: ComponentFixture<EditFitnessPlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditFitnessPlansComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditFitnessPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
