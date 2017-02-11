using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Xml;

namespace ExamAssistant.Models
{
    public class Exam
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<Section> Sections { get; set; }
        public string Type { get; set; }
        public int Total
        {
            get
            {
                int total = 0;
                foreach (Section section in Sections)
                {
                    section.Items.ForEach(i => total += i.Points);
                }
                return total;
            }
        }
    }

    public class Section
    {
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public string Instructions { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Question { get; set; }
        public List<string> Answer { get; set; }
        public List<Choice> Choices { get; set; }
        public QuestionType Type { get; set; }
        public int Points { get; set; }

    }

    public enum QuestionType
    {
        MultipleChoice,
        Enumeration,
        Selection,
        DragDrop,
        Mixed

    }
}