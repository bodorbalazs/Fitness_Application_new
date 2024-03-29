import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Event } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { FileResponse } from '../clientservice/api.client';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

interface FitnessExerciseWithPicture {
  fitnessExercise: FitnessExerciseDto;
  picture: any;
}
@Component({
  selector: 'app-create-fitness-plan',
  templateUrl: './create-fitness-plan.component.html',
  styleUrls: ['./create-fitness-plan.component.css']
})
export class CreateFitnessPlanComponent implements OnInit {
  public addFitnessPlanForm: FormGroup;
  public addFitnessExerciseForm: FormGroup;
  public fitnessPlanList: FitnessPlanDto[] = [];
  public fitnessExerciseLocalListWithPicture: FitnessExerciseWithPicture[] = [];
  public fitnessExerciseList: FitnessExerciseDto[] = [];
  public lastPlan: FileResponse | null = null;
  public lastExercise: FileResponse | null = null;
  public ExerciseId = 0;
  fileToUpload: File | null = null;
  closeResult: string = "";
  fulllist: any;
  filedata: any = null;

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService: NgbModal,
    private http: HttpClient,
    private _snackBar: MatSnackBar) {
    this.addFitnessPlanForm = this.formBuilder.group({
      name: '',
      description: ''
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
    this.fitnessPlanService.getAll().subscribe(element => this.fitnessPlanList = element)
    this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element)
  }

  SetExerciseList() {
    this.fitnessExerciseLocalListWithPicture.push({
      fitnessExercise: new FitnessExerciseDto({
        id: 0,
        name: this.addFitnessExerciseForm.get('name')?.value,
        description: this.addFitnessExerciseForm.get('description')?.value,
        pictureUrl: this.addFitnessExerciseForm.get('pictureUrl')?.value,
        difficulty: this.addFitnessExerciseForm.get('difficulty')?.value
      }),
      picture: this.filedata
    })
    this.addFitnessExerciseForm.get('name')?.setValue('');
    this.addFitnessExerciseForm.get('description')?.setValue('');
    this.addFitnessExerciseForm.get('difficulty')?.setValue('');
    this.filedata = undefined;
  }

  fileEvent(e: any) {
    this.filedata = e.target.files[0];
    console.log("FILE SAVED");
  }

  async setFitnessPlan() {
    const response$ = this.fitnessPlanService.addFitnessPlan(new FitnessPlanDto({
      id: 0,
      name: this.addFitnessPlanForm.get('name')?.value,
      description: this.addFitnessPlanForm.get('description')?.value
    }));
    this.lastPlan = await lastValueFrom(response$);
    const createdPlanId = Number(await this.lastPlan?.data.text());
    console.log(createdPlanId);

    if (!Number.isNaN(createdPlanId))
      this.setExercisesForEvent(createdPlanId);
      this.addFitnessPlanForm.get('name')?.setValue('');
      this.addFitnessPlanForm.get('description')?.setValue('');
      this.openSnackBar("Fitness Plan created","dismiss");

  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      verticalPosition: 'bottom', horizontalPosition: 'center',
      duration: 2000,
      panelClass: ['blue-snackbar']
    });
  }


  setExercisesForEvent(newestplan: number) {
    this.fitnessExerciseLocalListWithPicture.forEach(element => {
      element.fitnessExercise.fitnessPlanId = newestplan;
    })
    this.fitnessExerciseLocalListWithPicture.forEach(async element => {
      //exercise save
      const exerciseResponse$ = this.fitnessExerciseService.addFitnessExercise(element.fitnessExercise);
      this.lastExercise = await lastValueFrom(exerciseResponse$);
      this.ExerciseId = Number(await this.lastExercise?.data.text());
      //picture save
      if (element.picture != undefined) {
        var myFormData = new FormData();
        const headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');
        headers.append('Accept', 'application/json');
        myFormData.append('image', element.picture);
        myFormData.append('id', this.ExerciseId.toString());
        /* Image Post Request */
        this.http.post('https://fitnessappapi2023.azurewebsites.net/api/FitnessExercise/SavePicture', myFormData, {
          headers: headers
        }).subscribe(data => {
          //Check success message
          console.log(data);
        });
      }
    });
    this.filedata = undefined;
    this.fitnessExerciseLocalListWithPicture = [];
  }

  open(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
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
