using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Controllers;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo("LMSControllerTests")]
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var query = from s in db.Enrolleds
                        where subject == s.Class.Course.DepartmentCodeNavigation.DepartmentCode
                        && num == s.Class.Course.CourseNumber
                        && season == s.Class.Season
                        && year == s.Class.Semester
                        select new
                        {
                            fname = s.UIdNavigation.FirstName,
                            lname = s.UIdNavigation.LastName,
                            uid = s.UIdNavigation.UId,
                            dob = s.UIdNavigation.Dob,
                            grade = s.Grade
                        };


            return Json(query.ToArray());
        }


        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            var assignments = (from a in db.Assignments
                               where subject == a.Category.Class.Course.DepartmentCodeNavigation.DepartmentCode
                               && num == a.Category.Class.Course.CourseNumber
                               && season == a.Category.Class.Season
                               && year == a.Category.Class.Semester
                               && (category == null
                                  || category == a.Category.CategoryName)
                               select new
                               {
                                   aname = a.AssignmentName,
                                   cname = a.Category.CategoryName,
                                   due = a.DueDate,
                                   submissions = a.Submissions.Count
                               }
                         );


            return Json(assignments.ToArray());

        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the following fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from a in db.AssignmentCategories
                        where subject == a.Class.Course.DepartmentCode
                        && num == a.Class.Course.CourseNumber
                        && season == a.Class.Season
                        && year == a.Class.Semester
                        select new
                        {
                            name = a.CategoryName,
                            weight = a.GradeWeight
                        };

            return Json(query.ToArray());
        }


        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {

            var query = (from a in db.Classes
                         where subject == a.Course.DepartmentCode
                         && num == a.Course.CourseNumber
                         && season == a.Season
                         && year == a.Semester
                         select a).FirstOrDefault();


            if (query == null)
            {
                return Json(new { success = false });
            }

            var existingCat = db.AssignmentCategories.Any(c => c.ClassId == query.ClassId && c.CategoryName == category);


            if (existingCat)
            {
                return Json(new { success = false });
            }

            AssignmentCategory newCat = new AssignmentCategory
            {
                ClassId = query.ClassId,
                CategoryName = category,
                GradeWeight = (uint)catweight
            };

            db.AssignmentCategories.Add(newCat);
            db.SaveChanges();

            return Json(new { success = true });




        }


        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            var classQuery = (from a in db.Classes
                              where subject == a.Course.DepartmentCode
                              && num == a.Course.CourseNumber
                              && season == a.Season
                              && year == a.Semester
                              select a).FirstOrDefault();

            if (classQuery == null)
            {
                return Json(new { success = false });
            }

            var existingCat = (from c in db.AssignmentCategories
                               where c.ClassId == classQuery.ClassId
                               && category == c.CategoryName
                               select c).FirstOrDefault();

            if (existingCat == null)
            {
                return Json(new { success = false });
            }

            Assignment newAssignment = new Assignment();

            newAssignment.CategoryId = existingCat.CategoryId;
            newAssignment.AssignmentName = asgname;
            newAssignment.MaxPoints = (uint)asgpoints;
            newAssignment.DueDate = asgdue;
            newAssignment.Contents = asgcontents;

            try
            {
                db.Assignments.Add(newAssignment);
                db.SaveChanges();
            }
            catch
            {
                return Json(new { success = false });
            }

            // Update all students grade in class 
            var enrolled = (from e in db.Enrolleds
                            where e.ClassId == classQuery.ClassId
                            select e).ToList();

            foreach (var enrollment in enrolled)
            {
                string newLetterGrade = UpdateGrade((int)enrollment.ClassId, enrollment);
                enrollment.Grade = newLetterGrade;
            }
            try
            {
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from s in db.Submissions
                        where subject == s.Assignment.Category.Class.Course.DepartmentCode
                        && num == s.Assignment.Category.Class.Course.CourseNumber
                        && season == s.Assignment.Category.Class.Season
                        && year == s.Assignment.Category.Class.Semester
                        && category == s.Assignment.Category.CategoryName
                        && asgname == s.Assignment.AssignmentName

                        select new
                        {
                            fname = s.UIdNavigation.FirstName,
                            lname = s.UIdNavigation.LastName,
                            uid = s.UId,
                            time = s.SubmissionTime,
                            score = s.Score
                        };



            return Json(query.ToArray());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            var classId = (from c in db.Classes
                           where subject == c.Course.DepartmentCode
                           && num == c.Course.CourseNumber
                           && season == c.Season
                           && year == c.Semester
                           select c.ClassId).Single();

            var studentInClass = (from s in db.Enrolleds
                                  where s.ClassId == classId
                                  && s.UId == uid
                                  select s).Single();

            var assignmentID = (from a in db.Assignments
                                where a.Category.ClassId == classId
                                && a.AssignmentName == asgname
                                select a.AssignmentId).Single();

            var submission = (from s in db.Submissions
                              where s.AssignmentId == assignmentID
                              && s.UId == uid
                              select s).SingleOrDefault();
            if (submission == null)
                return Json(new { success = false });

            try
            {
                submission.Score = (uint)score;
                db.SaveChanges();
            }
            catch
            {
                return Json(new { success = false });
            }

            string letterGrade = UpdateGrade((int)classId, studentInClass);

            try
            {
                studentInClass.Grade = letterGrade;
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from p in db.Classes
                        where p.TaughtBy == uid
                        select new
                        {
                            subject = p.Course.DepartmentCodeNavigation.DepartmentCode,
                            number = p.Course.CourseNumber,
                            name = p.Course.CourseName,
                            season = p.Season,
                            year = p.Semester

                        };
            return Json(query.ToArray());

        }

        /// <summary>
        /// Computes a student's current letter grade in a class by:
        /// 1. Loading all non-empty assignment categories for the given class.
        /// 2. For each category, summing the student's earned points (treating missing submissions as 0)
        ///     and dividing by the total possible points to get a category percentage.
        /// 3. Scaling each category percentage by its weight, then rescaling so weights sum to 100%.
        /// 4. Translating the final numeric percentage into a letter grade.
        /// </summary>
        /// <param name="classID">Identifies the class whose grade to compute.</param>
        /// <param name="studentInClass">The student whose grade to compute.</param>
        /// <returns>The student's letter grade in a class.<returns>
        private string UpdateGrade(int classID, Enrolled studentInClass)
        {
            var assignmentCategories = (from c in db.AssignmentCategories
                                        where c.Class.ClassId == classID
                                        select c).ToList();

            double totalPointsEarned = 0;
            double totalWeight = 0;

            foreach (var category in assignmentCategories)
            {

                var assignments = (from a in db.Assignments
                                   where a.CategoryId == category.CategoryId
                                   select a).ToList();

                if (assignments.Count == 0)
                {
                    continue;
                }

                double catTotalPoints = 0;
                double catMaxPoints = 0;

                foreach (var assignment in assignments)
                {

                    var submission = (from s in db.Submissions
                                      where s.AssignmentId == assignment.AssignmentId
                                      && s.UId == studentInClass.UId
                                      select s).FirstOrDefault();

                    catMaxPoints = assignment.MaxPoints + catMaxPoints;

                    if (submission == null)
                    {
                        continue;
                    }
                    else
                    {
                        // Adding students scores for category 
                        catTotalPoints = submission.Score + catTotalPoints;

                    }
                }
                // Category percentage and scaling to category weight
                if (catMaxPoints > 0)
                {
                    double percent = catTotalPoints / catMaxPoints;
                    totalPointsEarned = percent * category.GradeWeight;
                    totalWeight = category.GradeWeight + totalWeight;
                }
            }
            if (totalWeight <= 0)
                return "E";
            // rescaling so weights sum to 100%
            double scalingFactor = 100 / totalWeight;
            double totalScaled = totalPointsEarned * scalingFactor;
            return LetterGrade(totalScaled);
        }

        /// <summary>
        /// Converts a numeric percentage score into its corresponding letter grade.
        /// </summary>
        /// <param name="percent">The percentage score.</param>
        /// <returns>A letter grade string corresponding to the given percentage.</returns>
        private static string LetterGrade(double percent)
        {
            if (percent >= 93)
                return "A";
            else if (percent >= 90)
                return "A-";
            else if (percent >= 87)
                return "B+";
            else if (percent >= 83)
                return "B";
            else if (percent >= 80)
                return "B-";
            else if (percent >= 77)
                return "C+";
            else if (percent >= 73)
                return "C";
            else if (percent >= 70)
                return "C-";
            else if (percent >= 67)
                return "D+";
            else if (percent >= 63)
                return "D";
            else if (percent >= 60)
                return "D-";
            else
                return "E";
        }
        /*******End code to modify********/
    }
}

