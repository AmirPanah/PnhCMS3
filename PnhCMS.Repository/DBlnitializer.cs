using PnhCMS.Repository.AppModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Repository
{
    public class DBlnitializer : DropCreateDatabaseAlways <AppContext>  
    {
        protected override void Seed(AppContext context)
        {

            IList<Course> courses = new List<Course>();
            IList<Subject> subjects = new List<Subject>();
            IList<Grade> grades = new List<Grade>();
            IList<Student> students = new List<Student>();
            IList<Teacher> teachers = new List<Teacher>();
            IList<TeacherEnrollment> teacherEnrollments = new List<TeacherEnrollment>();
            IList<StudentEnrollment> studentEnrollments = new List<StudentEnrollment>();

            if(!context.Courses.Any())
            {
                courses.Add(new Course() { CourseId = 2001, CourseCode = "2001", CourseName = "sql", CourseCredit = 3 });
                courses.Add(new Course() { CourseId = 2002, CourseCode = "2002", CourseName = "vb", CourseCredit = 4 });
                courses.Add(new Course() { CourseId = 2003, CourseCode = "2003", CourseName = "ds", CourseCredit = 3 });
                courses.Add(new Course() { CourseId = 2004, CourseCode = "2004", CourseName = "da", CourseCredit = 2 });
               
                context.Courses.AddRange(courses);
            }



            if (!context.Subjects.Any())
            {
                subjects.Add(new Subject() { SubjectId = 3001, SubjectCode = "DB-51", SubjectName = "Data Base", CourseId = 2001, SubjectCredit = 3 });
                subjects.Add(new Subject() { SubjectId = 3002, SubjectCode = "DA-52", SubjectName = "Design Algorithm", CourseId = 2004, SubjectCredit = 3 });
                subjects.Add(new Subject() { SubjectId = 3003, SubjectCode = "P-51", SubjectName = "Propgramming", CourseId = 2002, SubjectCredit = 3 });
                
               
                context.Subjects.AddRange(subjects);
            }

            if (!context.Grades.Any())
            {
                grades.Add(new Grade() { GradeId = 111, GradeName = "A+", GradePoint = 4.00, ScoreFrom = 80, ScoreTo = 100 });
                grades.Add(new Grade() { GradeId = 112, GradeName = "A", GradePoint = 3.75, ScoreFrom = 75, ScoreTo = 79 });
                grades.Add(new Grade() { GradeId = 113, GradeName = "A-", GradePoint = 3.50, ScoreFrom = 70, ScoreTo = 74 });
                grades.Add(new Grade() { GradeId = 114, GradeName = "B+", GradePoint = 3.25, ScoreFrom = 65, ScoreTo = 69 });
                grades.Add(new Grade()
                {
                    GradeId = 115,
                    GradeName = "B",
                    GradePoint = 3.25,
                    ScoreFrom = 60,
                    ScoreTo = 64
                });
                grades.Add(new Grade() { GradeId = 117, GradeName = "B-", GradePoint = 2.75, ScoreFrom = 55, ScoreTo = 59 });
                grades.Add(new Grade() { GradeId = 118, GradeName = "C+", GradePoint = 2.50, ScoreFrom = 50, ScoreTo = 54 });
                grades.Add(new Grade() { GradeId = 119, GradeName = "C", GradePoint = 2.25, ScoreFrom = 45, ScoreTo = 49 });
                grades.Add(new Grade() { GradeId = 120, GradeName = "D", GradePoint = 2.00, ScoreFrom = 40, ScoreTo = 45 });
                grades.Add(new Grade() { GradeId = 121, GradeName = "F", GradePoint = 2.75, ScoreFrom = 0, ScoreTo = 39 });
                context.Grades.AddRange(grades);
            }

            if (!context.Students.Any())
            {

                students.Add(new Student() { StudentId = 20211, StudentName = "A", BirthDate = "10/07/1991", RegistrationNo = "1010100010" });
                students.Add(new Student() { StudentId = 20212, StudentName = "B", BirthDate = "11/07/2000", RegistrationNo = "1010100011" });
                students.Add(new Student() { StudentId = 20213, StudentName = "C", BirthDate = "09/07/2004", RegistrationNo = "1010100012" });
                students.Add(new Student() { StudentId = 20214, StudentName = "D", BirthDate = "08/11/2003", RegistrationNo = "1010100013" });
                students.Add(new Student() { StudentId = 20215, StudentName = "E", BirthDate = "10/07/1998", RegistrationNo = "1010100014" });
                students.Add(new Student() { StudentId = 20216, StudentName = "F", BirthDate = "04/11/2011", RegistrationNo = "1010100015" });
                students.Add(new Student() { StudentId = 20217, StudentName = "G", BirthDate = "05/10/1995", RegistrationNo = "1010100016" });
                context.Students.AddRange(students);
            }

            if (!context.Teachers.Any())
            {

                teachers.Add(new Teacher() { TeacherId = 101, TeacherName = "Amir", BirthDate = "02/10/1985", Salary = 101010101 });
                teachers.Add(new Teacher() { TeacherId = 102, TeacherName = "Sam", BirthDate = "11/11/1975", Salary = 101010103 });
                teachers.Add(new Teacher() { TeacherId = 103, TeacherName = "Selena", BirthDate = "04/05/1982", Salary = 101010102 });
                teachers.Add(new Teacher() { TeacherId = 104, TeacherName = "Adrian", BirthDate = "10/07/1991", Salary = 101010104 });
                teachers.Add(new Teacher() { TeacherId = 104, TeacherName = "Elena", BirthDate = "08/04/1990", Salary = 101010104 });
                context.Teachers.AddRange(teachers);
            }

            if (!context.TeacherEnrollments.Any())
            {
                teacherEnrollments.Add(new TeacherEnrollment() { TeacherEnrollmentId = 10001, TeacherId = 101, CourseId = 2002, SubjectId = 3001 });
                teacherEnrollments.Add(new TeacherEnrollment() { TeacherEnrollmentId = 10001, TeacherId = 101, CourseId = 2001, SubjectId = 3001 });
                teacherEnrollments.Add(new TeacherEnrollment() { TeacherEnrollmentId = 10001, TeacherId = 101, CourseId = 2001, SubjectId = 3002 });
                context.TeacherEnrollments.AddRange(teacherEnrollments);
            }

            if (!context.StudentEnrollments.Any())
            {
                studentEnrollments.Add(new StudentEnrollment() { StudentId = 20213, CourseId = 2001, SubjectId = 3001, GradeId = 111 });
                studentEnrollments.Add(new StudentEnrollment() { StudentId = 20211, CourseId = 2001, SubjectId = 3001, GradeId = 112 });
                studentEnrollments.Add(new StudentEnrollment() { StudentId = 20212, CourseId = 2002, SubjectId = 3002, GradeId = 112 });
                context.StudentEnrollments.AddRange(studentEnrollments);
            }
            

            base.Seed(context);


            


        }
    }

}
