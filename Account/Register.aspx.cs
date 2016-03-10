using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using MBTimeSheetWebApp.Models;
using System.Threading.Tasks;

namespace MBTimeSheetWebApp.Account
{
    public partial class Register : Page
    {
        
        public event EventHandler ClearButtonClick;

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(RegisterUser));
        }

        private async Task RegisterUser()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, Roles= {Role.SelectedValue} };
            if (manager != null)
            {
                IdentityResult result = manager.Create(user, Password.Text);
                if (result.Succeeded)
                {
                    string script = "alert(\"Sending confirmation email!\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    await manager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    IdentityHelper.SignIn(manager, user, isPersistent: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
        }

    }
}