import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';

@Component({
  selector: 'app-plan-edit',
  templateUrl: './plan-edit.component.html',
  styleUrls: ['./plan-edit.component.css']
})
export class PlanEditComponent {

  id!:number;
  specifiedPlan! : FitnessPlanDto;
  updateForm!: FormGroup ;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService : FitnessPlanClient,
    private router: Router,
    private formBuilder: FormBuilder)
    { }
    ngOnInit(): void {
      this.id = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
      this.FitnessPlanService.get(this.id).subscribe(element => this.specifiedPlan = element);

      this.updateForm = this.formBuilder.group({
        name: this.formBuilder.control(this.specifiedPlan?.name),
        description: this.formBuilder.control(this.specifiedPlan?.description)
      })
   }

   onSubmit(){
      //var editedPlan= new FitnessPlanDto;
      if(this.updateForm.get('name')?.value!= null){
      this.specifiedPlan.name= this.updateForm.get('name')?.value;
      }
      if(this.updateForm.get('description')?.value != null){
      this.specifiedPlan.description= this.updateForm.get('description')?.value;
      }
      this.FitnessPlanService.put(this.id,this.specifiedPlan).subscribe();
      this.router.navigate(['/edit-fitness-plans'])
   }
   onSelectBack(){
    this.router.navigate(['/edit-fitness-plans'])
   }
}
