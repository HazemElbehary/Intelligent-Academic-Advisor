import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router }    from '@angular/router';
import { CourseService }     from '../../services/course.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ReturnedCourse }    from 'src/app/interfaces/course/ReturnedCourse';
import { MessageService }     from 'primeng/api';

@Component({
  selector: 'app-register-page2',
  templateUrl: './register-page2.component.html',
  styleUrls: ['./register-page2.component.css']
})
export class RegisterPage2Component implements OnInit {
  courses: ReturnedCourse[] = [];
  selectedCodes: string[] = [];       // <-- holds all checked course codes
  selectedCoursesGrades: { [code: string]: string } = {}; // <-- map course code to grade
  studentId!: number;
  searchTerm: string = '';
  isSubmitting = false;
  gradeOptions = [
    { label: 'A+', value: 'A_plus' },
    { label: 'A', value: 'A' },
    { label: 'B+', value: 'B_plus' },
    { label: 'A-', value: 'A_minus' },
    { label: 'B', value: 'B' },
    { label: 'B-', value: 'B_minus' },
    { label: 'C+', value: 'C_plus' },
    { label: 'C', value: 'C' },
    { label: 'C-', value: 'C_minus' },
    { label: 'D+', value: 'D_plus' },
    { label: 'D', value: 'D' },
    { label: 'D-', value: 'D_minus' },
    { label: 'F', value: 'F' }
  ];
  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit() {
    console.log("RegisterPage2Component initialized");
    this.route.queryParams.subscribe(params => {
      const sId = params['studentId'];
      if (sId != null) {
        this.studentId = +sId;
        this.loadCourses();
      }
    });
  }

  private loadCourses() {
    this.courseService.getByStudentId()
      .subscribe({
        next: (response: HttpResponse<ReturnedCourse[]>) => {
          this.courses = (response.body || []).sort((a, b) => a.Name.localeCompare(b.Name));
          this.selectedCodes = []; // Ensure no courses are checked by default
        },
        error: (err: HttpErrorResponse) => {
          console.error('Failed loading courses', err);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not load courses.' });
        }
      });
  }

  get filteredCourses(): ReturnedCourse[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) return this.courses;
    return this.courses.filter(course =>
      course.Name.toLowerCase().includes(term) ||
      course.Code.toLowerCase().includes(term)
    );
  }

  onCourseChecked(event: Event, code: string) {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked && !this.selectedCodes.includes(code)) {
      this.selectedCodes.push(code);
    } else if (!checked) {
      this.selectedCodes = this.selectedCodes.filter(c => c !== code);
      // Also clear grade if course is unchecked
      delete this.selectedCoursesGrades[code];
    }
  }
  onSubmit() {
    this.isSubmitting = true;
    console.log("Submitting selected courses:", this.selectedCodes, this.selectedCoursesGrades);
    if (this.selectedCodes.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'No Selection', detail: 'Please select at least one course.' });
      this.isSubmitting = false;
      return;
    }

    // Check if any selected course is missing a grade
    for (let i = 0; i < this.courses.length; i++) {
      const code = this.courses[i].Code;
      if (this.selectedCodes.includes(code) && !this.selectedCoursesGrades[code]) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Missing Grade',
          detail: `Please select the grade of course ${code}.`
        });
        this.isSubmitting = false;
        return;
      }
    }

    // Only include grades for selected courses
    const selectedGrades = this.courses
      .map((course) => ({
        code: course.Code,
        grade: this.selectedCoursesGrades[course.Code]
      }))
      .filter(item => this.selectedCodes.includes(item.code))
      .map(item => item.grade);

    this.courseService
      .submitCompletedCourses(this.selectedCodes, selectedGrades)
      .subscribe({
        next: () => {
          // Clear recommendation plan cache since courses were updated
          this.clearRecommendationPlanCache();
          this.router.navigate(['home']);
          this.messageService.add({ severity: 'success', summary: 'Submitted', detail: 'Your courses have been recorded.' });
          this.isSubmitting = false;
        },
        error: (err: HttpErrorResponse) => {
          console.error('Submission failed', err);
          let errorMessage = 'Failed to submit courses.';
          
          try {
            // Parse the error response if it's a JSON string
            const errorData = typeof err.error === 'string' ? JSON.parse(err.error) : err.error;
            errorMessage = errorData?.Message || errorMessage;
          } catch (parseError) {
            console.error('Error parsing error response:', parseError);
          }
          
          this.messageService.add({ severity: 'error', summary: 'Submit Error', detail: errorMessage });
          this.isSubmitting = false;
        }
      });
  }

  private clearRecommendationPlanCache(): void {
    try {
      localStorage.removeItem('recommendation_plan_cache');
      console.log('Recommendation plan cache cleared after course submission');
    } catch (error) {
      console.error('Error clearing recommendation plan cache:', error);
    }
  }

  skipNow() {
    this.router.navigate(['home']);
  }

  goToHome() {
    this.router.navigate(['home']);
  }
}
