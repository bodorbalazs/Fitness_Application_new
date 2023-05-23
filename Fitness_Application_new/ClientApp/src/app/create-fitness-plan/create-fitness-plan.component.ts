import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import {NgbModal,ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Event } from '@angular/router';

interface FitnessExerciseWithPicture{
  fitnessExercise: FitnessExerciseDto;
  picture : any;
}
@Component({
  selector: 'app-create-fitness-plan',
  templateUrl: './create-fitness-plan.component.html',
  styleUrls: ['./create-fitness-plan.component.css']
})
export class CreateFitnessPlanComponent implements OnInit {
public addFitnessPlanForm: FormGroup;
public addFitnessExerciseForm: FormGroup;
public fitnessPlanList: FitnessPlanDto[] =[];
public fitnessExerciseLocalListWithPicture: FitnessExerciseWithPicture[] =[];
public fitnessExerciseList: FitnessExerciseDto[] =[];
public exercises: number =0;
public lastExerciseId =0;
fileToUpload: File | null = null;
closeResult:string ="";
  fulllist: any;
  filedata:any;

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService:NgbModal,
    private http:HttpClient) 
    {
      this.addFitnessPlanForm = this.formBuilder.group({
        name:'',
        description:''
      })
      this.addFitnessExerciseForm = this.formBuilder.group({
        id:0,
        name:'',
        description:'',
        pictureUrl:'',
        difficulty:'',
      })

    }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element => this.fitnessPlanList = element)
  }

  SetExerciseList(){
    this.fitnessExerciseLocalListWithPicture.push({
      fitnessExercise : new FitnessExerciseDto({
              id:0,
              name:this.addFitnessExerciseForm.get('name')?.value,
              description:this.addFitnessExerciseForm.get('description')?.value,
              pictureUrl:this.addFitnessExerciseForm.get('pictureUrl')?.value,
              difficulty:this.addFitnessExerciseForm.get('difficulty')?.value
              }),
      picture: this.filedata
  }
    )
  }

  fileEvent(e:any){
    this.filedata = e.target.files[0];
  }

 setFitnessPlan(){
  

  this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element);
 

  this.fitnessPlanService.addFitnessPlan(new FitnessPlanDto({
    id:0,
    name:this.addFitnessPlanForm.get('name')?.value,
    description:this.addFitnessPlanForm.get('description')?.value
  })).subscribe();
  this.setExercisesForEvent();

  this.fitnessExerciseLocalListWithPicture =[];
 }
 setExercisesForEvent(){
  
  let lastPlanId = this.fitnessPlanList[this.fitnessPlanList.length-1].id+1;
  this.exercises = this.fitnessExerciseLocalListWithPicture.length;//possibly obsolete
  this.fitnessExerciseLocalListWithPicture.forEach(element =>{
    element.fitnessExercise.fitnessPlanId=lastPlanId;
  })
  this.lastExerciseId = this.fitnessPlanList[this.fitnessExerciseList.length-1].id+1;
  this.fitnessExerciseLocalListWithPicture.forEach(element =>{
    this.fitnessExerciseService.addFitnessExercise(element.fitnessExercise).subscribe();
    this.filedata.fileName=this.lastExerciseId.toString();
    var myFormData = new FormData();
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    myFormData.append('image', this.filedata);
    /* Image Post Request */
    this.http.post('https://localhost:7252/api/FitnessExercise/SavePicture', myFormData, {
    headers: headers
    }).subscribe(data => {
     //Check success message
     console.log(data);
    });
    this.lastExerciseId +=1;

  });
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
