import { Component } from '@angular/core';
import { FavouriteItemClient, FavouriteItemDto, FitnessPlan, FitnessPlanClient, FitnessPlanDto } from '../clientservice/api.client';
import { Router } from '@angular/router';

@Component({
  selector: 'app-favourite-plans',
  templateUrl: './favourite-plans.component.html',
  styleUrls: ['./favourite-plans.component.css']
})
export class FavouritePlansComponent {
  public favouriteList: FavouriteItemDto[]=[];
  public fitnessPlanList: FitnessPlanDto[] =[];
  public favouritePlanList: FitnessPlanDto[]=[];

  constructor(private fitnessPlanService: FitnessPlanClient,
    private favouriteService:FavouriteItemClient,
    private router: Router) { }

  ngOnInit(): void{
    this.favouriteService.getUsersPlans().subscribe(element => 
      {
        this.favouriteList=element
        this.favouriteList.forEach(favourite =>{
          if(favourite.fitnessPlanId != undefined){
          this.fitnessPlanService.get(favourite.fitnessPlanId).subscribe(plan => this.favouritePlanList.push(plan));
          }
        })
      
      });
    
  }
  onSelectPlan(id:number){
    this.router.navigate(['/plan-details',id]);
  }
}
