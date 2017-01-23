using System.Threading.Tasks;
using Hub.DataAccess;
using Hub.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace Services
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<HubDbContext>(HubDbContext.Create);
            app.CreatePerOwinContext<UserService>(UserService.Create);

            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }


    //public class Role : IRole<string>
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //}

    //public class RoleService:RoleManager<Role>
    //{
    //    public RoleService(IRoleStore<Role> store):base(store)
    //    { }

    //    public static RoleService Create(IdentityFactoryOptions<RoleService> options, IOwinContext context)
    //    {
    //        var c = context.Get<HubDbContext>();
    //        RoleService manager = new RoleService(new RoleStore<Role>(c));

    //        return manager;
    //    }
    //}


    public class UserService:UserManager<HubUser>
    {
        public UserService(IUserStore<HubUser> store):base(store)
        {}

        public static UserService Create(IdentityFactoryOptions<UserService> options, IOwinContext context)
        {
            var c = context.Get<HubDbContext>();
            UserService manager = new UserService(new UserStore<HubUser>(c));

            return manager;
        }
    }
}
