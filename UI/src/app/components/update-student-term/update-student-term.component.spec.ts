import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateStudentTermComponent } from './update-student-term.component';

describe('UpdateStudentTermComponent', () => {
  let component: UpdateStudentTermComponent;
  let fixture: ComponentFixture<UpdateStudentTermComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateStudentTermComponent]
    });
    fixture = TestBed.createComponent(UpdateStudentTermComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
