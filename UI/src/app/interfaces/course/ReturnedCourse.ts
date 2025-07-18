export interface ReturnedCourse {
    Code: string;
    Name: string;
  
    // FCAI-only fields (nullable)
    DistributionCategory?: string;
    Type?: string;
    Term?: string;
  
    // External-only field (nullable)
    EquivalentCourseCode?: string;
  }
  