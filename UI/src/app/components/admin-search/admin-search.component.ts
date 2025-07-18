import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-admin-search',
  templateUrl: './admin-search.component.html',
  styleUrls: ['./admin-search.component.css']
})
export class AdminSearchComponent {
  studentId: string = '';
  recommendationPlan: any = null;
  loading: boolean = false;
  showResults: boolean = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  searchRecommendation() {
    if (!this.studentId.trim()) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Please enter a Student ID'
      });
      return;
    }

    const studentIdNum = parseInt(this.studentId);
    if (isNaN(studentIdNum)) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please enter a valid Student ID (numbers only)'
      });
      return;
    }

    this.loading = true;
    this.recommendationPlan = null;
    this.showResults = false;

    this.authService.getStudentRecommendationPlan(studentIdNum).subscribe({
      next: (response) => {
        this.loading = false;
        this.recommendationPlan = response;
        this.showResults = true;
        console.log('Recommendation Plan:', response);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Recommendation plan loaded successfully'
        });
      },
      error: (error) => {
        this.loading = false;
        console.error('Error fetching recommendation plan:', error);
        let errorMessage = 'Failed to fetch recommendation plan';
        if (error.error && error.error.message) {
          errorMessage = error.error.message;
        } else if (error.status === 404) {
          errorMessage = 'Student not found or no recommendation plan available';
        } else if (error.status === 403) {
          errorMessage = 'Access denied. Only admins can access this feature';
        }
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage
        });
      }
    });
  }

  logout() {
    this.authService.logOut();
  }

  getKeys(obj: any): string[] {
    return obj ? Object.keys(obj) : [];
  }
} 