import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { FitnessPlansComponent } from './fitness-plans/fitness-plans.component';

//import { MaterialModule } from 'src/material/material.module';
import { FavouriteItemClient,RatingClient,FitnessExerciseClient,FitnessPlanClient } from './clientservice/api.client';
import { CreateFitnessPlanComponent } from './create-fitness-plan/create-fitness-plan.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FitnessPlansComponent,
    CreateFitnessPlanComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  // MaterialModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: 'fitness-plans',component: FitnessPlansComponent},
      { path: 'create-fitness-plan',component:CreateFitnessPlanComponent,canActivate: [AuthorizeGuard]},
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
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
