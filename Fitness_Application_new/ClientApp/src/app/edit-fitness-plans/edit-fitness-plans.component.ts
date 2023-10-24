import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto, FavouriteItemClient, FavouriteItemDto, FitnessPlanClient, RatingClient, RatingDto, FitnessPlanDto } from '../clientservice/api.client';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Router, ParamMap } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit-fitness-plans',
  templateUrl: './edit-fitness-plans.component.html',
  styleUrls: ['./edit-fitness-plans.component.css']
})
export class EditFitnessPlansComponent implements OnInit {


  public addFitnessPlanForm: FormGroup;
  public addFitnessExerciseForm: FormGroup;
  public fitnessPlanList: FitnessPlanDto[] = [];
  public fitnessPlanEdited: FitnessPlanDto = new FitnessPlanDto;
  public fitnessExerciseList: FitnessExerciseDto[] = [];
  public ratingList: RatingDto[] = [];
  favouriteList: FavouriteItemDto[] = [];
  closeResult: string = "";
  PlanName: any;
  PlanDescription: any;
  PlanId: any;
  wait: number = 0;

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private router: Router,
    private ratingService: RatingClient,
    private favouriteService: FavouriteItemClient,
    private _snackBar: MatSnackBar
  ) {
    this.addFitnessPlanForm = this.formBuilder.group({
      name: '',
      description: ''
    })
    this.addFitnessExerciseForm = this.formBuilder.group({
      name: '',
      description: '',
      pictureUrl: '',
      difficulty: ''
    })

  }
  ngOnInit(): void {
    this.fetchData();
    this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element);
    this.ratingService.getAll().subscribe(rating => this.ratingList = rating);

  }

  onSelectToEdit(id: number) {
    this.router.navigate(['/plan-edit', id])
  }
  deleteFitnessPlan(deleteId: number) {
    var fitnessPlanIndex = this.fitnessPlanList.findIndex(item => item.id = deleteId);
    var fitnessPlanName = this.fitnessPlanList[fitnessPlanIndex].name;
    if (confirm("Are you sure you want to delete fitness plan " + fitnessPlanName + " ?")) {
      this.fitnessPlanService.delete(deleteId).subscribe(
        //this.fetchData
        );
      this.fitnessPlanList = this.fitnessPlanList.filter(item => item.id != this.fitnessPlanList[fitnessPlanIndex].id);
      
      this.openSnackBar("Fitness plan deleted", "dismiss");
    }
  }

  fetchData() {
    this.fitnessPlanService.getUsersPlans().subscribe(
      element => {
        this.fitnessPlanList = element
      });
  }
  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      verticalPosition: 'bottom', horizontalPosition: 'center',
      duration: 2000,
      panelClass: ['blue-snackbar']
    });
  }
}
