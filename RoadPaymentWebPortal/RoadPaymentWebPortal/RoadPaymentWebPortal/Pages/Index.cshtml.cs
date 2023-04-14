using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSSD_project.Pages
{
    public class IndexModel : PageModel
    {

        public bool success = false;
        public int attempts = 0;

        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

     
        public void OnPost()
        {

        }

        public RedirectToPageResult OnPostLogin()
        {
            dbEdit dbEditor = new dbEdit();
            var email = Request.Form["email"];
            var password = Request.Form["pass"];

            bool found = dbEditor.checkLogIn(email, password);
    
            if (found)
            {
                int userType = dbEditor.getUserTypebyEmail(email);
                switch (userType)
                {
                    case 1:
                        return RedirectToPage("Homepage", "Start", new { id = dbEditor.getUserIdbyEmail(email) });
                    case 2:
                        return RedirectToPage("Adminpage", "Start", new { id = dbEditor.getUserIdbyEmail(email) });
                    default:
                        break;
                }
                
            }

            attempts++;
            return null;
        }

        
    }
}