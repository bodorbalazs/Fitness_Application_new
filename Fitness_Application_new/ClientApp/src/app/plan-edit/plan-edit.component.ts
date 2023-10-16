import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import { NgbModal,ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-plan-edit',
  templateUrl: './plan-edit.component.html',
  styleUrls: ['./plan-edit.component.css']
})
export class PlanEditComponent {

  id!:number;
  specifiedPlan! : FitnessPlanDto;
  updateForm!: FormGroup ;
  editFitnessExerciseForm: FormGroup;
  fitnessExercises: FitnessExerciseDto[]=[];
  specifiedFitnessExercises: FitnessExerciseDto[]=[];
  closeResult:string ="";
  filedata:any = undefined;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService : FitnessPlanClient,
    private router: Router,
    private formBuilder: FormBuilder,
    private FitnessExerciseService : FitnessExerciseClient,
    private modalService:NgbModal,
    private http:HttpClient)
    {
      this.editFitnessExerciseForm = this.formBuilder.group({
        id:0,
        name:'',
        description:'',
        pictureUrl:'',
        difficulty:'',
      })


     }
    ngOnInit(): void {
      this.id = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
      this.FitnessPlanService.get(this.id).subscribe(element => this.specifiedPlan = element );
      

      this.FitnessExerciseService.getAll().subscribe(element =>
        {
          this.fitnessExercises= element;
          this.fitnessExercises.forEach(exercise => {
            if(exercise.fitnessPlanId== this.id){
              this.specifiedFitnessExercises.push(exercise);
            }
          });
        });

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

   fileEvent(e:any){
    this.filedata = e.target.files[0];
    console.log("FILE SAVED");
  }
   open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    this.filedata=undefined;
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  EditExerciseList(exerciseId:number){

    this.FitnessExerciseService.put(exerciseId,new FitnessExerciseDto({
                id:exerciseId,
                name:this.editFitnessExerciseForm.get('name')?.value,
                description:this.editFitnessExerciseForm.get('description')?.value,
                pictureUrl:this.editFitnessExerciseForm.get('pictureUrl')?.value,
                difficulty:this.editFitnessExerciseForm.get('difficulty')?.value,
                fitnessPlanId:this.id
                })
                ).subscribe();

                if(this.filedata!=undefined){
      
                  var myFormData = new FormData();
                  const headers = new HttpHeaders();
                  headers.append('Content-Type', 'multipart/form-data');
                  headers.append('Accept', 'application/json');
                  myFormData.append('image', this.filedata);
                  myFormData.append('id',exerciseId.toString());
                  /* Image Post Request */
                  this.http.post('https://fitnessappapi2023.azurewebsites.net/api/FitnessExercise/SavePicture', myFormData, {
                  headers: headers
                  }).subscribe(data => {
                   //Check success message
                   console.log(data);
                   //window.location.reload();
                  });
                }
      this.filedata = undefined;
  }

  selectexercise(exercise:FitnessExerciseDto){
    this.editFitnessExerciseForm = this.formBuilder.group({
      id: this.formBuilder.control(exercise.id),
      name:this.formBuilder.control(exercise.name),
      description:this.formBuilder.control(exercise.description),
      pictureUrl:'',
      difficulty:this.formBuilder.control(exercise.difficulty),
    })
  }

}
