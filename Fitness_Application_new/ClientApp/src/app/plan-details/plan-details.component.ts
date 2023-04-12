import { Component,ViewChild } from '@angular/core';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import {MatAccordion} from '@angular/material/expansion';
import { AUTO_STYLE,trigger, state, style, transition, animate } from '@angular/animations';
@Component({
  selector: 'app-plan-details',
  templateUrl: './plan-details.component.html',
  styleUrls: ['./plan-details.component.css'],
})
export class PlanDetailsComponent {

  id!:number;
  specifiedPlan! : FitnessPlanDto;
  fitnessExercises: FitnessExerciseDto[]=[];
  specifiedFitnessExercises: FitnessExerciseDto[]=[];
  panelOpenState= false;
  @ViewChild(MatAccordion) accordion!: MatAccordion;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService : FitnessPlanClient,
    private router: Router,
    private FitnessExerciseService: FitnessExerciseClient)
  { }

  ngOnInit():void{
    this.id = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
    this.FitnessPlanService.get(this.id).subscribe(element => this.specifiedPlan = element);
    this.FitnessExerciseService.getAll().subscribe(element => this.fitnessExercises= element);
    this.fitnessExercises.forEach(element => {
      if(element.fitnessPlanId== this.id){
        this.specifiedFitnessExercises.push(element);
      }
    });
  }
}
