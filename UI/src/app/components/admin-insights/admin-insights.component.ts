import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';

interface CourseWithFailsDto {
  CourseName: string;
  NumberOfFails: number;
}

@Component({
  selector: 'app-admin-insights',
  templateUrl: './admin-insights.component.html',
  styleUrls: ['./admin-insights.component.css']
})
export class AdminInsightsComponent implements OnInit {
  coursesWithFails: CourseWithFailsDto[] = [];
  loading = false;

  constructor(private authService: AuthService, private messageService: MessageService) {}

  ngOnInit() {
    this.fetchInsights();
  }

  fetchInsights() {
    this.loading = true;
    this.authService.getCoursesWithFails().subscribe({
      next: (data) => {
        this.coursesWithFails = data;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load insights.' });
      }
    });
  }
} 