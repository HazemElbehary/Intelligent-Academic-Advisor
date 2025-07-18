import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { filter, take } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private authService: AuthService, 
    private router: Router,
    private messageService: MessageService) 
  {
      // Subscribe to router events to fetch GPA on navigation to this component
    this.router.events
      .pipe(
        // 1) type‑guard so TS knows this event is NavigationEnd
        filter((e): e is NavigationEnd => e instanceof NavigationEnd),

        // 2) only when URL is exactly '/home'
        filter(e => e.urlAfterRedirects === '/home'),

        // 3) only handle the first matching navigation
        take(1)
      )
      .subscribe(() => this.getGPA());
  }
  
    gpa: number | undefined;

    private getGPA() {
      this.authService.getGPA().pipe(take(1))
        .subscribe({
          next: (value: number) => {
            this.gpa = value;
          },
          error: (err: HttpErrorResponse) => {
            let detail = 'An error occurred while fetching GPA.';
            if (err.error && err.error.Message) {
              detail = err.error.Message;
            }
            this.messageService.add({ severity: 'error', summary: 'Error', detail });
          }
        });
    }

  logOut() {
    this.authService.logOut();
  }

  navigateToAddCourse() {
    const FCAIID = this.authService.getFCAIIDFromToken();
    console.log("FCAIID from token: ", FCAIID);
    if (FCAIID) {
      this.router.navigate(['register-page2'], {
        queryParams: { studentId: FCAIID }
      });
    }
  }
}
