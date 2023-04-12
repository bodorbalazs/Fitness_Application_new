import { Component, OnInit } from '@angular/core';
import { FitnessPlan, FitnessPlanClient } from '../clientservice/api.client';
import { Router, ParamMap } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fitness-plans',
  templateUrl: './fitness-plans.component.html',
  styleUrls: ['./fitness-plans.component.css']
})
export class FitnessPlansComponent implements OnInit {
  public fitnessPlanList: FitnessPlan[] =[];
  
  constructor(private fitnessPlanService: FitnessPlanClient,
    private router: Router) { }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element => this.fitnessPlanList = element)
  }
  
  onSelectPlan(id:number){
    this.router.navigate(['/plan-details',id])
  }

}
