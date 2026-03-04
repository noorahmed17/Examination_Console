using System;
using System.Collections.Generic;

namespace Examination_sys
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Subject practiceSubject = new Subject("C# Practice");
            Subject finalSubject = new Subject("C# Final");

            Student std1 = new Student("Ahmed", 101);
            Student std2 = new Student("Ali", 102);

            practiceSubject.Enroll(std1);
            practiceSubject.Enroll(std2);

            finalSubject.Enroll(std1);
            finalSubject.Enroll(std2);

            AnswerList tfAnswers = new AnswerList(
                new List<Answer>
                {
                    new Answer(1, "True"),
                    new Answer(2, "False")
                });

            Question p1 = new TrueFalseQuestion(
                "P1",
                "C# is an OOP language?",
                5,
                tfAnswers,
                tfAnswers[0]);

            AnswerList chooseOneAnswers = new AnswerList(
                new List<Answer>
                {
                    new Answer(1, "Encapsulation"),
                    new Answer(2, "Abstraction"),
                    new Answer(3, "Inheritance"),
                    new Answer(4, "All of the above")
                });

            Question p2 = new ChooseOneQuestion(
                "P2",
                "Which is an OOP concept?",
                5,
                chooseOneAnswers,
                chooseOneAnswers[3]);

            Question[] practiceQuestions = { p1, p2 };

            AnswerList chooseAllAnswers = new AnswerList(
                new List<Answer>
                {
                    new Answer(1, "Polymorphism"),
                    new Answer(2, "Encapsulation"),
                    new Answer(3, "Recursion"),
                    new Answer(4, "Inheritance")
                });

            List<Answer> finalCorrectAnswers = new List<Answer>
            {
                chooseAllAnswers[0],
                chooseAllAnswers[1],
                chooseAllAnswers[3]
            };

            Question f1 = new ChooseAllQuestion(
                "F1",
                "Select all OOP concepts:",
                10,
                chooseAllAnswers,
                finalCorrectAnswers);

            Question f2 = new TrueFalseQuestion(
                "F2",
                "Recursion is a type of OOP concept?",
                5,
                tfAnswers,
                tfAnswers[1]);

            Question[] finalQuestions = { f1, f2 };

            QuestionList questionFile = new QuestionList("questions.txt");

            foreach (var q in practiceQuestions)
                questionFile.Add(q);

            foreach (var q in finalQuestions)
                questionFile.Add(q);

            PracticeExam practiceExam = new PracticeExam(30, practiceQuestions, practiceSubject);
            FinalExam finalExam = new FinalExam(60, finalQuestions, finalSubject);

            practiceExam.ExamStarted += std1.OnExamStarted;
            practiceExam.ExamStarted += std2.OnExamStarted;

            finalExam.ExamStarted += std1.OnExamStarted;
            finalExam.ExamStarted += std2.OnExamStarted;

            Console.WriteLine("Select Exam Type:");
            Console.WriteLine("1 - Practice");
            Console.WriteLine("2 - Final");

            string choice = Console.ReadLine();

            Exam selectedExam = null;

            switch (choice)
            {
                case "1":
                    selectedExam = practiceExam;
                    break;

                case "2":
                    selectedExam = finalExam;
                    break;

                default:
                    selectedExam = null;
                    break;
            }

            if (selectedExam == null)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            selectedExam.ShowExam();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}