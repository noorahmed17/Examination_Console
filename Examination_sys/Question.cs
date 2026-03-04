using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Examination_sys
{
    internal abstract class Question
    {
        protected Question(string h, string b, int m, AnswerList ans, Answer corrAns)
        {
            if (string.IsNullOrWhiteSpace(h))
            {
                throw new ArgumentException("Header Cannot be null or white space.");
            }
            if (string.IsNullOrWhiteSpace(b))
            {
                throw new ArgumentException("Body Cannot be null or white space.");
            }
            if (m < 0)
            {
                throw new ArgumentException("Marks Should be Greater than 0.");
            }
            Header = h;
            Body = b;
            Marks = m;
            Answers = ans ?? throw new ArgumentException(nameof(ans));
            CorrectAnswer = corrAns ?? throw new ArgumentException(nameof(corrAns));
        }
        public string Header { get; set; }
        public string Body { get; set; }
        public int Marks { get; set; }
        //public string[] Answers { get; set; }
        //public string CorrectAnswer { get; set; }
        public AnswerList Answers { get; set; }
        public Answer CorrectAnswer { get; set; }
        public abstract void Display();
        public abstract bool CheckAnswer(Answer answer, Answer studentAnswer);
        public override string ToString()
        {
            return $"{Header}\n{Body}: ({Marks} point)";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj is Question qus)
            {
                return Header == qus.Header && Body == qus.Body && Marks == qus.Marks;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Body, Marks);
        }

    }
    internal class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string h, string b, int m, AnswerList ans, Answer corrAns) : base(h, b, m, 
            new AnswerList(new List<Answer>() { new Answer(1, "True"), new Answer(2, "False") }), corrAns){}

        public override bool CheckAnswer(Answer answer, Answer studentAnswer)
        {
            return answer.Equals(studentAnswer);
        }
        public override void Display()
        {
            Console.WriteLine(ToString());
            for (int i = 0; i < Answers.Count; i++)
                Console.WriteLine(Answers[i]);
        }
    }

    internal class ChooseOneQuestion : Question
    {
        public ChooseOneQuestion(string h, string b, int m, AnswerList ans, Answer corrAns) : base(h, b, m, ans, corrAns){ }
        public override bool CheckAnswer(Answer answer, Answer studentAnswer)
        {
            return answer.Equals(studentAnswer);
        }
        public override void Display()
        {
            Console.WriteLine(ToString());
            for (int i = 0; i < Answers.Count; i++)
                Console.WriteLine(Answers[i]);
        }
    }

    internal class ChooseAllQuestion : Question
    {
        private List<Answer> correctAns;
        public ChooseAllQuestion(string h, string b, int m, AnswerList ans, List<Answer> correctAns) : base(h, b, m, ans, correctAns[0])
        {
            this.correctAns = correctAns;
        }
        public override bool CheckAnswer(Answer answer, Answer studentAnswer)
        {
            throw new NotSupportedException("Use CheckAnswer(List<Answer>) for multiple answers.");
        }

        public bool CheckAnswer(List<Answer> stdCorrectAns)
        {
            if (stdCorrectAns.Count != correctAns.Count) return false;
            foreach(Answer ans in stdCorrectAns)
            {
                if (!correctAns.Contains(ans)) return false;
            }
            return true;
        }
        public override void Display()
        {
            Console.WriteLine(ToString());
            for (int i = 0; i < Answers.Count; i++)
                Console.WriteLine(Answers[i]);
        }
    }
    internal class QuestionList : List<Question>
    {
        //private List<Question> qus;
        private readonly string filename;
        public QuestionList(string _filemname)
        {
            if (string.IsNullOrEmpty(_filemname))
                throw new ArgumentException("FileName is Not valid");
            filename = _filemname;
        }
        public new void Add(Question q)
        {
            if (q == null)
                throw new ArgumentException("Quesyion should not be null");
            base.Add(q);
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine("-----------------------------------------");
                    writer.WriteLine($"Date: {DateTime.Now.ToShortTimeString()}");
                    writer.WriteLine(q.ToString());
                    writer.WriteLine("-----------------------------------------");
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }
    }

}
