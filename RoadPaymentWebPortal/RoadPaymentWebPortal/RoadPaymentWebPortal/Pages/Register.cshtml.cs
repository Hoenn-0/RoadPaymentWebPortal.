using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CSSD_project.Pages
{
    public class RegisterModel : PageModel
    {

        bool passMatch = true;
        bool emailValid = false;
        public void OnGet()
        {
        }

        public void OnPostRegister()
        {
            dbEdit editor = new dbEdit();
            var email = Request.Form["email"];
            var firstName = Request.Form["fName"];
            var lastName = Request.Form["lName"];
            var pass = Request.Form["pass"];

            //checks if the passwords match.
            if (pass != Request.Form["pass-repeat"])
            {
                passMatch = false;
            }

            //checks if email is valid. these checks are seperate from the if below as the bools allow for changes on the front end.
            if (new EmailAddressAttribute().IsValid(email.ToString()))
            {
                emailValid = true;
            }

            //if this fails it will be because the email is already registered.
            if(editor.InsertUser(email, firstName, lastName, pass, 1) && passMatch && emailValid)
            {
                Response.Redirect("Homepage");
            }
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
