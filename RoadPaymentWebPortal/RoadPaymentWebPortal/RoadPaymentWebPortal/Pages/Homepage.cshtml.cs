using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace CSSD_project.Pages
{
    public class HomepageModel : PageModel
    {

        public Guid currentUserId;
        public List<string[]> bills;
        public string name;
        public dbEdit editor = new dbEdit();
        public bool showUnpaid = true;
        public bool showPaid = false;
        public string label = "";
        public void OnGet()
        {
        }

        public void OnGetStart(Guid id)
        {
            currentUserId = id;          
            bills = editor.getBillsbyUserId(id);
            name = editor.getNamebyId(id);
            label = "Paid Bills";
        }

        public void OnPostUnpaid(Guid id)
        {
            showUnpaid = true;
            showPaid = false;
            currentUserId = id;
            bills = editor.getBillsbyUserId(id);
            name = editor.getNamebyId(id);
            label = "Unpaid Bills";
        }

        public void OnPostPaid(Guid id)
        {
            showPaid = true;
            showUnpaid = false;
            currentUserId = id;
            bills = editor.getBillsbyUserId(id);
            name = editor.getNamebyId(id);
            label = "Paid Bills";
        }

        public void OnPostBoth(Guid id)
        {
            showPaid = true;
            showUnpaid = true;
            currentUserId = id;
            bills = editor.getBillsbyUserId(id);
            name = editor.getNamebyId(id);
            label = "All Bills";
        }

        public RedirectToPageResult OnPostPay(Guid id)
        {
            return RedirectToPage("Payment", "Start", new { id = id });
        }
    }
}
