using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinacialPortal2.Startup))]
namespace FinacialPortal2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
