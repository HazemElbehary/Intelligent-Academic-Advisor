import { Component, Input, OnInit } from '@angular/core';
import { CourseService } from 'src/app/services/course.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-recommendation-plan',
  templateUrl: './recommendation-plan.component.html',
  styleUrls: ['./recommendation-plan.component.css']
})
export class RecommendationPlanComponent implements OnInit {
  @Input() recommendationData: any = null;
  recommendationPlan: any;
  loading = false;
  private readonly CACHE_KEY = 'recommendation_plan_cache';

  constructor(private courseService: CourseService, private messageService: MessageService) {}

  ngOnInit(): void {
    if (this.recommendationData) {
      // Use the provided data
      this.recommendationPlan = this.recommendationData;
    } else {
      // Load data from service (original behavior)
      this.loadRecommendationPlan();
    }
  }

  loadRecommendationPlan(): void {
    // Check if we have valid cached data
    const cachedData = this.getCachedRecommendationPlan();
    if (cachedData) {
      this.recommendationPlan = cachedData;
      this.loading = false;
      console.log('Loaded recommendation plan from cache');
      return;
    }

    // If no valid cache, load from server
    this.loading = true;
    this.courseService.getRecommendationPlan().subscribe({
      next: response => {
        this.recommendationPlan = response;
        this.loading = false;
        this.cacheRecommendationPlan(response);
        console.log('Loaded Recommendation Plan from server:', response);
      },
      error: err => {
        this.loading = false;
        console.error('Failed to load recommendation plan:', err);
        let detail = 'Failed to load recommendation plan.';
        if (err.error && err.error.Message) {
          detail = err.error.Message;
        } else if (typeof err.error === 'string') {
          detail = err.error;
        }
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail
        });
      }
    });
  }

  private getCachedRecommendationPlan(): any {
    try {
      const cachedData = localStorage.getItem(this.CACHE_KEY);
      
      if (!cachedData) {
        return null;
      }

      return JSON.parse(cachedData);
    } catch (error) {
      console.error('Error reading cached recommendation plan:', error);
      this.clearCache();
      return null;
    }
  }

  private cacheRecommendationPlan(data: any): void {
    try {
      localStorage.setItem(this.CACHE_KEY, JSON.stringify(data));
      console.log('Recommendation plan cached successfully');
    } catch (error) {
      console.error('Error caching recommendation plan:', error);
    }
  }

  private clearCache(): void {
    try {
      localStorage.removeItem(this.CACHE_KEY);
      console.log('Recommendation plan cache cleared');
    } catch (error) {
      console.error('Error clearing cache:', error);
    }
  }

  // Method to be called when courses are updated
  refreshRecommendationPlan(): void {
    this.clearCache();
    this.loadRecommendationPlan();
  }

  getKeys(obj: any): string[] {
    return obj ? Object.keys(obj) : [];
  }
}
