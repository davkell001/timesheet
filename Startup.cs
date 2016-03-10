using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MBTimeSheetWebApp.Startup))]
namespace MBTimeSheetWebApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
