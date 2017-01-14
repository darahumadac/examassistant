using System;
using System.Configuration;
using ExamAssistant.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExamAssistant.Tests
{
    [TestClass]
    public class ConfigurationShould
    {
        [TestMethod]
        public void Have_Default_SectionName()
        {
            Assert.IsNotNull(ExamConfiguration.GetSetting("DefaultSectionTitle"));
        }

        [TestMethod]
        public void Have_Default_SectionInstructions()
        {
            Assert.IsNotNull(ExamConfiguration.GetSetting("DefaultSectionInstructions"));
        }

        [TestMethod]
        public void Have_Default_Question()
        {
            Assert.IsNotNull(ExamConfiguration.GetSetting("DefaultQuestion"));
        }

        [TestMethod]
        public void Have_Default_Answer()
        {
            Assert.IsNotNull(ExamConfiguration.GetSetting("DefaultAnswer"));
        }

        [TestMethod]
        public void Have_Default_QuestionType()
        {
            Assert.IsNotNull(ExamConfiguration.GetSetting("DefaultQuestionType"));
        }

        

    }
}
