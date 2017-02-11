using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ExamAssistant.Models
{
    public class MySqlRepo : IRepository
    {
        private string _connectionString;

        public MySqlRepo()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ExamAssistant"].ConnectionString;
        }

        public List<ExamInformation> GetExamList()
        {
            List<ExamInformation> examList = new List<ExamInformation>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("GetExamList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ExamInformation examInfo = new ExamInformation()
                        {
                            Id = reader.GetInt32("EXAM_ID"),
                            Name = reader.GetString("EXAM NAME"),
                            Subject = reader.GetString("SUBJECT NAME"),
                            ExamType = reader.GetString("EXAM TYPE"),
                            TotalPoints = reader.GetInt32("TOTAL POINTS"),
                            CreatedBy = reader.GetString("CREATED BY"),
                            CreatedDate = reader.GetDateTime("CREATED_DATE"),
                            UpdatedBy = reader.GetString("UPDATED BY"),
                            UpdatedDate = reader.GetDateTime("UPDATED_DATE")
                        };

                        examList.Add(examInfo);
                    }
                }
                connection.Close();
            }

            return examList;
        }

        public List<User> GetStudentList()
        {
            List<User> studentList = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("GetStudentList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        User studentInfo = new User()
                        {
                            Id = reader.GetInt32("USER_ID"),
                            Username = reader.GetString("USERNAME"),
                            FirstName = reader.GetString("FIRST_NAME"),
                            LastName = reader.GetString("LAST_NAME"),
                            GradeLevel = reader.GetInt32("GRADE_LEVEL"),
                            Section = reader.GetString("SECTION"),
                            CreatedBy = reader.GetString("CREATED BY"),
                            CreatedDate = reader.GetDateTime("CREATED_DATE"),
                            UpdatedBy = reader.GetString("UPDATED BY"),
                            UpdatedDate = reader.GetDateTime("UPDATED_DATE")
                        };

                        studentList.Add(studentInfo);
                    }
                }
                connection.Close();
            }

            return studentList;
        }

        public Exam GetExamById(string examId)
        {
            throw new NotImplementedException();
        }

        public List<StudentExam> GetExamsByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<LoginStatus, User> VerifyUser(string username, string password)
        {
            KeyValuePair<LoginStatus, User> loginResult = new KeyValuePair<LoginStatus, User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("VerifyUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userUsername", username);
                    command.Parameters.AddWithValue("@userPassword", password);

                    MySqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        loginResult = new KeyValuePair<LoginStatus, User>(LoginStatus.Fail, new UnauthorizedUser());
                    }
                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            Id = reader.GetInt32("USER_ID"),
                            Username = reader.GetString("USERNAME"),
                            Password = reader.GetString("PASSWORD"),
                            FirstName = reader.GetString("FIRST_NAME"),
                            LastName = reader.GetString("LAST_NAME"),
                            GradeLevel = reader.GetInt32("GRADE_LEVEL"),
                            Section = reader.GetString("SECTION"),
                            IsAdmin = reader.GetInt32("IS_ADMIN") == 1,
                            CreatedBy = reader.GetString("CREATED_BY"),
                            CreatedDate = reader.GetDateTime("CREATED_DATE"),
                            UpdatedBy = reader.GetString("UPDATED_BY"),
                            UpdatedDate = reader.GetDateTime("UPDATED_DATE")
                        };

                        loginResult = new KeyValuePair<LoginStatus, User>(LoginStatus.Success, user);
                    }
                }
                connection.Close();
            }

            return loginResult;
        }


        public RegisterStatus AddUser(string username, string password, string firstName, string lastName,
            int gradeLevel, string section, bool isAdmin)
        {
            RegisterStatus addUserStatus = RegisterStatus.NotStarted;
            MySqlConnection connection = new MySqlConnection(_connectionString);

            try
            {
                using (connection)
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("AddUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userUsername", username);
                        command.Parameters.AddWithValue("@userPassword", password);
                        command.Parameters.AddWithValue("@userFirstname", firstName);
                        command.Parameters.AddWithValue("@userLastname", lastName);
                        command.Parameters.AddWithValue("@userGradeLevel", gradeLevel);
                        command.Parameters.AddWithValue("@userSection", section);
                        command.Parameters.AddWithValue("@userIsAdmin", isAdmin);
                        command.Parameters.AddWithValue("@userCreatedBy", 3); //TODO: Implement get userid by username
                        command.Parameters.AddWithValue("@userUpdatedBy", 3);

                        command.ExecuteNonQuery();

                        addUserStatus = RegisterStatus.Success;
                    }
                    connection.Close();

                }
            }
            catch (MySqlException ex)
            {
                addUserStatus = RegisterStatus.Fail; //TODO: Implement other register status e.g. AlreadyExists
                if (ex.Number == 1062)
                {
                    addUserStatus = RegisterStatus.AlreadyExists;
                }
                
            }
            finally
            {
                connection.Close();
            }

            return addUserStatus;
        }
    }
}