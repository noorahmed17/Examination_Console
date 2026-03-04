using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Examination_sys
{
    internal class Student
    {
        public Student(string name, int id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (id <= 0)
                throw new ArgumentException("Invalid Id.");

            Name = name;
            Id = id;
        }
        public string Name { get; set; }
        public int Id { get; set; }
        //CALLBACK METHOD
        public void OnExamStarted(object? sender, ExamEventArgs e)
        {
            Console.WriteLine($"Student {Name} received notification:");
            //Console.WriteLine(e.Subject);
            Console.WriteLine($"Subject: {e.Subject.Name}");
            Console.WriteLine();
        }
        public override string ToString()
        {
            return $"{Id} - {Name}";
        }

    }
}
