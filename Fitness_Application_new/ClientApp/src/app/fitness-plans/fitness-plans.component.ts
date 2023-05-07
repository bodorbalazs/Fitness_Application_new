import { Component, OnInit } from '@angular/core';
import { FitnessPlan,RatingClient,RatingDto, FitnessPlanClient ,FavouriteItemClient, FavouriteItemDto, FitnessPlanDto} from '../clientservice/api.client';
import { Router, ParamMap } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

interface FitnessWithRating{
  fitnessPlan: FitnessPlanDto;
  avgRating:number;
}
@Component({
  selector: 'app-fitness-plans',
  templateUrl: './fitness-plans.component.html',
  styleUrls: ['./fitness-plans.component.css']
})
export class FitnessPlansComponent implements OnInit {
  public fitnessPlanList: FitnessPlanDto[] =[];
  public favouriteList: FavouriteItemDto[]=[];
  public fitnessWithRatingList: FitnessWithRating[]=[];
  
  constructor(private fitnessPlanService: FitnessPlanClient,
    private favouriteService:FavouriteItemClient,
    private router: Router,
    private ratingClient:RatingClient) { }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element =>{
      this.fitnessPlanList = element;
      this.fitnessPlanList.forEach(plan =>{
        this.ratingClient.getSpecificEventAverageRating(plan.id).subscribe(ratings=>{
          this.fitnessWithRatingList.push({
            fitnessPlan:plan ,
            avgRating: ratings
          });
        });
      });
    });
    this.favouriteService.getUsersPlans().subscribe(element => this.favouriteList=element);
  }
  
  onSelectPlan(id:number){
    this.router.navigate(['/plan-details',id])
  }
  FavouritePressed(id: number){
    var exists=false;
    this.favouriteList.forEach(element => {
      if(element.fitnessPlanId == id){
        this.favouriteService.delete(element.id).subscribe();
        exists=true;
      }
    });
    if(!exists){
      this.favouriteService.addFavouriteItem(
        new FavouriteItemDto({
            id:0,
            fitnessPlanId:id
        })).subscribe();
    }
    
    this.favouriteService.getUsersPlans().subscribe(element => this.favouriteList=element);
  }
  favouriteExists(id:number):boolean{
    var returnval=false;
    this.favouriteList.forEach(element => {
      if(element.fitnessPlanId == id){
        this.favouriteService.delete(element.id).subscribe();
        returnval=true;
      }
    });
    return returnval;
  }

}
