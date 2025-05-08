using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo("LMSControllerTests")]
namespace LMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private LMSContext db;
        public StudentController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
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


        public IActionResult ClassListings(string subject, string num)
        {
            System.Diagnostics.Debug.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from c in db.Classes
                        where c.Enrolleds.Any(e => e.UId == uid)
                        select new
                        {
                            subject = c.Course.DepartmentCode, 
                            number = c.Course.CourseNumber,
                            name = c.Course.CourseName,
                            season = c.Season,
                            year = c.Semester,  
                            grade = c.Enrolleds
                                .Where(e => e.UId == uid)
                                .Select(e => e.Grade)
                                .FirstOrDefault() ?? "--"
                        };
            return Json(query.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {

            var assignmentsInClass = from a in db.Assignments
                                     where a.Category.Class.Course.DepartmentCode == subject
                                     && a.Category.Class.Course.CourseNumber == num
                                     && a.Category.Class.Season == season
                                     && a.Category.Class.Semester == year

                                     select new
                                     {
                                         aname = a.AssignmentName,
                                         cname = a.Category.CategoryName,
                                         due = a.DueDate,
                                         score = a.Submissions
                                             .Where(s => s.UId == uid)
                                             .Select(s => s.Score)
                                             .FirstOrDefault()
                                     };

            return Json(assignmentsInClass.ToArray());
        }



        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)
        {
            var currentTime = DateTime.Now;
            var assignment = (from c in db.Classes
                              where c.Course.DepartmentCode == subject && c.Course.CourseNumber == num
                              where c.Season == season && c.Semester == year
                              from assignCat in db.AssignmentCategories
                              from a in db.Assignments
                              where assignCat.CategoryName == category && a.AssignmentName == asgname
                              select a).FirstOrDefault();

            // a assignment does not exist
            if (assignment == null)
            {
                return Json(new { success = false });
            }

            //check for existing submission
            var existingAssignment = db.Submissions.FirstOrDefault(s => s.AssignmentId == assignment.AssignmentId && s.UId == uid);


            //current submission exists
            if (existingAssignment != null)
            {
                existingAssignment.SubmissionContent = contents;
                existingAssignment.SubmissionTime = currentTime;
            }
            else
            {
                Submission submission = new Submission
                {
                    AssignmentId = assignment.AssignmentId,
                    SubmissionTime = currentTime,
                    Score = 0,
                    UId = uid,
                    SubmissionContent = contents,
                };
                db.Submissions.Add(submission);
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
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false}. 
        /// false if the student is already enrolled in the class, true otherwise.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {


            var curClass = (from c in db.Classes
                            where year == c.Semester
                            && season == c.Season
                            && subject == c.Course.DepartmentCode
                            && num == c.Course.CourseNumber
                            select c.ClassId).FirstOrDefault();

            if (curClass == 0)
            {
                return Json(new { success = false });
            }

            var existingEnrollment = (from e in db.Enrolleds
                                      where e.UId == uid
                                      && e.ClassId == curClass
                                      select e).FirstOrDefault();

            if (existingEnrollment == null)
            {

                Enrolled newEnroll = new Enrolled
                {
                    UId = uid,
                    ClassId = curClass,
                    Grade = "--"
                };

                db.Enrolleds.Add(newEnroll);
                db.SaveChanges();

                return Json(new { success = true });

            }
            else
            {

                return Json(new { success = false });
            }
        }



        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student is not enrolled in any classes, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {
            var studGrades = (from g in db.Enrolleds
                              where g.UId == uid
                              && g.Grade != "--"
                              select g.Grade).ToList();

            if (studGrades.Count == 0)
            {
                return Json(new { gpa = 0.0 });
            }

            var totalPoints = 0.0;
            var totalAttemptedHours = studGrades.Count * 4;

            foreach (var grade in studGrades)
            {
                var points = GetGradePoints(grade);
                totalPoints = (points * 4) + totalPoints;

            }

            var gpa = totalPoints / totalAttemptedHours;

            return Json(new { gpa = gpa });
        }

        /// <summary>
        /// Converts a letter grade into the standardized grade-point value.
        /// </summary>
        /// <param name="currentGrade">Letter grade</param>
        /// <returns>Grade point</returns>
        public static double GetGradePoints(string currentGrade)
        {

            double points = 0;

            switch (currentGrade)
            {
                case "A":
                    points = 4.0;
                    break;
                case "A-":
                    points = 3.7;
                    break;
                case "B+":
                    points = 3.3;
                    break;
                case "B":
                    points = 3.0;
                    break;
                case "B-":
                    points = 2.7;
                    break;
                case "C+":
                    points = 2.3;
                    break;
                case "C":
                    points = 2.0;
                    break;
                case "C-":
                    points = 1.7;
                    break;
                case "D+":
                    points = 1.3;
                    break;
                case "D":
                    points = 1.0;
                    break;
                case "D-":
                    points = 0.7;
                    break;
                case "E":
                    points = 0.0;
                    break;
                default:
                    points = 0.0;
                    break;

            }

            return points;
        }

        /*******End code to modify********/

    }
}

