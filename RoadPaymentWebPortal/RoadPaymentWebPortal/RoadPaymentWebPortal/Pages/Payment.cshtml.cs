using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CSSD_project.Pages
{
    public class PaymentModel : PageModel
    {

        public Guid currentId;
        public float total;
        dbEdit editor = new dbEdit();


        public void OnGet()
        {
        }

        public void OnGetStart(Guid id)
        {
            currentId = id;
            total = editor.getTotalPrice(id);
            total = total / 100;
        }

        public RedirectToPageResult OnPostPay(Guid id)
        {
            if (CardChecker())
            {
                editor.payBills(id);
                return RedirectToPage("Accepted");
            }
            return RedirectToPage("Payment", "Start", new { id = id });
        }

        public bool CardChecker()
        {
            var cardNum = Request.Form["cardnumber"];
            var expMonth = Request.Form["expmonth"];
            var expYear = Request.Form["expyear"];
            var cvv = Request.Form["cvv"];

            var cardCheck = new Regex(@"^[0-9]{16}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNum.ToString())) 
              return false;
            if (!cvvCheck.IsMatch(cvv.ToString())) 
              return false;
            if (!monthCheck.IsMatch(expMonth.ToString())) 
              return false;
            if (!yearCheck.IsMatch(expYear.ToString())) 
              return false;

            DateTime dt = DateTime.Now;

            if (int.Parse(expMonth) <= dt.Month && int.Parse(expYear) == dt.Year)
            {
                return false;
            }
            if (int.Parse(expYear) <= dt.Year)
            {
                return false;
            }

            return true;

        }

        
           
           
       


    }
}
