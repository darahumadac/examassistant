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
        private readonly string _examSetFilename;
        private List<Section> _sections;
        public List<Section> Sections { get { return _sections; } }

        public Exam(string examSetFilename)
        {
            _examSetFilename = examSetFilename;
            parseExam();
        }

        private void parseExam()
        {  
            XmlDocument examSet = new XmlDocument();
            examSet.Load(_examSetFilename);

            XmlNode examsetNode = examSet.GetElementsByTagName("examset")[0];
            parseSections(examsetNode.ChildNodes);
        }

        private void parseSections(XmlNodeList sectionNodeList)
        {
            _sections = new List<Section>();

            foreach (XmlNode section in sectionNodeList)
            {
                _sections.Add(new Section(section));
            }
        }
    }

    public class Section
    {
        private string _title;
        private QuestionType _sectionType;
        private string _instructions;
        private List<Item> _items;

        public Section(XmlNode section)
        {
            parseAttributes(section.Attributes);
            parseItems(section);
        }

        private void parseAttributes(XmlAttributeCollection sectionAttributes)
        {
            if (sectionAttributes != null)
            {
                _title = sectionAttributes["title"] != null
                    ? sectionAttributes["title"].InnerText
                    : ConfigurationManager.AppSettings["DefaultSectionTitle"];

                QuestionType questionType = QuestionType.Enumeration;
                if (sectionAttributes["type"] != null &&
                    Enum.IsDefined(typeof (QuestionType), sectionAttributes["type"].InnerText))
                {
                    Enum.TryParse(sectionAttributes.GetNamedItem("type").InnerText, out questionType);                    
                }
                _sectionType = questionType;

                _instructions = sectionAttributes["instructions"] != null
                    ? sectionAttributes.GetNamedItem("instructions").InnerText
                    : ConfigurationManager.AppSettings["DefaultSectionInstructions"];
            }
        }

        private void parseItems(XmlNode sectionNode)
        {
            _items = new List<Item>();
            XmlNodeList itemNodes = sectionNode.ChildNodes;
            foreach (XmlNode item in itemNodes)
            {
                _items.Add(new Item(item, _sectionType));
            }
        }

        public string Title { get { return _title; } }
        public QuestionType Type { get { return _sectionType; } }
        public string Instructions { get { return _instructions; } }
        public List<Item> Items { get { return _items; } }
    }

    public class Item
    {
        private readonly QuestionType _sectionType;
        private string _question;
        private List<string> _answers;
        private List<string> _choices;
        private QuestionType _questionType;

        public string Question { get { return _question;} }
        public List<string> Answer { get { return _answers;} }
        public List<string> Choices { get { return _choices;} }
        public QuestionType Type { get { return _questionType; } }

        public Item(XmlNode item, QuestionType sectionType)
        {
            _sectionType = sectionType;

            parseAttributes(item.Attributes);
            parseChoices(item.ChildNodes);
        }

        private void parseAttributes(XmlAttributeCollection itemAttributes)
        {
            if (itemAttributes != null)
            {
                _question = itemAttributes["question"] != null 
                    ? itemAttributes["question"].InnerText
                    : ConfigurationManager.AppSettings["DefaultQuestion"];

                _answers = itemAttributes["answer"] != null
                    ? itemAttributes["answer"].InnerText.Split(',').ToList()
                    : new List<string>{ConfigurationManager.AppSettings["DefaultAnswer"]};

                if (_sectionType != QuestionType.Mixed)
                {
                    _questionType = _sectionType;
                }
                else
                {
                    QuestionType questionType;
                    Enum.TryParse(ConfigurationManager.AppSettings["DefaultQuestionType"], out questionType);

                    if (itemAttributes["type"] != null &&
                        Enum.IsDefined(typeof (QuestionType), itemAttributes["type"].InnerText))
                    {
                        Enum.TryParse(itemAttributes["type"].InnerText, out questionType);                    
                    }
                    _questionType = questionType;

                }
            }
            
        }

        private void parseChoices(XmlNodeList choiceNodes)
        {
            _choices = new List<string>();
            foreach (XmlNode choice in choiceNodes)
            {
                _choices.Add(choice.InnerText);
            }
        }
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