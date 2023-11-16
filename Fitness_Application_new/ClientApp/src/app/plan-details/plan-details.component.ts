import { Component, ViewChild } from '@angular/core';
import { FavouriteItemClient, FavouriteItemDto, FitnessExerciseClient, FitnessExerciseDto, FitnessPlanClient, FitnessPlanDto, RatingClient, RatingDto } from '../clientservice/api.client';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatAccordion } from '@angular/material/expansion';
import { AUTO_STYLE, trigger, state, style, transition, animate } from '@angular/animations';
import { elementAt } from 'rxjs';
import { NgbRatingModule } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-plan-details',
  templateUrl: './plan-details.component.html',
  styleUrls: ['./plan-details.component.css'],
})
export class PlanDetailsComponent {

  eventId!: number;
  btnVal: string = 'Favourite';
  currentRate = 0;
  specifiedPlan: FitnessPlanDto = new FitnessPlanDto;
  specifiedFavourite: FavouriteItemDto | undefined;
  fitnessExercises: FitnessExerciseDto[] = [];
  specifiedFitnessExercises: FitnessExerciseDto[] = [];
  favouriteList: FavouriteItemDto[] = [];
  specifiedRating!: RatingDto;
  exists: boolean = false;

  panelOpenState = false;
  @ViewChild(MatAccordion) accordion!: MatAccordion;
  constructor(private route: ActivatedRoute,
    private FitnessPlanService: FitnessPlanClient,
    private router: Router,
    private FitnessExerciseService: FitnessExerciseClient,
    private favouriteService: FavouriteItemClient,
    private ratingService: RatingClient) { }

  ngOnInit(): void {
    this.eventId = parseInt(this.route.snapshot.paramMap.get('id') || '{}');
    this.FitnessPlanService.get(this.eventId).subscribe(element => this.specifiedPlan = element);
    this.FitnessExerciseService.getAll().subscribe(element => {
      this.fitnessExercises = element;
      this.fitnessExercises.forEach(exercise => {
        if (exercise.fitnessPlanId == this.eventId) {
          this.specifiedFitnessExercises.push(exercise);
        }
      });
    });
    this.favouriteService.getUsersPlans().subscribe(element => {
      this.favouriteList = element;
      this.favouriteList.forEach(favourite => {
        if (favourite.fitnessPlanId == this.eventId) {
          this.specifiedFavourite = favourite;
          this.btnVal = 'Unfavourite';
        }
      })

    }
    );
    this.ratingService.getSpecificEventRating(this.eventId).subscribe(element => {
      this.specifiedRating = element;
      this.currentRate = this.specifiedRating.value;
    });
  }
  onRateChange(rating: number) {
    if (this.specifiedRating.id == 0 && !this.exists) {
      this.currentRate = rating;
      this.ratingService.addRating(
        new RatingDto({
          id: 0,
          fitnessPlanId: this.eventId,
          value: rating
        })
      ).subscribe();
      this.exists = true;
    } else {
      console.log(this.specifiedRating.id)
      console.log(this.exists)
      this.currentRate = rating;
      this.ratingService.getSpecificEventRating(this.eventId).subscribe(
        element => {
          this.ratingService.put(element.id, new RatingDto({
            id: element.id,
            fitnessPlanId: this.eventId,
            value: rating
          })

          ).subscribe();
        }
      )
    }
  }

  onFavouritePlan() {
    console.log('number:', this.favouriteList.length);
    if (this.btnVal == 'Favourite') {
      this.favouriteService.addFavouriteItem(
        new FavouriteItemDto({
          id: 0,
          fitnessPlanId: this.eventId
        })).subscribe();
      this.btnVal = 'Unfavourite'
    } else {
      this.btnVal = 'Favourite';
      if (this.specifiedFavourite != undefined) {
        this.favouriteService.delete(this.specifiedFavourite.id).subscribe();
      }
    }
  }
}
