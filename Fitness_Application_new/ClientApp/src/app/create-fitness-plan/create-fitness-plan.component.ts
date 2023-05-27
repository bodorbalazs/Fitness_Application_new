import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import {NgbModal,ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Event } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { FileResponse } from '../clientservice/api.client';

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
public lastExercise: FileResponse | null =null;
//public exercises: number =0;
public lastExerciseId =0;
fileToUpload: File | null = null;
closeResult:string ="";
  fulllist: any;
  filedata:any = null;

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
              
      
  })
    this.filedata = undefined;
  }

  fileEvent(e:any){
    this.filedata = e.target.files[0];
    console.log("FILE SAVED");
  }

 async setFitnessPlan(){
  
  this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element);
 

  const response$ = this.fitnessPlanService.addFitnessPlan(new FitnessPlanDto({
    id:0,
    name:this.addFitnessPlanForm.get('name')?.value,
    description:this.addFitnessPlanForm.get('description')?.value
  }));
  this.lastExercise = await lastValueFrom(response$);
  const createdPlanId = Number(await this.lastExercise?.data.text());
  console.log(createdPlanId);
  
  if(!Number.isNaN(createdPlanId))
  this.setExercisesForEvent(createdPlanId);
 }
 setExercisesForEvent(newestplan:number){
  
  //this.exercises = this.fitnessExerciseLocalListWithPicture.length;//possibly obsolete
  this.fitnessExerciseLocalListWithPicture.forEach(element =>{
    element.fitnessExercise.fitnessPlanId=newestplan;
  })
  this.lastExerciseId = this.fitnessPlanList[this.fitnessPlanList.length-1].id+1;
  this.fitnessExerciseLocalListWithPicture.forEach(element =>{
    this.fitnessExerciseService.addFitnessExercise(element.fitnessExercise).subscribe();
    if(element.picture!=undefined){
    //element.picture.name=this.lastExerciseId.toString();
    //TODO:: send exercise id back with picture
    var myFormData = new FormData();
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    myFormData.append('image', element.picture);
    myFormData.append('id',this.lastExerciseId.toString());
    /* Image Post Request */
    this.http.post('https://localhost:7252/api/FitnessExercise/SavePicture', myFormData, {
    headers: headers
    }).subscribe(data => {
     //Check success message
     console.log(data);
    });
  }
    this.lastExerciseId +=1;

  });
  this.filedata=undefined;
  this.fitnessExerciseLocalListWithPicture =[];
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
