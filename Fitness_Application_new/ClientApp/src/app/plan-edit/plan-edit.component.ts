import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-plan-edit',
  templateUrl: './plan-edit.component.html',
  styleUrls: ['./plan-edit.component.css']
})
export class PlanEditComponent {

  id!: number;
  specifiedPlan: FitnessPlanDto = new FitnessPlanDto;
  updateForm!: FormGroup;
  editFitnessExerciseForm: FormGroup;
  addFitnessExerciseForm: FormGroup;
  fitnessExercises: FitnessExerciseDto[] = [];
  specifiedFitnessExercises: FitnessExerciseDto[] = [];
  fitnessExerciseToBeAdded: FitnessExerciseDto[] = [];
  closeResult: string = "";
  filedata: any = undefined;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService: FitnessPlanClient,
    private router: Router,
    private formBuilder: FormBuilder,
    private FitnessExerciseService: FitnessExerciseClient,
    private modalService: NgbModal,
    private http: HttpClient,
    private _snackBar: MatSnackBar) {
    this.editFitnessExerciseForm = this.formBuilder.group({
      id: 0,
      name: '',
      description: '',
      pictureUrl: '',
      difficulty: '',
    })
    this.addFitnessExerciseForm = this.formBuilder.group({
      id: 0,
      name: '',
      description: '',
      pictureUrl: '',
      difficulty: '',
    })


  }
  ngOnInit(): void {
    this.id = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
    this.FitnessPlanService.get(this.id).subscribe(element => this.specifiedPlan = element);


    this.FitnessExerciseService.getAll().subscribe(element => {
      this.fitnessExercises = element;
      this.fitnessExercises.forEach(exercise => {
        if (exercise.fitnessPlanId == this.id) {
          this.specifiedFitnessExercises.push(exercise);
        }
      });
    });

    this.updateForm = this.formBuilder.group({
      name: this.formBuilder.control(this.specifiedPlan?.name),
      description: this.formBuilder.control(this.specifiedPlan?.description)
    })
  }

  onSubmit() {
    if (this.updateForm.get('name')?.value != null) {
      this.specifiedPlan.name = this.updateForm.get('name')?.value;
    }
    if (this.updateForm.get('description')?.value != null) {
      this.specifiedPlan.description = this.updateForm.get('description')?.value;
    }
    this.FitnessPlanService.put(this.id, this.specifiedPlan).subscribe();
    this.openSnackBar("Fitness plan edited", "dismiss");
    //this.router.navigate(['/edit-fitness-plans'])
  }
  onSelectBack() {
    this.router.navigate(['/edit-fitness-plans'])
  }

  fileEvent(e: any) {
    this.filedata = e.target.files[0];
    console.log("FILE SAVED");
  }
  open(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    this.filedata = undefined;
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  EditExerciseList(exerciseId: number) {
    let indexToUpdate = this.specifiedFitnessExercises.findIndex(item => item.id === exerciseId)
    var newFitnessExercise = new FitnessExerciseDto({
      id: exerciseId,
      name: this.editFitnessExerciseForm.get('name')?.value,
      description: this.editFitnessExerciseForm.get('description')?.value,
      //keep image url, it will be changed in the backend if there is a new one
      pictureUrl: this.specifiedFitnessExercises[indexToUpdate].pictureUrl,
      difficulty: this.editFitnessExerciseForm.get('difficulty')?.value,
      fitnessPlanId: this.id
    })
    this.FitnessExerciseService.put(exerciseId,
      newFitnessExercise).subscribe();

    if (this.filedata != undefined) {

      var myFormData = new FormData();
      const headers = new HttpHeaders();
      headers.append('Content-Type', 'multipart/form-data');
      headers.append('Accept', 'application/json');
      myFormData.append('image', this.filedata);
      myFormData.append('id', exerciseId.toString());
      /* Image Post Request */
      this.http.post('https://fitnessappapi2023.azurewebsites.net/api/FitnessExercise/SavePicture', myFormData, {
        headers: headers
      }).subscribe(data => {
        //Check success message
        console.log(data);


        window.location.reload();
      });
    }
    //show the update
    this.specifiedFitnessExercises[indexToUpdate] = newFitnessExercise;
    this.openSnackBar("Fitness exercise edited", "dismiss");
    this.filedata = undefined;
  }

  selectexercise(exercise: FitnessExerciseDto) {
    this.editFitnessExerciseForm = this.formBuilder.group({
      id: this.formBuilder.control(exercise.id),
      name: this.formBuilder.control(exercise.name),
      description: this.formBuilder.control(exercise.description),
      pictureUrl: '',
      difficulty: this.formBuilder.control(exercise.difficulty),
    })
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      verticalPosition: 'bottom', horizontalPosition: 'center',
      duration: 2000,
      panelClass: ['blue-snackbar']
    });
  }
  async AddToExerciseList() {
    var newFitnessExercise = new FitnessExerciseDto({
      id: 0,
      name: this.addFitnessExerciseForm.get('name')?.value,
      description: this.addFitnessExerciseForm.get('description')?.value,
      difficulty: this.addFitnessExerciseForm.get('difficulty')?.value,
      fitnessPlanId: this.id
    })
    //exercise save
    const exerciseResponse$ = this.FitnessExerciseService.addFitnessExercise(newFitnessExercise);
      var lastExercise = await lastValueFrom(exerciseResponse$);
      var newExerciseId = Number(await lastExercise?.data.text());

      //picture save
    if (this.filedata != undefined) {

      var myFormData = new FormData();
      const headers = new HttpHeaders();
      headers.append('Content-Type', 'multipart/form-data');
      headers.append('Accept', 'application/json');
      myFormData.append('image', this.filedata);
      myFormData.append('id', newExerciseId.toString());
      /* Image Post Request */
      this.http.post('https://fitnessappapi2023.azurewebsites.net/api/FitnessExercise/SavePicture', myFormData, {
        headers: headers
      }).subscribe(data => {
        //Check success message
        console.log(data);


        window.location.reload();
      });
    }
    //show the update
    this.specifiedFitnessExercises.push(newFitnessExercise);
    this.openSnackBar("Fitness exercise added", "dismiss");
    this.filedata = undefined;
  }
  deleteExercise(exerciseToDelete: FitnessExerciseDto){
    if (confirm("Are you sure you want to delete fitness exercie " + exerciseToDelete.name + " ?")) {
    this.FitnessExerciseService.delete(exerciseToDelete.id).subscribe();
    this.specifiedFitnessExercises = this.specifiedFitnessExercises.filter(item=> item.id!=exerciseToDelete.id)
    this.openSnackBar("Fitness exercise deleted", "dismiss");
    }
  }
}
