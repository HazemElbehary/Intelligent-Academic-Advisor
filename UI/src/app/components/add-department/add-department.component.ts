import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth.service';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-add-department',
  templateUrl: './add-department.component.html',
  styleUrls: ['./add-department.component.css']
})
export class AddDepartmentComponent implements OnInit {
  departmentForm = this.fb.group({
    departmentId: [null, Validators.required]
  });

  departmentOptions: { label: string; value: number }[] = [];
  loading = false;
  private readonly RECOMMENDATION_CACHE_KEY = 'recommendation_plan_cache';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    public router: Router,
    private courseService: CourseService
  ) {}

  ngOnInit() {
    this.loadDepartmentOptions();
  }

  private loadDepartmentOptions() {
    this.authService.getDepartmentOptions().subscribe({
      next: (response) => {
        if (response.body) {
          this.departmentOptions = response.body.map((dept: any) => ({
            label: dept.Name,
            value: dept.Id
          }));
        }
      },
      error: (err) => {
        console.error('Failed to load departments:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load departments'
        });
      }
    });
  }

  onSubmit() {
    if (this.departmentForm.invalid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Validation Error',
        detail: 'Please select a department'
      });
      return;
    }

    this.loading = true;
    const departmentId = this.departmentForm.value.departmentId;

    if (departmentId === null || departmentId === undefined) {
      this.loading = false;
      return;
    }

    this.authService.addDepartmentToStudent(departmentId).subscribe({
      next: (response) => {
        this.loading = false;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: response || 'Department added successfully!'
        });
        // Update recommendation plan in local storage
        this.courseService.getRecommendationPlan().subscribe({
          next: (plan) => {
            localStorage.setItem(this.RECOMMENDATION_CACHE_KEY, JSON.stringify(plan));
            this.router.navigate(['home']);
          },
          error: () => {
            // Even if fetching plan fails, still navigate
            this.router.navigate(['home']);
          }
        });
      },
      error: (err) => {
        this.loading = false;
        console.error('Failed to add department:', err);
        let errorMessage = 'Failed to add department';
        
        try {
          const errorData = typeof err.error === 'string' ? JSON.parse(err.error) : err.error;
          errorMessage = errorData?.Message || errorMessage;
        } catch (parseError) {
          console.error('Error parsing error response:', parseError);
        }
        
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: errorMessage
        });
      }
    });
  }

  get departmentId() {
    return this.departmentForm.controls['departmentId'];
  }
} 