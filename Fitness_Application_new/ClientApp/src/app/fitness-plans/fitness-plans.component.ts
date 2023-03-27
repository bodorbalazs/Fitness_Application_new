import { Component, OnInit } from '@angular/core';
import { FitnessPlan, FitnessPlanClient } from '../clientservice/api.client';

@Component({
  selector: 'app-fitness-plans',
  templateUrl: './fitness-plans.component.html',
  styleUrls: ['./fitness-plans.component.css']
})
export class FitnessPlansComponent implements OnInit {
  public fitnessPlanList: FitnessPlan[] =[];
  
  constructor(private fitnessPlanService: FitnessPlanClient) { }

  ngOnInit(): void {
    this.fitnessPlanService.getAll().subscribe(element => this.fitnessPlanList = element)
  }
  

}
