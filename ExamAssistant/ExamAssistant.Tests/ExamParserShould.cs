using System;
using System.Collections.Generic;
using System.IO;
using ExamAssistant.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExamAssistant.Tests
{
    [TestClass]
    public class ExamParserShould
    {

        private readonly string VALID_EXAMSET = @"examset.eaxml";
        private readonly string INVALID_EXAMSET = @"invalid.eaxml";
        private readonly string QUESTION_FORMAT_EXAMSET = @"question.eaxml";
        private readonly string SECTION_FORMAT_EXAMSET = @"section.eaxml";

        private IConfigurationReader _config;
        private readonly string DEFAULT_SECTION_INSTRUCTION;
        private readonly string DEFAULT_SECTION_TITLE;

        private readonly string DEFAULT_QUESTION;
        private readonly string DEFAULT_ANSWER;
        private readonly QuestionType DEFAULT_QUESTIONTYPE;

        public ExamParserShould()
        {
            _config = new WebConfigReader();
            DEFAULT_SECTION_INSTRUCTION = _config.GetSetting("DefaultSectionInstructions");
            DEFAULT_SECTION_TITLE = _config.GetSetting("DefaultSectionTitle");
            DEFAULT_QUESTION = _config.GetSetting("DefaultQuestion");
            DEFAULT_ANSWER = _config.GetSetting("DefaultAnswer");

            QuestionType questionType;
            Enum.TryParse(_config.GetSetting("DefaultQuestionType"), out questionType);
            DEFAULT_QUESTIONTYPE = questionType;
        }

        [TestMethod]
        public void Parse_ExamSet_To_Exam_Object_IfValidFile()
        {
            Exam exam = new Exam(VALID_EXAMSET);
            Assert.IsNotNull(exam);
            Assert.AreEqual("Exam 1", exam.Name);
            Assert.AreEqual("Math", exam.Subject);
            Assert.AreEqual("Quiz", exam.Type);

            Exam exam2 = new Exam(QUESTION_FORMAT_EXAMSET);
            Assert.AreEqual(10, exam2.Total);

        }

        [TestMethod]
        [ExpectedException(typeof (FileNotFoundException))]
        public void Return_Error_IfInvalidFile()
        {
            Exam exam = new Exam(INVALID_EXAMSET);
        }

        [TestMethod]
        public void Read_AllSectionNodes()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            Assert.AreEqual(10, exam.Sections.Count);
        }

        [TestMethod]
        public void Parse_Sections_With_Title()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            List<Section> sections = exam.Sections;

            Assert.AreEqual("All Enumeration Section Title", sections[0].Title);
            Assert.AreEqual("All Multiple Choice Section Title", sections[1].Title);
            Assert.AreEqual("All Selection Section Title", sections[2].Title);
            Assert.AreEqual("All Drag Drop Section Title", sections[3].Title);
            Assert.AreEqual("All Default Section Title", sections[4].Title);
            Assert.AreEqual("Mixed Section Title", sections[5].Title);

        }

        [TestMethod]
        public void Parse_Sections_Without_Title()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            List<Section> sections = exam.Sections;

            Assert.AreEqual(DEFAULT_SECTION_TITLE, sections[6].Title);
        }

        [TestMethod]
        public void Parse_Sections_With_Type()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            List<Section> sections = exam.Sections;

            Assert.AreEqual(QuestionType.Enumeration, sections[0].Type, "Enumeration Type should be Enumeration");
            Assert.AreEqual(QuestionType.MultipleChoice, sections[1].Type, "Multiple Choice Type should be MultipleChoice");
            Assert.AreEqual(QuestionType.Selection, sections[2].Type, "Selection Type should be Selection");
            Assert.AreEqual(QuestionType.DragDrop, sections[3].Type, "DragDrop Type should be DragDrop");
            Assert.AreEqual(QuestionType.Mixed, sections[5].Type, "Multiple Choice Type should be MultipleChoice");

            Assert.AreEqual(QuestionType.Enumeration, sections[6].Type, "Has Enumeration Type should be Enumeration");
            
            Assert.AreEqual(DEFAULT_QUESTIONTYPE, sections[8].Type, "Invalid Type should be Enumeration");
        }

        [TestMethod]
        public void Parse_Sections_Without_Type()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            Section noTypeSection = exam.Sections[4];

            Assert.AreEqual(DEFAULT_QUESTIONTYPE, noTypeSection.Type);
        }

        [TestMethod]
        public void Parse_Section_With_Instructions()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            Section sectionWithInstruction = exam.Sections[0];

            Assert.AreEqual("This is a sample instruction.", sectionWithInstruction.Instructions);
        }

        [TestMethod]
        public void Parse_Section_Without_Instructions()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            Section sectionWithoutInstruction = exam.Sections[1];

            Assert.AreEqual(DEFAULT_SECTION_INSTRUCTION, sectionWithoutInstruction.Instructions);
        }

        [TestMethod]
        public void Parse_Sections_Without_TypeTitleInstructions()
        {
            Exam exam = new Exam(SECTION_FORMAT_EXAMSET);
            Section noAttributeSection = exam.Sections[7];

            Assert.AreEqual(QuestionType.Enumeration, noAttributeSection.Type);
            Assert.AreEqual(DEFAULT_SECTION_TITLE, noAttributeSection.Title);
            Assert.AreEqual(DEFAULT_SECTION_INSTRUCTION, noAttributeSection.Instructions);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_With_QuestionAnswerChoicesPts()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //All attributes present
            Item item1 = section.Items[0];
            List<string> item1Answers = new List<string> { "red", "blue", "yellow" };
            List<string> item1Choices = new List<string>(){"Option 1", "Option 2"};
            Assert.AreEqual("Name the 3 primary colors.", item1.Question);
            CollectionAssert.AreEqual(item1Answers, item1.Answer);
            CollectionAssert.AreEqual(item1Choices, item1.Choices, "Item should not have choices");
            Assert.AreEqual(2, item1.Points);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_DifferentItemTypeDefined()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //All attributes present
            Item item1 = section.Items[0];
            Assert.AreEqual(section.Type, item1.Type);
            
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_Without_Answer()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //No answer provided in attributes
            Item item2 = section.Items[1];
            List<string> item2Answers = new List<string> { DEFAULT_ANSWER };
            CollectionAssert.AreEqual(item2Answers, item2.Answer, "Item should have DefaultAnswer");
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_Without_Question()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //No question provided in attributes
            Item item3 = section.Items[2];
            Assert.AreEqual(DEFAULT_QUESTION, item3.Question);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_Without_Choices()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            Item item2 = section.Items[1];
            List<string> item2Choices = new List<string>();
            CollectionAssert.AreEqual(item2Choices, item2.Choices, "Item should not have choices");
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_Without_QuestionAnswerChoices()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //No attributes provided
            Item item4 = section.Items[3];
            List<string> item4Choices = new List<string>();
            List<string> item4Answer = new List<string> { DEFAULT_ANSWER };
            Assert.AreEqual(DEFAULT_QUESTION, item4.Question);
            CollectionAssert.AreEqual(item4Answer, item4.Answer);
            CollectionAssert.AreEqual(item4Choices, item4.Choices);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsNonMixed_Without_QuestionAnswer()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section section = exam.Sections[0];

            //Has choices
            Item item5 = section.Items[4];
            List<string> item5Choices = new List<string> { "Option 1", "Option 2" };
            List<string> item5Answers = new List<string> { DEFAULT_ANSWER };
            Assert.AreEqual(DEFAULT_QUESTION, item5.Question);
            CollectionAssert.AreEqual(item5Choices, item5.Choices);
            CollectionAssert.AreEqual(item5Answers, item5.Answer);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsMixed_With_TypeQuestionAnswerChoices()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section mixedSection = exam.Sections[1];

            Item completeItem = mixedSection.Items[0];
            List<string> answers = new List<string>{"red", "blue", "yellow"};
            List<string> choices = new List<string> { "Option 1", "Option 2" };

            Assert.AreEqual("Name the 3 primary colors.", completeItem.Question);
            Assert.AreEqual(QuestionType.MultipleChoice, completeItem.Type);
            Assert.AreNotEqual(mixedSection.Type, completeItem.Type);
            CollectionAssert.AreEqual(answers, completeItem.Answer);
            CollectionAssert.AreEqual(choices, completeItem.Choices);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsMixed_Without_QuestionAnswer()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section mixedSection = exam.Sections[1];

            Item completeItem = mixedSection.Items[1];
            List<string> answers = new List<string>{DEFAULT_ANSWER};
            List<string> choices = new List<string> { "Option 1", "Option 2" };

            Assert.AreEqual(DEFAULT_QUESTION, completeItem.Question);
            Assert.AreEqual(QuestionType.Enumeration, completeItem.Type);
            CollectionAssert.AreEqual(answers, completeItem.Answer);
            CollectionAssert.AreEqual(choices, completeItem.Choices);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsMixed_Without_QuestionAnswerType()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section mixedSection = exam.Sections[1];

            Item completeItem = mixedSection.Items[2];
            List<string> answers = new List<string> { DEFAULT_ANSWER };
            List<string> choices = new List<string> { "Option 1", "Option 2" };

            Assert.AreEqual(DEFAULT_QUESTION, completeItem.Question);
            Assert.AreEqual(DEFAULT_QUESTIONTYPE, completeItem.Type);
            CollectionAssert.AreEqual(answers, completeItem.Answer);
            CollectionAssert.AreEqual(choices, completeItem.Choices);
        }

        [TestMethod]
        public void Parse_Questions_SectionIsMixed_EmptyItemNode()
        {
            Exam exam = new Exam(QUESTION_FORMAT_EXAMSET);
            Section mixedSection = exam.Sections[1];

            Item emptyItem = mixedSection.Items[3];
            Assert.AreEqual(DEFAULT_QUESTION, emptyItem.Question);
            Assert.AreEqual(DEFAULT_QUESTIONTYPE, emptyItem.Type);
            List<string> answer = new List<string> {DEFAULT_ANSWER};
            CollectionAssert.AreEqual(answer, emptyItem.Answer);
            List<string> choices = new List<string>();
            CollectionAssert.AreEqual(choices, emptyItem.Choices);
        }
       

    }
}
