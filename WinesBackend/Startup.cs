using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WinesBackend.Startup))]
namespace WinesBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
