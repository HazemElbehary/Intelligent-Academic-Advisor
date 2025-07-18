using FCAI.Application.Abstraction.DTOs;
using FCAI.Application.Abstraction.DTOs.AI_Response;
using FCAI.Application.Abstraction.DTOs.CourseController;
using FCAI.Application.Abstraction.Exceptions;
using FCAI.Application.Abstraction.IServices;
using FCAI.Domain.Contracts;
using FCAI.Domain.Entities;
using FCAI.Domain.Enums;
using FCAI.Domain.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FCAI.Application.Services.StudentServices
{
    public class StudentService(
        UserManager<Student> userManager,
        IUnitOfWork unitOfWork,
        IAIRecommendationService _aiService,
        IMemoryCache memoryCache) 
    : IStudentService
    {
        private const string AllFCAICoursesCacheKey = "AllFCAICourses";
        private const int CacheExpirationDays = 30;

        private async Task<Dictionary<string, string>> GetCachedCourseNamesAsync()
        {
            if (!memoryCache.TryGetValue(AllFCAICoursesCacheKey, out Dictionary<string, string> courseNameDict))
            {
                // Cache miss - fetch from database
                var courses = await unitOfWork.GetRepository<FCAICourses, string>()
                    .GetAllWithSpecAsync(new BaseISpecifications<FCAICourses, string>(x => true));
                
                courseNameDict = courses.ToDictionary(c => c.Code, c => c.Name);
                
                // Set cache with expiration
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(CacheExpirationDays));
                
                memoryCache.Set(AllFCAICoursesCacheKey, courseNameDict, cacheEntryOptions);
            }
            
            return courseNameDict;
        }

        public void ClearCourseCache()
        {
            memoryCache.Remove(AllFCAICoursesCacheKey);
        }

        public async Task<decimal> GetStudentGPAAsync(string? studentId)
        {
            if(string.IsNullOrEmpty(studentId))
                throw new ApiExceptionResponse(400, "Student ID is required.");

            var student = await userManager.Users
                    .Include(u => u.StudentCourses)
                        .ThenInclude(sc => sc.Course)
                    .FirstOrDefaultAsync(u => u.FCAIID == int.Parse(studentId));

            if (student == null)
                throw new ApiExceptionResponse(404, "Student not found.");

            return student.GPA;
        }

        public async Task AddCoursesToStudentAsync(string[] courseCodes, string[] courseGrades, string? studentIdClaim)
        {
            if (courseCodes == null || courseCodes.Length == 0)
                throw new ApiExceptionResponse(400, "At least one course code must be provided.");
            if (courseGrades == null || courseCodes.Length != courseGrades.Length)
                throw new ApiExceptionResponse(400, "The number of grades provided must match the number of course codes.");

            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out int studentId))
                throw new ApiExceptionResponse(400, "Invalid or missing student ID.");

            // Fetch the student entity
            var student = await unitOfWork.GetRepository<Student, int>().GetWithSpecAsync(new BaseISpecifications<Student, int>(s => s.FCAIID == studentId));
            if (student is null)
                throw new ApiExceptionResponse(404, "Student not found.");

            // If student has a department, check total credits
            if (student.DepartmentId != null)
            {
                // Get completed courses
                var studentCompletedCourses = await unitOfWork.GetRepository<StudentCourse, string>()
                    .GetAllWithSpecAsync(new BaseISpecifications<StudentCourse, string>(sc => sc.StudentFCAIID == studentId));
                var completedCourseCodes = studentCompletedCourses.Select(sc => sc.CourseCode).ToList();
                // Combine with current registration
                var allRelevantCourses = completedCourseCodes.Concat(courseCodes).Distinct().ToList();
                var totalCreditHours = await unitOfWork.GetRepository<FCAICourses, string>()
                    .GetAllWithSpecAsync(new BaseISpecifications<FCAICourses, string>(c => allRelevantCourses.Contains(c.Code)));
                var totalCredits = totalCreditHours.Sum(c => c.CreditHours);
                if (totalCredits < 60)
                {
                    throw new ApiExceptionResponse(400, "You must choose courses totaling at least 60 credit hours as you're in a department.");
                }
            }

            var firstRegisteredCourse = await unitOfWork.GetRepository<FCAICourses, string>().GetAsync(courseCodes[0]);


            // student try to register courses from FCAI university
            if (firstRegisteredCourse is not null || student.UniversityId == 2)
            {
                for (int i = 0; i < courseCodes.Length; i++)
                {
                    var code = courseCodes[i];
                    var grade = courseGrades[i];

                    // Get course prerequisites
                    var coursePrerequisites = await unitOfWork.GetRepository<CoursePrerequisite, string>()
                        .GetAllWithSpecAsync(new BaseISpecifications<CoursePrerequisite, string>(
                            cp => cp.CourseCode == code));

                    // Check if student can register this course
                    bool canRegister = true;
                    var missingPrerequisites = new List<string>();

                    foreach (var prereq in coursePrerequisites)
                    {
                        if (prereq.PrerequisiteCourseCode != null)
                        {
                            // Check if prerequisite course is completed or being registered now
                            var prereqCompleted = await unitOfWork.GetRepository<StudentCourse, string>()
                                .GetWithSpecAsync(new BaseISpecifications<StudentCourse, string>(
                                    sc => sc.StudentFCAIID == studentId && sc.CourseCode == prereq.PrerequisiteCourseCode));

                            bool prereqInCurrentRegistration = courseCodes.Contains(prereq.PrerequisiteCourseCode);

                            if (prereqCompleted == null && !prereqInCurrentRegistration)
                            {
                                canRegister = false;
                                // Get course name for the prerequisite
                                var prereqCourse = await unitOfWork.GetRepository<FCAICourses, string>()
                                    .GetAsync(prereq.PrerequisiteCourseCode);
                                missingPrerequisites.Add(prereqCourse?.Name ?? prereq.PrerequisiteCourseCode);
                            }
                        }
                        else if (prereq.PrerequisiteCreditHours > 0)
                        {
                            // Check if student has enough credit hours
                            var studentCompletedCourses = await unitOfWork.GetRepository<StudentCourse, string>()
                                .GetAllWithSpecAsync(new BaseISpecifications<StudentCourse, string>(
                                    sc => sc.StudentFCAIID == studentId));

                            // Get credit hours for completed courses
                            var completedCourseCodes = studentCompletedCourses.Select(sc => sc.CourseCode).ToList();
                            var coursesBeingRegistered = courseCodes.ToList();

                            var allRelevantCourses = completedCourseCodes.Concat(coursesBeingRegistered).Distinct().ToList();

                            var totalCreditHours = await unitOfWork.GetRepository<FCAICourses, string>()
                                .GetAllWithSpecAsync(new BaseISpecifications<FCAICourses, string>(
                                    c => allRelevantCourses.Contains(c.Code)));

                            var totalCredits = totalCreditHours.Sum(c => c.CreditHours);

                            if (totalCredits < prereq.PrerequisiteCreditHours)
                            {
                                canRegister = false;
                                missingPrerequisites.Add($"Need {prereq.PrerequisiteCreditHours} credit hours (have {totalCredits})");
                            }
                        }
                    }

                    if (!canRegister)
                    {
                        // Get the current course name
                        var currentCourse = await unitOfWork.GetRepository<FCAICourses, string>().GetAsync(code);
                        var courseName = currentCourse?.Name ?? code;
                        
                        throw new ApiExceptionResponse(400, 
                            $"Cannot register course '{courseName}'. Missing prerequisites: {string.Join(", ", missingPrerequisites)}");
                    }

                    // Check if the student is already registered for this course
                    var existingRegistration = await unitOfWork.GetRepository<StudentCourse, string>()
                        .GetWithSpecAsync(new BaseISpecifications<StudentCourse, string>(
                            sc => sc.StudentFCAIID == studentId && sc.CourseCode == code));

                    if (existingRegistration is not null)
                    {
                        // Update the grade
                        existingRegistration.Grade = Enum.Parse<Grades>(grade, ignoreCase: true);
                        unitOfWork.GetRepository<StudentCourse, string>().Update(existingRegistration);
                    }
                    else
                    {
                        // Add new registration
                        var registration = new StudentCourse
                        {
                            CourseCode = code,
                            StudentFCAIID = studentId,
                            Grade = Enum.Parse<Grades>(grade, ignoreCase: true)
                        };
                        await unitOfWork.GetRepository<StudentCourse, string>().AddAsync(registration);
                    }
                }
            }

            // student try to register courses from external university
            else
            {
                var externalCoursesSpec = new BaseISpecifications<ExternalCourses, string>(x => courseCodes.Contains(x.Code));
                var externalCourses = await unitOfWork.GetRepository<ExternalCourses, string>().GetAllWithSpecAsync(externalCoursesSpec);

                var registrations = externalCourses
                    .Zip(courseGrades, (externalCourse, grade) => new StudentCourse
                    {
                        CourseCode = externalCourse.EquivalentCourseCode,
                        StudentFCAIID = studentId,
                        Grade = Enum.Parse<Grades>(grade, ignoreCase: true)
                    })
                    .ToList();

                await unitOfWork.GetRepository<StudentCourse, string>().AddRangeAsync(registrations);
            }
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ReturnedCourseDto>> GetCoursesBasedOnStudentIdAsync(string? FCAIIDClaim, string? universityIdClaim)
        {
            if (string.IsNullOrEmpty(universityIdClaim) || !int.TryParse(universityIdClaim, out int universityId))
                throw new ApiExceptionResponse(400, "Invalid or missing university ID.");

            if (string.IsNullOrEmpty(FCAIIDClaim) || !int.TryParse(FCAIIDClaim, out int studentId))
                throw new ApiExceptionResponse(400, "Invalid or missing student ID.");

            var university = await unitOfWork.GetRepository<University, int>().GetAsync(universityId);

            if (university is null)
                throw new ApiExceptionResponse(404, "University not found.");

            var FCAIIDSpec = new BaseISpecifications<Student, int>(x => x.FCAIID == studentId);
            var student = await unitOfWork.GetRepository<Student, int>().GetWithSpecAsync(FCAIIDSpec);

            if (student is null)
                throw new ApiExceptionResponse(404, $"Student with Id {studentId} is not found.");

            var spec = new BaseISpecifications<StudentCourse, string>(x => x.StudentFCAIID == studentId);
            var isOldExternalStudent = await unitOfWork.GetRepository<StudentCourse, string>().GetWithSpecAsync(spec);

            return (university.Name == "General Cairo University" || isOldExternalStudent is not null)
                ? await GetAvailableCoursesAsync(studentId, student?.DepartmentId)
                : await GetExternalCoursesAsync(universityId);
        }

        // Courses that are available for the student to register in
        private async Task<IEnumerable<ReturnedCourseDto>> GetAvailableCoursesAsync(int FCAIID, int? departmentId)
        {
            // 1) which courses the student has already taken?
            var takenSpec = new BaseISpecifications<StudentCourse, string>(x => x.StudentFCAIID == FCAIID && x.Grade != Grades.F);
            var takenCourses = await unitOfWork
                                     .GetRepository<StudentCourse, string>()
                                     .GetAllWithSpecAsync(takenSpec);
            var takenCodes = takenCourses.Select(sc => sc.CourseCode).ToHashSet();

            // 2) pull all FCAI courses that are NOT in takenCodes
            var notTakenSpec = new BaseISpecifications<FCAICourses, string>(c => !takenCodes.Contains(c.Code));
            var notTakenCourses = await unitOfWork
                                        .GetRepository<FCAICourses, string>()
                                        .GetAllWithSpecAsync(notTakenSpec);

            // 3) pull all department‐to‐course mappings once
            var allDeptMappings = await unitOfWork
                                        .GetRepository<CourseDepartments, string>()
                                        .GetAllAsync();
            // set of codes that have any department
            var codesWithAnyDept = allDeptMappings
                                      .Select(m => m.CourseCode)
                                      .ToHashSet();
            // set of codes mapped to our specific department (if any)
            var codesInSelectedDept = departmentId.HasValue
                ? allDeptMappings
                    .Where(m => m.DepartmentID == departmentId.Value)
                    .Select(m => m.CourseCode)
                    .ToHashSet()
                : new HashSet<string>();

            // 4) now filter the not‐taken list:
            //   - if departmentId == null: only those NOT in any department
            //   - if departmentId != null: those NOT in any department OR in the selected department
            var filtered = notTakenCourses.Where(c =>
                departmentId.HasValue
                    ? (!codesWithAnyDept.Contains(c.Code)
                       || codesInSelectedDept.Contains(c.Code))
                    : !codesWithAnyDept.Contains(c.Code)
            );

            // 5) map to DTO
            return filtered.Select(c => new ReturnedCourseDto
            {
                Code = c.Code,
                Name = c.Name,
                DistributionCategory = c.DistributionCategory.ToString(),
                Type = c.Type.ToString(),
                Term = c.Term.ToString()
            });
        }

        private async Task<IEnumerable<ReturnedCourseDto>> GetExternalCoursesAsync(int universityId)
        {
            var spec = new BaseISpecifications<ExternalCourses, string>(x => x.UniversityCourse.ID == universityId);
            var courses = await unitOfWork.GetRepository<ExternalCourses, string>()
                                          .GetAllWithSpecAsync(spec);

            return courses.Select(c => new ReturnedCourseDto
            {
                Code = c.Code,
                Name = c.Name,
                EquivalentCourseCode = c.EquivalentCourseCode
            });
        }

        private async Task<List<AIReturnedCourseDto>> GetCompletedCoursesForStudentAsync(int FCAIID)
        {
            var completedCourses = await unitOfWork.GetRepository<StudentCourse, string>()
                .GetAllWithSpecAsync(new BaseISpecifications<StudentCourse, string>(x => x.StudentFCAIID == FCAIID));

            if (completedCourses == null || !completedCourses.Any())
                return new List<AIReturnedCourseDto>();

            // Filter out F grades
            return completedCourses
                .Where(sc => sc.Grade != Grades.F)
                .SelectMany(sc => MapCourseToAiDtos(sc.Course))
                .ToList();
        }

        private IEnumerable<AIReturnedCourseDto> MapCourseToAiDtos(FCAICourses course)
        {
            // If the course has departments, create a DTO for each one.
            if (course.CourseDepartments != null && course.CourseDepartments.Any())
            {
                return course.CourseDepartments.Select(cd => new AIReturnedCourseDto
                {
                    code = course.Code,
                    course_name = course.Name,
                    credit_hours = course.CreditHours,
                    distribution_category = course.DistributionCategory.ToString(),
                    type = course.Type.ToString(),
                    Term = course.Term.ToString() ?? "null",
                    department = cd.Department?.Name ?? "null",
                    prerequisites = course.Prerequisites?
                        .Where(p => p.PrerequisiteCourse != null)
                        .Select(p => p.PrerequisiteCourse!.Code)
                        .ToList() ?? new List<string>()
                });
            }

            // Otherwise, create a single DTO with a null department.
            return new[] { new AIReturnedCourseDto
            {
                code = course.Code,
                course_name = course.Name,
                credit_hours = course.CreditHours,
                distribution_category = course.DistributionCategory.ToString(),
                type = course.Type.ToString(),
                Term = course.Term.ToString() ?? "null",
                department = "null",
                prerequisites = course.Prerequisites?
                        .Where(p => p.PrerequisiteCourse != null)
                        .Select(p => p.PrerequisiteCourse!.Code)
                        .ToList() ?? new List<string>()
            }};
        }

        private async Task<List<AIReturnedCourseDto>> GetAllFCAICoursesAsync()
        {
            var courses = await unitOfWork.GetRepository<FCAICourses, string>()
                .GetAllWithSpecAsync(new BaseISpecifications<FCAICourses, string>(x => true));

            if (courses == null || !courses.Any())
                return new List<AIReturnedCourseDto>();

            return courses
                .SelectMany(c => MapCourseToAiDtos(c))
                .ToList();
        }

        public async Task<object> GetRecommendedPlanAsync(int? studentId, string? requestingUserId = null, string? userType = null)
        {
            // If studentId is provided and user is admin, use that studentId
            // If studentId is null and user is student, use their own ID
            // If studentId is provided and user is student, only allow if it's their own ID
            
            int targetStudentId;
            
            if (studentId.HasValue)
            {
                // Admin can access any student's plan
                if (userType == "Admin")
                {
                    targetStudentId = studentId.Value;
                }
                // Student can only access their own plan
                else if (userType == "Student")
                {
                    if (int.TryParse(requestingUserId, out var requestingId) && requestingId == studentId.Value)
                    {
                        targetStudentId = studentId.Value;
                    }
                    else
                    {
                        throw new ApiExceptionResponse(403, "Students can only access their own recommendation plan.");
                    }
                }
                else
                {
                    throw new ApiExceptionResponse(400, "Invalid user type.");
                }
            }
            else
            {
                // No studentId provided, use the requesting user's ID
                if (int.TryParse(requestingUserId, out var requestingId))
                {
                    targetStudentId = requestingId;
                }
                else
                {
                    throw new ApiExceptionResponse(400, "Student ID is required to get the recommended plan.");
                }
            }

            var student = await GetStudentAsync(targetStudentId);
            var aiRequest = await BuildAiRequestDto(student);
            var aiResponse = await _aiService.GetRecommendationAsync(aiRequest);
            return await BuildEnrichedResponse(aiResponse);
        }

        private async Task<Student> GetStudentAsync(int studentId)
        {
            var spec = new BaseISpecifications<Student, int>(x => x.FCAIID == studentId);
            var student = await unitOfWork.GetRepository<Student, int>().GetWithSpecAsync(spec);
            if (student == null)
                throw new ApiExceptionResponse(404, $"Student with ID {studentId} not found.");
            return student;
        }

        private async Task<AiRequestDto> BuildAiRequestDto(Student student)
        {
            var completedCourses = await GetCompletedCoursesForStudentAsync(student.FCAIID);
            var allCourses = await GetAllFCAICoursesAsync();
            return new AiRequestDto
            {
                StudentId = student.FCAIID,
                DepartmentName = student.Department?.Name ?? "null",
                Term = student.StudentTerm.ToString(),
                GPA = student.GPA,
                CompletedCourses = completedCourses ?? [],
                AllCourses = allCourses
            };
        }

        private async Task<object> BuildEnrichedResponse(AiResponseDto aiResponse)
        {
            // STEP 1: Collect all course Codes
            var courseCodes = new HashSet<string>();
            if (aiResponse.core_courses != null)
                courseCodes.UnionWith(aiResponse.core_courses);
            if (aiResponse.electives?.AppliedOptions != null)
                courseCodes.UnionWith(aiResponse.electives.AppliedOptions);
            if (aiResponse.electives?.GeneralOptions != null)
                courseCodes.UnionWith(aiResponse.electives.GeneralOptions);
            if (aiResponse.outside_dept?.AvailableOutsideCoursesCodes != null)
                courseCodes.UnionWith(aiResponse.outside_dept.AvailableOutsideCoursesCodes);
            if(aiResponse.ineligible_courses != null)
            {
                courseCodes.UnionWith(aiResponse.ineligible_courses.Select(c => c.CourseCode));

                // Handle MissingPrereqs properly
                var missingPrereqCodes = aiResponse.ineligible_courses
                    .Where(c => !string.IsNullOrEmpty(c.MissingPrereqs))
                    .SelectMany(c => c.MissingPrereqs
                        .Split(',')
                        .Select(code => code.Trim())
                        .Where(code => !string.IsNullOrEmpty(code)));

                courseCodes.UnionWith(missingPrereqCodes);
            }
            

            // STEP 2: Get cached course names and filter for required codes
            var allCourseNames = await GetCachedCourseNamesAsync();
            var courseNameDict = allCourseNames
                .Where(kvp => courseCodes.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // STEP 3: Build response with only used properties
            var enrichedResponse = new
            {
                CoreCourses = aiResponse.core_courses?.Select(code => courseNameDict.GetValueOrDefault(code)),
                Electives = new
                {
                    Applied = aiResponse.electives?.Applied ?? 0,
                    AppliedOptions = aiResponse.electives?.AppliedOptions?.Select(code => courseNameDict.GetValueOrDefault(code)),
                    General = aiResponse.electives?.General,
                    GeneralOptions = aiResponse.electives?.GeneralOptions?.Select(code => courseNameDict.GetValueOrDefault(code)),
                    TotalElectives = aiResponse.electives?.TotalElectives,
                    UsedElectiveCredits = aiResponse.electives?.UsedElectiveCredits
                },
                IneligibleCourses = aiResponse.ineligible_courses?.Select(c => new {
                    course = courseNameDict.GetValueOrDefault(c.CourseCode),
                    missingPrereqs = GetMissingPrereqsString(c, courseNameDict)
                }).ToList(),
                OutsideDept = new
                {
                    AvailableOutside = aiResponse.outside_dept?.AvailableOutsideCoursesCodes?.Select(code => courseNameDict.GetValueOrDefault(code)),
                    CoursesCodesCanTakeOutside = aiResponse.outside_dept?.CoursesCodesCanTakeOutside,
                    NumOutsideDeptTakenCourses = aiResponse.outside_dept?.NumOutsideDeptTakenCourses
                },
                remaining_requirements = aiResponse.remaining_requirements,
                student_summary = aiResponse.student_summary
            };
            return enrichedResponse;
        }

        private string GetMissingPrereqsString(IneligibleCourseDto ineligibleCourse, Dictionary<string, string> courseNameDict)
        {
            var missingCourses = new List<string>();
            
            if (!string.IsNullOrEmpty(ineligibleCourse.MissingPrereqs))
            {
                var missingCodes = ineligibleCourse.MissingPrereqs
                    .Split(',')
                    .Select(code => code.Trim())
                    .Where(code => !string.IsNullOrEmpty(code));
                    
                foreach (var missingCode in missingCodes)
                {
                    var missingCourse = courseNameDict.GetValueOrDefault(missingCode);
                    if (!string.IsNullOrEmpty(missingCourse))
                        missingCourses.Add(missingCourse);
                }
            }
            
            return string.Join(", ", missingCourses);
        }
    
        public async Task AddDepartmentToStudentAsync(int deparmtentId, string? studentIdCalim)
        {
            if(!int.TryParse(studentIdCalim, out int studentId))
                throw new ApiExceptionResponse(400, "Invalid or missing student ID token. Please ensure you are properly authenticated.");

            var student = await unitOfWork.GetRepository<Student, int>().GetWithSpecAsync(new BaseISpecifications<Student, int>(s => s.FCAIID == studentId));
        
            if(student is null) throw new ApiExceptionResponse(400, "Student is not exist!");

            if(student.DepartmentId is not null) throw new ApiExceptionResponse(409, "You can't change the department");

            var studentCourses = await unitOfWork.GetRepository<StudentCourse, string>()
                .GetAllWithSpecAsync(new BaseISpecifications<StudentCourse, string>(sc => sc.StudentFCAIID == student.FCAIID));
            var studentCourseCodes = studentCourses.Select(sc => sc.CourseCode).ToList();
            
            var totalCreditHours = await unitOfWork.GetRepository<FCAICourses, string>()
                                .GetAllWithSpecAsync(new BaseISpecifications<FCAICourses, string>(
                                    c => studentCourseCodes.Contains(c.Code)));

            var totalCredits = totalCreditHours.Sum(c => c.CreditHours);

            if(totalCredits < 60) throw new ApiExceptionResponse(403, "You Can't choose department before Third Year");

            var department = await unitOfWork.GetRepository<Department, int>().GetAsync(deparmtentId);
            if (department is null)
                throw new ApiExceptionResponse(404, $"Department with ID {deparmtentId} not found.");

            student.DepartmentId = deparmtentId;
            unitOfWork.GetRepository<Student, int>().Update(student);
            await unitOfWork.CompleteAsync();
        }

        public async Task UpdateTermOfStudentAsync(int termId, string? studentIdCalim)
        {
            if(!int.TryParse(studentIdCalim, out int studentId))
                throw new ApiExceptionResponse(400, "Invalid or missing student ID token. Please ensure you are properly authenticated.");

            var student = await unitOfWork.GetRepository<Student, int>().GetWithSpecAsync(new BaseISpecifications<Student, int>(s => s.FCAIID == studentId));
        
            if(student is null) throw new ApiExceptionResponse(400, "Student is not exist!");

            if (!Enum.IsDefined(typeof(StudentTerm), termId))
                throw new ApiExceptionResponse(400, "Invalid term Id");

            student.StudentTerm = (StudentTerm)termId;
            
            unitOfWork.GetRepository<Student, int>().Update(student);
            await unitOfWork.CompleteAsync();
        }
    }
}
