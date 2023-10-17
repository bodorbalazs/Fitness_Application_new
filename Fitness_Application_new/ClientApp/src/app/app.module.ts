import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { FitnessPlansComponent } from './fitness-plans/fitness-plans.component';
import { ScrollingModule } from '@angular/cdk/scrolling';

import { MatExpansionModule } from '@angular/material/expansion';
import { MatCardModule } from '@angular/material/card';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon'
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { FavouriteItemClient, RatingClient, FitnessExerciseClient, FitnessPlanClient } from './clientservice/api.client';
import { CreateFitnessPlanComponent } from './create-fitness-plan/create-fitness-plan.component';
import { EditFitnessPlansComponent } from './edit-fitness-plans/edit-fitness-plans.component';
import { PlanEditComponent } from './plan-edit/plan-edit.component';
import { PlanDetailsComponent } from './plan-details/plan-details.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FavouritePlansComponent } from './favourite-plans/favourite-plans.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FitnessPlansComponent,
    CreateFitnessPlanComponent,
    EditFitnessPlansComponent,
    PlanEditComponent,
    PlanDetailsComponent,
    FavouritePlansComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ScrollingModule,
    MatExpansionModule,
    MatCardModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ApiAuthorizationModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: 'fitness-plans', component: FitnessPlansComponent },
      { path: 'create-fitness-plan', component: CreateFitnessPlanComponent, canActivate: [AuthorizeGuard] },
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'edit-fitness-plans', component: EditFitnessPlansComponent, canActivate: [AuthorizeGuard] },
      { path: 'plan-edit/:id', component: PlanEditComponent, canActivate: [AuthorizeGuard] },
      { path: 'plan-details/:id', component: PlanDetailsComponent, },
      { path: 'favourite-plans', component: FavouritePlansComponent, canActivate: [AuthorizeGuard] }
    ]),
    NgbModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    FavouriteItemClient,
    RatingClient,
    FitnessExerciseClient,
    FitnessPlanClient
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
