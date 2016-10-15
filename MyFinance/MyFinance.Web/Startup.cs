using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyFinance.Web.Startup))]
namespace MyFinance.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
