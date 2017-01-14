using System;
using System.Configuration;
using ExamAssistant.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExamAssistant.Tests
{
    [TestClass]
    public class ConfigurationShould
    {
        private IConfigurationReader _config;
        public ConfigurationShould()
        {
            _config = new WebConfigReader();
        }

        [TestMethod]
        public void Have_Default_SectionName()
        {
            Assert.IsNotNull(_config.GetSetting("DefaultSectionTitle"));
        }

        [TestMethod]
        public void Have_Default_SectionInstructions()
        {
            Assert.IsNotNull(ConfigurationManager.AppSettings["DefaultSectionInstructions"]);
        }

        [TestMethod]
        public void Have_Default_Question()
        {
            Assert.IsNotNull(ConfigurationManager.AppSettings["DefaultQuestion"]);
        }

        [TestMethod]
        public void Have_Default_Answer()
        {
            Assert.IsNotNull(ConfigurationManager.AppSettings["DefaultAnswer"]);
        }

        [TestMethod]
        public void Have_Default_QuestionType()
        {
            Assert.IsNotNull(ConfigurationManager.AppSettings["DefaultQuestionType"]);
        }

        

    }
}
