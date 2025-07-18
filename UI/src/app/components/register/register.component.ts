import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { RegisterStudent } from 'src/app/interfaces/student/registerStudent';
import { AuthService } from 'src/app/services/auth.service';
import { passwordMatchValidator } from 'src/app/shared/password-match.directive';
import { ReturnedUniversity } from 'src/app/interfaces/university/ReturnedUniversity';
import { ReturnedDepartment } from 'src/app/interfaces/department/ReturnedDepartment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {

  // Form step management
  currentStep = 1;
  totalSteps = 2;

  // Reactive form for user registration
  registerForm = this.fb.group({
    userName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9_]+$/)]],
    fcaiid: ['', [Validators.required, Validators.pattern(/^202\d{5}$/)]],
    password: ['', [Validators.required, Validators.minLength(8), this.passwordComplexityValidator]],
    confirmPassword: ['', Validators.required],
    UniversityId: [null, Validators.required],
    DepartmentId: [null],
    UserTerm: [null, Validators.required]
  }, {
    validators: passwordMatchValidator
  });

  // Dropdown options for universities
  UniversityOptions: { label: string; value: any }[] = [];
  DepartmentOptions: { label: string; value: any }[] = [];
  UserTermOptions: { label: string; value: any }[] = [
    { label: 'First Term', value: 1 },
    { label: 'Second Term', value: 2 }
  ];

  universityOptions: any[] = []; // Initialize as an empty array
  departmentOptions: any[] = []; // Initialize as an empty array

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadUniversityOptions();
    this.loadDepartmentOptions();
    // Department should be disabled by default
    this.DepartmentId.disable();
    // Enable/disable DepartmentId based on university selection
    this.UniversityId.valueChanges.subscribe((val) => {
      if (val == 2) {
        this.DepartmentId.enable();
      } else {
        this.DepartmentId.disable();
        this.DepartmentId.setValue(null);
      }
    });
  }

  private loadUniversityOptions() {
    this.authService.getUniversityOptions().subscribe(
      (response: HttpResponse<ReturnedUniversity[]>) => {
        if (response.body) {
          console.log('University options loaded:', response.body);
          this.UniversityOptions = response.body!.map((u: ReturnedUniversity) => ({
            label: (u as ReturnedUniversity).Name,
            value: (u as ReturnedUniversity).ID
          }));
        } else {
          console.warn('Response body is null.');
        }
      },
      error => {
        console.error('Failed to load university options:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load university options.' });
      }
    );
  }

  private loadDepartmentOptions() {
    this.authService.getDepartmentOptions().subscribe(
      (response: HttpResponse<ReturnedDepartment[]>) => {
        console.log('Department options response:', response);
        if (response.body) {
          console.log('Department options loaded:', response.body);
          this.DepartmentOptions = response.body!.map((u: ReturnedDepartment) => ({
            label: (u as ReturnedDepartment).Name,
            value: (u as ReturnedDepartment).Id
          }));
        } else {
          console.warn('Response body is null.');
        }
      },
      error => {
        console.error('Failed to load department options:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load department options.' });
      }
    );
  }

  private passwordComplexityValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) {
      return null; // Let the required validator handle empty values
    }
    const errors: ValidationErrors = {};

    if (!/[A-Z]/.test(value)) {
      errors['uppercase'] = true;
    }
    if (!/[a-z]/.test(value)) {
      errors['lowercase'] = true;
    }
    if (!/[0-9]/.test(value)) {
      errors['digit'] = true;
    }
    if (!/[@$!%*?&]/.test(value)) {
      errors['special'] = true;
    }

    return Object.keys(errors).length > 0 ? errors : null;
  }


  // Getters for form controls
  get userName() {
    return this.registerForm.controls['userName'];
  }

  get fcaiid() {
    return this.registerForm.controls['fcaiid'];
  }
  get password() {
    return this.registerForm.controls['password'];
  }

  get confirmPassword() {
    return this.registerForm.controls['confirmPassword'];
  }

  get UniversityId() {
    return this.registerForm.controls['UniversityId'];
  }

  get DepartmentId() {
    return this.registerForm.controls['DepartmentId'];
  }

  get UserTerm() {
    return this.registerForm.controls['UserTerm'];
  }

  // Method to validate password against a regex pattern
  isPasswordValid(pattern: RegExp, value: string | null): boolean {
    return pattern.test(value || '');
  }

  submitDetails() {
    if (this.registerForm.invalid) {
      this.messageService.add({
        severity: 'warn',
        summary:  'Validation Error',
        detail:   'Please fill out the form correctly.'
      });
      return;
    }
  
    const postData: RegisterStudent = {
      UserName: this.registerForm.value.userName!,
      FCAIID: parseInt(this.registerForm.value.fcaiid!),
      Password: this.registerForm.value.password!,
      UniversityId: parseInt(this.registerForm.value.UniversityId!),
      DepartmentId: parseInt(this.registerForm.value.DepartmentId!),
      UserTerm: parseInt(this.registerForm.value.UserTerm!)
    };
    console.log('Register users Term :', postData.UserTerm);
    this.authService.registerUser(postData)
      .subscribe({
        next: (response) => {
          console.log('Token:', response.Token);
          localStorage.setItem('token', response?.Token as string);
          console.log(response);
          this.messageService.add({
            severity: 'success',
            summary:  'Success',
            detail:   'Registered successfully!'
          });
          console.log('Navigating to register-page2 with studentId:', postData.FCAIID);
          this.router.navigate(['register-page2'], { queryParams: { studentId: postData.FCAIID } });
          console.log('Navigated to register-page2');
        },
        error: (err: HttpErrorResponse) => {
          console.log(err);
          const payload = err.error as { StatusCode: number; Message: string; Details: any };
          // show the server's Message
          const userMsg = payload?.Message ?? 'Unexpected error';
      
          this.messageService.add({
            severity: 'error',
            summary: `Error ${err.status}`,
            detail: userMsg
          });
        }
      });
  }

  // Navigation methods
  nextStep() {
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
    }
  }

  previousStep() {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  isStepValid(step: number): boolean {
    if (step === 1) {
      return this.userName.valid && this.fcaiid.valid && this.password.valid && this.confirmPassword.valid;
    } else if (step === 2) {
      return this.UniversityId.valid && this.UserTerm.valid;
    }
    return false;
  }

  canProceedToNext(): boolean {
    return this.isStepValid(this.currentStep);
  }

  isGeneralCairoUniversitySelected(): boolean {
    if(this.UniversityId.value == null) return false;
    console.log("University Value is: ", this.UniversityId.value);
    const universityValue = this.UniversityId.value;
    console.log("University Value == 2 ", universityValue == 2);
    return universityValue == 2; // true if 2 or "2", false otherwise (including null/undefined)
  }
}
