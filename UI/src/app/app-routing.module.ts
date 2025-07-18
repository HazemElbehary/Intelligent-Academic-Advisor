import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { authGuard } from './guards/auth.guard';
import { RegisterPage2Component } from './components/register-page2/register-page2.component';
import { ChatBotComponent } from './components/chat-bot/chat-bot.component';
import { RecommendationPlanComponent } from './components/recommendation-plan/recommendation-plan.component';
import { AdminSearchComponent } from './components/admin-search/admin-search.component';
import { AdminInsightsComponent } from './components/admin-insights/admin-insights.component';
import { AddDepartmentComponent } from './components/add-department/add-department.component';
import { UpdateStudentTermComponent } from './components/update-student-term/update-student-term.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'register-page2',
    component: RegisterPage2Component,
    canActivate: [authGuard]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        component: RecommendationPlanComponent
      },
      {
        path: 'chat-bot',
        component: ChatBotComponent
      }
    ]
  },
  {
    path: 'admin-search',
    component: AdminSearchComponent
  },
  {
    path: 'admin-insights',
    component: AdminInsightsComponent
  },
  {
    path: 'add-department',
    component: AddDepartmentComponent,
    canActivate: [authGuard]
  },
  {
    path: 'update-student-term',
    component: UpdateStudentTermComponent,
    canActivate: [authGuard]
  },
  {
    path: '', redirectTo: 'home', pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
