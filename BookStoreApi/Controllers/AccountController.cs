using BookStoreApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace BookStoreApi.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task< IHttpActionResult> Registeration(UserModel account)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ApplicationDBContext context = new ApplicationDBContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();
                user.UserName = account.Name;
                user.Name = account.Name;
                user.PasswordHash = account.Password;
                user.Address = account.Address;
                user.Email = account.Email;
                IdentityResult result = await manager.CreateAsync(user,account.Password);
                if(result.Succeeded)
                {
                    return Created("", "Register Success And User Name Is  :  " + user.UserName);
                }
                else
                {
                    return BadRequest((result.Errors.ToList())[0]);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
