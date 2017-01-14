using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExamAssistant.Startup))]
namespace ExamAssistant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
