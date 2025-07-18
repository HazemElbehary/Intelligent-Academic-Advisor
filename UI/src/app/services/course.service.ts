import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReturnedCourse } from '../interfaces/course/ReturnedCourse';
import { Observable } from 'rxjs';
import { RecommendationResponse } from '../interfaces/course/RecommendationResponse ';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  // private baseUrl = 'https://intelligentacademicadvisor.somee.com/api';
  private baseUrl = 'https://localhost:7087/api';


  constructor(private http: HttpClient) { }

  getRecommendationPlan(): Observable<RecommendationResponse> {
    const token = localStorage.getItem('token')!;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<RecommendationResponse>(
      `${this.baseUrl}/students/me/recommended-plan`,
      { headers }
    );
  }

  getByStudentId(): Observable<HttpResponse<ReturnedCourse[]>> {
    console.log("Fetching courses for Student ID from token");
    const token = localStorage.getItem('token')!;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    return this.http.get<ReturnedCourse[]>(
      `${this.baseUrl}/students/me/available-courses`, {
        headers,
        observe: 'response'
      }
    );
  }

  submitCompletedCourses(CoursesCodes: string[], CourseGrades: string[]): Observable<any> {
      console.log("Submitting completed courses from submitCompletedCourses:", CoursesCodes, CourseGrades);
      const token = localStorage.getItem('token')!;
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      const body = { CoursesCodes, CourseGrades };
      return this.http.post(`${this.baseUrl}/students/me/courses`, body, {
        headers,
        responseType: 'text'
      });
  }
}
