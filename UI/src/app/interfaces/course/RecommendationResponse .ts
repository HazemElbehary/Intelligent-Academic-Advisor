export interface RecommendationResponse {
    core_courses: string[];
    distribution: {
      Applied_Sciences: number;
      General_Requirements: number;
      Graduation_Project: number;
    };
    electives: {
      Applied: number;
      AppliedOptions: string[];
      General: number;
      GeneralOptions: string[];
      TotalElectives: number;
      UsedElectiveCredits: number;
    };
    ineligible_courses: {
      CourseCode: string;
      MissingPrereqs: string;
    }[];
    outside_dept: {
      AvailableOutside: string[];
      CanTakeOutside: number;
      NumOutsideDeptTaken: number;
    };
    remaining_requirements: {
      [key: string]: string;
    };
    student_summary: {
      AcademicLevel: string;
      CreditLimit: number;
      CurrentTerm: string;
      Department: string;
      Gpa: number;
      StudentId: string;
      TotalCompletedHours: number;
      TotalRemaining: number;
    };
    total_core_credits: number;
}  