using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;


namespace CSSD_project.Pages
{
    public class AdminModel : PageModel
    {
        public Guid currentUserId;
        public Guid? driverId = null;
        public string name;
        public dbEdit editor = new dbEdit();
        public string label = "Please enter an email address to show a table";
        public List<string[]> bills;
        public bool showUnpaid = true;
        public bool showPaid = false;
        public void OnGet()
        {
        }

        public void OnGetStart(Guid id)
        {
            currentUserId = id;
            name = editor.getNamebyId(id);
        }

        public void OnPostResult(Guid id)
        {
            currentUserId = id;
            name = editor.getNamebyId(id);
            string email = Request.Form["email"];
            driverId = editor.getUserIdbyEmail(email);
            bills = editor.getBillsbyUserId(driverId);
            label = "Unpaid Bills";
        }

        public void OnPostUnpaid(Guid id, Guid driverId)
        {
            showUnpaid = true;
            showPaid = false;
            currentUserId = id;
            this.driverId = driverId;
            name = editor.getNamebyId(id);
            bills = editor.getBillsbyUserId(driverId);
            label = "Unpaid Bills";
        }

        public void OnPostPaid(Guid id, Guid driverId)
        {
            showPaid = true;
            showUnpaid = false;
            currentUserId = id;
            this.driverId = driverId;
            name = editor.getNamebyId(id);
            bills = editor.getBillsbyUserId(driverId);
            label = "Paid Bills";
        }

        public void OnPostBoth(Guid id, Guid driverId)
        {
            showPaid = true;
            showUnpaid = true;
            currentUserId = id;
            this.driverId = driverId;
            name = editor.getNamebyId(id);
            bills = editor.getBillsbyUserId(driverId);
            label = "All Bills";
        }
    }
}
