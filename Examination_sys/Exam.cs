using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Examination_sys
{
    public enum ExamMode
    {
        Starting,
        Queued,
        Finished
    }
    internal abstract class Exam : ICloneable, IComparable<Exam>
    {
        public Exam(int t, Question[] _Questions, Subject sub) {
            if (t < 0)
                throw new ArgumentException("TIme should be Greater Than 0");
            Time = t;
            Questions = _Questions ?? throw new ArgumentNullException(nameof(_Questions));
            QuestionAnswerDictionary = new Dictionary<Question, Answer>();
            Subject = sub?? throw new ArgumentNullException(nameof(sub));
            Mode = ExamMode.Queued;
        }
        public int Time { get; set; }
        public int NumberOfQuestions => Questions.Length;
        public Question[] Questions { get; set; }
        public Dictionary<Question, Answer> QuestionAnswerDictionary { get; set; }
        public Subject Subject { get; set; }
        public ExamMode Mode { get; set; }
        public abstract void ShowExam();
        public virtual void Start()
        {
            Mode = ExamMode.Starting;
            Console.WriteLine("Exam Startred....");
            OnExamStarted(new ExamEventArgs(Subject, this));
            
        }
        public event EventHandler<ExamEventArgs> ExamStarted;
        protected virtual void OnExamStarted(ExamEventArgs e)
        {
            ExamStarted?.Invoke(this, e);
        }
        public virtual void Finish()
        {
            Mode = ExamMode.Finished;
            Console.WriteLine("Exam Finished....");
        }
        public int CorrectExam()
        {
            int total = 0;
            foreach(var DictQus in QuestionAnswerDictionary)
            {
                Question question = DictQus.Key;
                Answer studentAnswer = DictQus.Value;

                if (question.CheckAnswer(question.CorrectAnswer, studentAnswer))
                    total += DictQus.Key.Marks;
            }
            return total;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Time, Mode, NumberOfQuestions);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is Exam e)
            {
                return Time == e.Time &&
                       NumberOfQuestions == e.NumberOfQuestions;
            }
            return false;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public int CompareTo(Exam? other)
        {
            if (other == null) return 1;
            int com = Time.CompareTo(other.Time);
            if (com != 0)
                return com;
            return NumberOfQuestions.CompareTo(other.NumberOfQuestions);
        }
    }

    internal class PracticeExam : Exam
    {
        public PracticeExam(int t, Question[] _Questions, Subject subject) : base(t, _Questions, subject)
        {
        }
        public override void ShowExam()
        {
            Start();
            foreach(Question q in Questions)
            {
                q.Display();
                Console.Write("Enter answer number: ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) ||
                   choice < 1 || choice > q.Answers.Count)
                    {
                        Console.Write("Invalid input. Enter valid answer number: ");
                    }

                Answer studentAnswer = q.Answers[choice - 1];

                QuestionAnswerDictionary[q] = studentAnswer;
                Console.WriteLine();
            }

            Finish();
            Console.WriteLine("Student Report:");
            foreach(var entry in QuestionAnswerDictionary)
            {
                Console.WriteLine($"Question {entry.Key.Header}");
                Console.WriteLine($"Student Answer {entry.Value}");
                Console.WriteLine($"Correct Answer {entry.Key.CorrectAnswer}");
                Console.WriteLine();
            }
            Console.WriteLine($"Final Result: {CorrectExam()}");
        }
    }
    internal class FinalExam : Exam
    {
        public FinalExam(int t, Question[] _Questions, Subject subject) : base(t, _Questions, subject)
        {
        }

        public override void ShowExam()
        {
            Start();
            foreach (Question q in Questions)
            {
                q.Display();
                Console.Write("Enter answer number: ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) ||
                   choice < 1 || choice > q.Answers.Count)
                {
                    Console.Write("Invalid input. Enter valid answer number: ");
                }

                Answer studentAnswer = q.Answers[choice - 1];

                QuestionAnswerDictionary[q] = studentAnswer;
                Console.WriteLine();
            }

            Finish();
            Console.WriteLine("Student Answers:");
            foreach (var entry in QuestionAnswerDictionary)
            {
                Console.WriteLine($"Question {entry.Key.Header}");
                Console.WriteLine($"Student Answer {entry.Value}");
                Console.WriteLine();
            }
            //Console.WriteLine($"Final Result: {CorrectExam()}");
        }
    }
    internal class ExamEventArgs : EventArgs
    {
        public Subject Subject { get; }
        public Exam Exam { get; }

        public ExamEventArgs(Subject subject, Exam exam)
        {
            Subject = subject;
            Exam = exam;
        }
    }
}
