import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LoginStudent } from 'src/app/interfaces/student/loginStudent';
import { LoginDto } from 'src/app/interfaces/admin/admin';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = this.fb.group({
    role: ['', Validators.required],
    identifier: ['', Validators.required],
    password: ['', Validators.required]
  });

  roleOptions = [
    { label: 'Student', value: 'user' },
    { label: 'Admin', value: 'admin' }
  ];

  selectedRole: string = 'user';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private msgService: MessageService
  ) { }

  ngOnInit() {
    // Listen to role changes to update validation
    this.loginForm.get('role')?.valueChanges.subscribe(role => {
      this.selectedRole = role || 'user';
      this.updateIdentifierValidation();
    });
  }

  get role() {
    return this.loginForm.controls['role'];
  }

  get identifier() {
    return this.loginForm.controls['identifier'];
  }

  get password() { 
    return this.loginForm.controls['password']; 
  }

  private updateIdentifierValidation() {
    const identifierControl = this.loginForm.get('identifier');
    if (this.selectedRole === 'user') {
      identifierControl?.setValidators([Validators.required, Validators.pattern(/^202\d{5}$/)]);
    } else {
      identifierControl?.setValidators([Validators.required]);
    }
    identifierControl?.updateValueAndValidity();
  }

  loginUser() {
    if (this.selectedRole === 'user') {
      this.loginAsUser();
    } else {
      this.loginAsAdmin();
    }
  }

  private loginAsUser() {
    const postData: LoginStudent = { 
      FCAIID: (this.identifier.value as unknown) as number, 
      Password: this.password.value as string 
    };
    
    this.authService.loginUser(postData).subscribe({
      next: response => {
        console.log(response);
        if (response.status === 200) {
          localStorage.setItem('token', response.body?.Token as string);
          localStorage.setItem('userRole', 'user');
          this.router.navigate(['home']);
        } else {
          this.msgService.add({ severity: 'error', summary: 'Error', detail: 'Something went wrong' });
        }
      },
      error: err => {
        console.log(err);
        this.msgService.add({ severity: 'error', summary: 'Error', detail: 'FCAI ID or password is wrong' });
      }
    });
  }

  private loginAsAdmin() {
    const postData: LoginDto = { 
      FCAIID: parseInt(this.identifier.value as string), 
      Password: this.password.value as string 
    };
    
    this.authService.loginAdmin(postData).subscribe({
      next: response => {
        if (response.status === 200) {
          localStorage.setItem('token', response.body?.Token as string);
          localStorage.setItem('userRole', 'admin');
          localStorage.setItem('adminInfo', JSON.stringify({
            adminID: response.body?.AdminID,
            userName: response.body?.UserName,
            fullName: response.body?.FullName,
            position: response.body?.Position,
            department: response.body?.Department
          }));
          this.router.navigate(['admin-search']);
        } else {
          this.msgService.add({ severity: 'error', summary: 'Error', detail: 'Something went wrong' });
        }
      },
      error: err => {
        console.log("Error: ", err);
        this.msgService.add({ severity: 'error', summary: 'Error', detail: 'FCAI ID or password is wrong' });
      }
    });
  }
}
