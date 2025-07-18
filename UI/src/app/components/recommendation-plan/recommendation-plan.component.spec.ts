import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendationPlanComponent } from './recommendation-plan.component';

describe('RecommendationPlanComponent', () => {
  let component: RecommendationPlanComponent;
  let fixture: ComponentFixture<RecommendationPlanComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RecommendationPlanComponent]
    });
    fixture = TestBed.createComponent(RecommendationPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
