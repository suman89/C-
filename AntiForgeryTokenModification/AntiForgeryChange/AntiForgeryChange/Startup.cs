using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AntiForgeryChange.Startup))]
namespace AntiForgeryChange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
