namespace ExamAssistant.Models
{
    public class User : DbRecord
    {
        private bool _isAdmin;

        public User()
        {
            GradeLevel = 0;
            Section = "No Section";
            IsActive = false;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public int GradeLevel { get; set; }
        public string Section { get; set; }

        public bool IsAdmin {
            get { return GradeLevel == 0 && Section.Equals("No Section"); }
            set { _isAdmin = value; }
        }

        public bool IsActive { get; set; }
    }
}