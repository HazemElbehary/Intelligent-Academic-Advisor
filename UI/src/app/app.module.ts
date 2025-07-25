import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MessageService } from 'primeng/api';
import { DropdownModule } from 'primeng/dropdown';
import { RegisterPage2Component } from './components/register-page2/register-page2.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { ChatBotComponent } from './components/chat-bot/chat-bot.component';
import { RecommendationPlanComponent } from './components/recommendation-plan/recommendation-plan.component';
import { AdminSearchComponent } from './components/admin-search/admin-search.component';
import { AdminInsightsComponent } from './components/admin-insights/admin-insights.component';
import { AddDepartmentComponent } from './components/add-department/add-department.component';
import { UpdateStudentTermComponent } from './components/update-student-term/update-student-term.component';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    RegisterPage2Component,
    ChatBotComponent,
    RecommendationPlanComponent,
    AdminSearchComponent,
    AdminInsightsComponent,
    AddDepartmentComponent,
    UpdateStudentTermComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CardModule,
    InputTextModule,
    ReactiveFormsModule,
    ButtonModule,
    HttpClientModule,
    ToastModule,
    BrowserAnimationsModule,
    DropdownModule,
    CommonModule,
    FormsModule,
    CheckboxModule
  ],
  providers: [MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
