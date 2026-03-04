using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Examination_sys
{
    internal class Answer : IComparable<Answer>
    {
        public Answer(int id, string text)
        {
            if (id < 0)
                throw new ArgumentException("ID should be Greater Than 0");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text Should Not Be null or white space");
            Id = id;
            Text = text;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Text}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is Answer ans)
            {
                return Id == ans.Id && Text == ans.Text;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Text);
        }

        public int CompareTo(Answer? obj)
        {
            if (obj == null) return 0;
            return this.Id.CompareTo(obj.Id);
        }
    }
    internal class AnswerList
    {
        private List<Answer> answers;
        public AnswerList(List<Answer> ans)
        {
            answers = ans;
        }
        public int Count => answers.Count;
        public void Add(Answer ans)
        {
            if (ans == null) throw new ArgumentException("Answer should Not be null");
            answers.Add(ans);
        }
        public Answer GetById(int id)
        {
            foreach(Answer ans in answers)
            {
                if (ans.Id == id)
                    return ans;
            }
            throw new ArgumentException("Id Should be Greater Than 0");
        }

        public Answer this[int id]
        {
            get
            {
                if(id < 0 || id > answers.Count)
                    throw new ArgumentException("InValid ID");
                return answers[id];
            }
        }
    }
}
