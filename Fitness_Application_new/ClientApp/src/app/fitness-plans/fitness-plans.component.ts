import { Component, OnInit } from '@angular/core';
import { FitnessPlan, RatingClient, RatingDto, FitnessPlanClient, FavouriteItemClient, FavouriteItemDto, FitnessPlanDto } from '../clientservice/api.client';
import { Router, ParamMap } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
interface FitnessWithRating {
  fitnessPlan: FitnessPlanDto;
  avgRating: number;
}
@Component({
  selector: 'app-fitness-plans',
  templateUrl: './fitness-plans.component.html',
  styleUrls: ['./fitness-plans.component.css']
})
export class FitnessPlansComponent implements OnInit {
  public fitnessPlanList: FitnessPlanDto[] = [];
  public favouriteList: FavouriteItemDto[] = [];
  public fitnessWithRatingList: FitnessWithRating[] = [];

  constructor(private fitnessPlanService: FitnessPlanClient,
    private favouriteService: FavouriteItemClient,
    private authorizeService: AuthorizeService,
    private router: Router,
    private ratingClient: RatingClient) { }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element => {
      this.fitnessPlanList = element;
      this.fitnessPlanList.forEach(plan => {
        this.ratingClient.getSpecificEventAverageRating(plan.id).subscribe(ratings => {
          this.fitnessWithRatingList.push({
            fitnessPlan: plan,
            avgRating: ratings
          });
        });
      });
    });
  }

  onSelectPlan(id: number) {
    if(this.authorizeService.isAuthenticated()){
      this.router.navigate(['/plan-details', id])
    }
    else{
      this.router.navigate(["/authentication/login"])
    }
  }

}
