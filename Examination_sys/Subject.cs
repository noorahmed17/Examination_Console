using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Examination_sys
{
   internal class Subject
    {
        public Subject(string name, int capacity = 10)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Subject Name is Not Valid");
            Name = name;
            EnrolledStudents = new Student[capacity];
            count = 0;
        }
        private int count = 0;
        public string Name { get; set; }
        public Student[] EnrolledStudents { get; set; }

        public void Enroll(Student std)
        {
            if(std == null)
                throw new ArgumentNullException(nameof(std));
            if (count == EnrolledStudents.Length)
                throw new Exception("Subject is FULL");
            EnrolledStudents[count++]= std;
        }

        public void NotifyStudents(Exam exam)
        {
            foreach(Student stud in EnrolledStudents)
            {
                exam.ExamStarted += stud.OnExamStarted;
            }
        }
    }
}
