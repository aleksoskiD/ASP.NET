using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelWeb.Startup))]
namespace HotelWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
