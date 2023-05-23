import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FitnessExerciseClient, FitnessExerciseDto,FavouriteItemClient,FavouriteItemDto, FitnessPlanClient,RatingClient,RatingDto, FitnessPlanDto } from '../clientservice/api.client';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Router, ParamMap } from '@angular/router';

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
  public ratingList: RatingDto[]=[];
  favouriteList: FavouriteItemDto[]=[];
  closeResult:string ="";
  PlanName: any;
  PlanDescription: any;
  PlanId: any;
  wait:number=0;

  constructor(private formBuilder: FormBuilder,
    private fitnessPlanService: FitnessPlanClient,
    private fitnessExerciseService: FitnessExerciseClient,
    private authorizeService: AuthorizeService,
    private modalService:NgbModal,
    private route: ActivatedRoute,
    private router: Router,
    private ratingService: RatingClient ,
    private favouriteService: FavouriteItemClient) 
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
    ngOnInit(): void {
      this.fetchData();
      this.fitnessExerciseService.getAll().subscribe(element => this.fitnessExerciseList = element);
      this.ratingService.getAll().subscribe(rating=>this.ratingList=rating );
    
    }

    onSelectToEdit(id:number){
      this.router.navigate(['/plan-edit',id])
    }
     deleteFitnessPlan(id:number){
     //this.deleteForeignKeys(id);

      this.fitnessPlanService.delete(id).subscribe();
      this.fetchData();
    }

   deleteForeignKeys(id:number){
      this.fitnessExerciseService.getAll().subscribe(exercises =>{
        this.fitnessExerciseList = exercises;
        this.fitnessExerciseList.forEach(exercise=>{
            if(exercise.fitnessPlanId==id){
            this.fitnessExerciseService.delete(exercise.id).subscribe();
            }
          });
        


        }
        );
        this.ratingService.getAll().subscribe(ratings =>{
          this.ratingList = ratings;
          this.ratingList.forEach(rating=>{
              if(rating.fitnessPlanId==id){
              this.ratingService.delete(rating.id).subscribe();
              }
            });
          });
          this.favouriteService.getAll().subscribe(favourites =>{
            this.favouriteList = favourites;
            this.favouriteList.forEach(favourite=>{
                if(favourite.fitnessPlanId==id){
                this.ratingService.delete(favourite.id).subscribe();
                }
              });
            });
    }

    fetchData() {
      this.fitnessPlanService.getUsersPlans().subscribe(
        element =>{ this.fitnessPlanList = element
      });
  }
}
