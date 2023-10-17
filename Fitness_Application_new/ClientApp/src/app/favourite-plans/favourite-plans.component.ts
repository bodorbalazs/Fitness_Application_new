import { Component } from '@angular/core';
import { FavouriteItemClient, FavouriteItemDto, FitnessPlan, FitnessPlanClient, FitnessPlanDto, RatingClient } from '../clientservice/api.client';
import { Router } from '@angular/router';

interface FitnessWithRating {
  fitnessPlan: FitnessPlanDto;
  avgRating: number;
}
@Component({
  selector: 'app-favourite-plans',
  templateUrl: './favourite-plans.component.html',
  styleUrls: ['./favourite-plans.component.css']
})
export class FavouritePlansComponent {
  public favouriteList: FavouriteItemDto[] = [];
  public fitnessPlanList: FitnessPlanDto[] = [];
  public favouritePlanList: FitnessPlanDto[] = [];
  public fitnessWithRatingList: FitnessWithRating[] = [];

  constructor(private fitnessPlanService: FitnessPlanClient,
    private favouriteService: FavouriteItemClient,
    private router: Router,
    private ratingClient: RatingClient) { }

  ngOnInit(): void {
    this.favouriteService.getUsersPlans().subscribe(element => {
      this.favouriteList = element
      this.favouriteList.forEach(favourite => {
        if (favourite.fitnessPlanId != undefined) {
          /*this.ratingClient.getSpecificEventAverageRating(favourite.fitnessPlanId).subscribe(ratings=>{
            this.fitnessWithRatingList.push({
              this.fitnessPlanService.get(favourite.fitnessPlanId).subscribe(plan => this.favouritePlanList.push(plan));
            })
          })*/
          this.fitnessPlanService.get(favourite.fitnessPlanId).subscribe(plan => this.favouritePlanList.push(plan));
        }
      });
    });
  }
  onSelectPlan(id: number) {
    this.router.navigate(['/plan-details', id]);
  }
}
