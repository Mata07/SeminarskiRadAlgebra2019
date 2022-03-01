using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Aplikacija.Startup))]
namespace Aplikacija
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
