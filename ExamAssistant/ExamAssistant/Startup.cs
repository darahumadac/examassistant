using System.Configuration;
using ExamAssistant.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExamAssistant.Startup))]
namespace ExamAssistant
{
    public partial class Startup
    {
        public static IRepository Repository;
        public static int RecordsPerPage;

        public void Configuration(IAppBuilder app)
        {
            Repository = new MySqlRepo();
            RecordsPerPage = int.Parse(ConfigurationManager.AppSettings["AdminPage_RowsDisplayedPerPage"]);

            ConfigureAuth(app);
        }
    }
}
