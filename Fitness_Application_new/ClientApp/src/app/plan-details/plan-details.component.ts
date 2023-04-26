import { Component,ViewChild } from '@angular/core';
import { FavouriteItemClient, FavouriteItemDto, FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto, RatingClient ,RatingDto} from '../clientservice/api.client';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import {MatAccordion} from '@angular/material/expansion';
import { AUTO_STYLE,trigger, state, style, transition, animate } from '@angular/animations';
import { elementAt } from 'rxjs';
import { NgbRatingModule } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-plan-details',
  templateUrl: './plan-details.component.html',
  styleUrls: ['./plan-details.component.css'],
})
export class PlanDetailsComponent {

  id!:number;
  btnVal:string ='Favourite';
  currentRate=0;
  specifiedPlan! : FitnessPlanDto;
  specifiedFavourite: FavouriteItemDto | undefined;
  fitnessExercises: FitnessExerciseDto[]=[];
  specifiedFitnessExercises: FitnessExerciseDto[]=[];
  favouriteList: FavouriteItemDto[]=[];
  panelOpenState= false;
  @ViewChild(MatAccordion) accordion!: MatAccordion;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService : FitnessPlanClient,
    private router: Router,
    private FitnessExerciseService: FitnessExerciseClient,
    private favouriteService:FavouriteItemClient,
    private ratingService:RatingClient)
  { }

  ngOnInit():void{
    this.id = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
    this.FitnessPlanService.get(this.id).subscribe(element => this.specifiedPlan = element);
    this.FitnessExerciseService.getAll().subscribe(element =>
      {
        this.fitnessExercises= element;
        this.fitnessExercises.forEach(exercise => {
          if(exercise.fitnessPlanId== this.id){
            this.specifiedFitnessExercises.push(exercise);
          }
        });
      });
    this.favouriteService.getAll().subscribe(element =>
    {
       this.favouriteList= element;
       this.favouriteList.forEach(favourite =>{
      if(favourite.fitnessPlanId==this.id){
        this.specifiedFavourite=favourite;
        this.btnVal='Unfavourite';
      }
       })
    
    }
    ); 
    
  }
  onRateChange(rating: number){
      
  }

  onFavouritePlan(){
    console.log('number:',this.favouriteList.length);
    if(this.btnVal=='Favourite'){
      this.favouriteService.addFavouriteItem(
      new FavouriteItemDto({
          id:0,
          fitnessPlanId:this.id
      })).subscribe();
      this.btnVal='Unfavourite'
    }else{
      this.btnVal='Favourite';
      if(this.specifiedFavourite!=undefined){
      this.favouriteService.delete(this.specifiedFavourite.id).subscribe();}

    }
    
  }
}
