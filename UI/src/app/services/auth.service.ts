import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterStudent } from '../interfaces/student/registerStudent';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { ReturnedStudent } from '../interfaces/student/returnedStudent';
import { LoginStudent } from '../interfaces/student/loginStudent';
import { ReturnedUniversity } from '../interfaces/university/ReturnedUniversity';
import { ReturnedDepartment } from '../interfaces/department/ReturnedDepartment';
import { AdminDto, LoginDto } from '../interfaces/admin/admin';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ReturnedTerm } from '../interfaces/course/ReturnedTerm';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // private baseUrl = 'https://intelligentacademicadvisor.somee.com/api';
  private baseUrl = 'https://localhost:7087/api';


  constructor(private http: HttpClient, 
    private router: Router, 
    private messageService: MessageService) { }

  get token(): string | null {
    return localStorage.getItem('token');
  }

  
  registerUser(userDetails: RegisterStudent): Observable<ReturnedStudent> {
    return this.http.post<any>(`${this.baseUrl}/auth/register`, userDetails);
  }

  getUniversityOptions(): Observable<HttpResponse<ReturnedUniversity[]>> {
    return this.http.get<ReturnedUniversity[]>(`${this.baseUrl}/universities`, { observe: 'response' });
  }

  getDepartmentOptions(): Observable<HttpResponse<ReturnedDepartment[]>> {
    return this.http.get<ReturnedDepartment[]>(`${this.baseUrl}/departments`, { observe: 'response' });
  }
  
  loginUser(loginUser: LoginStudent): Observable<HttpResponse<ReturnedStudent>>{
    return this.http.post<any>(`${this.baseUrl}/auth/login`, loginUser, { observe: 'response' });
  }

  loginAdmin(loginAdmin: LoginDto): Observable<HttpResponse<AdminDto>>{
    return this.http.post<any>(`${this.baseUrl}/admin/login`, loginAdmin, { observe: 'response' });
  }

  getStudentRecommendationPlan(studentId: number): Observable<any> {
    const token = this.token;
    if (!token) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please login.',
      });
      return of(null);
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(`${this.baseUrl}/admin/students/${studentId}/recommendation-plan`, { headers });
  }

  isTokenExpired(token: string): boolean {
    if (!token || token.split('.').length !== 3) return true;
    const payload = JSON.parse(atob(token.split('.')[1]));
    console.log(`isTokenExpired: ${Date.now() >= payload.exp * 1000}`);
    return Date.now() >= payload.exp * 1000;
  }

  isAuthenticated(): boolean {
    const token = this.token;
    if (!token) return false;
    return !this.isTokenExpired(token);
  }

  logOut() {
    console.log("Logging out...");
    localStorage.removeItem('token');
    localStorage.removeItem('userRole');
    localStorage.removeItem('adminInfo');
    localStorage.removeItem('recommendation_plan_cache');
    this.router.navigate(['login']);
  }

  getFCAIIDFromToken(): string | null {
    try {
      const base64 = this.token!.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      const payload = JSON.parse(jsonPayload);
      return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"];
    } catch (e) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error decoding token',
        detail: 'Failed to decode token. Please try again later.',
      });
      return null;
    }
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'admin';
  }

  isUser(): boolean {
    return this.getUserRole() === 'user';
  }

  getAdminInfo(): any {
    const adminInfo = localStorage.getItem('adminInfo');
    return adminInfo ? JSON.parse(adminInfo) : null;
  }

  getGPA(): Observable<number> {
    const token = this.token;
    if (!token) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error decoding token',
        detail: 'Please login.',
      });
      return of(0);
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<number>(`${this.baseUrl}/students/me/gpa`, { headers });
  }

  getCoursesWithFails() {
    const token = this.token;
    const headers = token ? { headers: { Authorization: `Bearer ${token}` } } : {};
    return this.http.get<any[]>(`${this.baseUrl}/admin/courses-with-fails`, headers);
  }

  addDepartmentToStudent(departmentId: number): Observable<any> {
    console.log(departmentId);
    const token = this.token;
    if (!token) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please login.',
      });
      return of(null);
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post(`${this.baseUrl}/students/department?departmentId=${departmentId}`, null, { headers, responseType: 'text' });
  }

  getTerms(): Observable<ReturnedTerm[]> {
    const token = this.token;
    if (!token) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please login.',
      });
      return of([]);
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<ReturnedTerm[]>(`${this.baseUrl}/terms`, { headers }).pipe(
      tap(response => {
        console.log('getTerms response:', response);
      })
    );
  }

  updateStudentTerm(termId: number): Observable<any> {
    const token = this.token;
    if (!token) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please login.',
      });
      return of(null);
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.patch(`${this.baseUrl}/students/term?termId=${termId}`, null, { headers, responseType: 'text' });
  }
}