import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ReturnedTerm } from '../../interfaces/course/ReturnedTerm';
import { Router } from '@angular/router';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-update-student-term',
  templateUrl: './update-student-term.component.html',
  styleUrls: ['./update-student-term.component.css']
})
export class UpdateStudentTermComponent implements OnInit {
  terms: ReturnedTerm[] = [];
  selectedTermId: number | null = null;
  loading = false;
  private readonly RECOMMENDATION_CACHE_KEY = 'recommendation_plan_cache';

  constructor(
    private authService: AuthService,
    public router: Router,
    private courseService: CourseService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.authService.getTerms().subscribe({
      next: (terms) => {
        this.terms = terms.map((t: any) => ({
          id: t.Id,
          name: t.Name
        }));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  updateTerm() {
    if (this.selectedTermId) {
      this.loading = true;
      this.authService.updateStudentTerm(this.selectedTermId).subscribe({
        next: (response) => {
          console.log('UpdateTermOfStudent response:', response);
          // Clear the cache so the next load fetches from backend
          localStorage.removeItem(this.RECOMMENDATION_CACHE_KEY);
          this.loading = false;
          this.router.navigate(['/home']);
        },
        error: () => {
          this.loading = false;
        }
      });
    }
  }
}
