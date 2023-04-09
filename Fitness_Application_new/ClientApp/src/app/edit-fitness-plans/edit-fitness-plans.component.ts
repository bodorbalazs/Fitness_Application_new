import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';

@Component({
  selector: 'app-edit-fitness-plans',
  templateUrl: './edit-fitness-plans.component.html',
  styleUrls: ['./edit-fitness-plans.component.css']
})
export class EditFitnessPlansComponent implements OnInit{


  public addFitnessPlanForm: FormGroup;
  public addFitnessExerciseForm: FormGroup;
  public fitnessPlanList: FitnessPlanDto[] =[];
  public fitnessPlanEdited: FitnessPlanDto = new FitnessPlanDto  ;
  public fitnessExerciseList: FitnessExerciseDto[] =[];
  closeResult:string ="";
  PlanName: any;
  PlanDecription: any;

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService:NgbModal) 
    {
      this.addFitnessPlanForm = this.formBuilder.group({
        name: '',
        description:''
      })
      this.addFitnessExerciseForm = this.formBuilder.group({
        name:'',
        description:'',
        pictureUrl:'',
        difficulty:''
      })

    }
    ngOnInit(): void {  // TODO instead of getAll() , getCUrrentUsersPlans()
      this.fitnessPlanService.getUsersPlans().subscribe(element => this.fitnessPlanList = element);
      this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element);
    }
    setFitnessPlan(id: number) {
      this.fitnessPlanService.get(id).subscribe(element=> this.fitnessPlanEdited);
      var FitnessPlanName;
      var FitnessPlanDescription;
      
      if(this.addFitnessPlanForm.get('name')?.value==''){
         FitnessPlanName = this.fitnessPlanEdited.name;
      }else{
         FitnessPlanName=this.addFitnessPlanForm.get('name')?.value
      }

      if(this.addFitnessPlanForm.get('description')?.value==''){
        FitnessPlanDescription = this.fitnessPlanEdited.name;
     }else{
      FitnessPlanDescription=this.addFitnessPlanForm.get('description')?.value
     }
      this.fitnessPlanEdited.name=FitnessPlanName;
      this.fitnessPlanEdited.description=FitnessPlanDescription;
      this.fitnessPlanService.put(id,this.fitnessPlanEdited).subscribe();
      }
    editFitnessPlan(id:number) {
       this.fitnessPlanService.get(id).subscribe(element=> this.fitnessPlanEdited);
       this.PlanName = this.fitnessPlanEdited.name;
       this.PlanDecription =this.fitnessPlanEdited.description;
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
