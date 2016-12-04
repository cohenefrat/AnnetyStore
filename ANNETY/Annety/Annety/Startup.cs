using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Annety.Startup))]
namespace Annety
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
