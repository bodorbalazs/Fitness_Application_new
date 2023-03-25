import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import {NgbModal,ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-create-fitness-plan',
  templateUrl: './create-fitness-plan.component.html',
  styleUrls: ['./create-fitness-plan.component.css']
})
export class CreateFitnessPlanComponent implements OnInit {
public addFitnessPlanForm: FormGroup;
public addFitnessExerciseForm: FormGroup;
public fitnessPlanList: FitnessPlanDto[] =[];
public fitnessExerciseList: FitnessExerciseDto[] =[];
closeResult:string ="";

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService:NgbModal) 
    {
      this.addFitnessPlanForm = this.formBuilder.group({
        name:'',
        description:''
      })
      this.addFitnessExerciseForm = this.formBuilder.group({
        name:'',
        description:'',
        pictureUrl:'',
        difficulty:''
      })

    }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element => this.fitnessPlanList = element)
  }

  SetExerciseList(){

    this.fitnessExerciseList.push(new FitnessExerciseDto({
      id:0,
      name:this.addFitnessExerciseForm.get('name')?.value,
      description:this.addFitnessExerciseForm.get('description')?.value,
      pictureUrl:this.addFitnessExerciseForm.get('pictureUrl')?.value,
      difficulty:this.addFitnessExerciseForm.get('difficulty')?.value
    }))
  }









  open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

}
