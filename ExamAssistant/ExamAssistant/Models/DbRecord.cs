using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAssistant.Models
{
    public class DbRecord
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}