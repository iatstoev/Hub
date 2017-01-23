using System.Web.Mvc;

namespace Hub.Controllers
{
    public class FinanceController : Controller
    {

        public PartialViewResult DisplayAccounts()
        {
            //
            //TODO
            //create the accountModel. design it first
            //create the financeService that will fetch the model data
            //design the view, the actions the validation etc.

            return PartialView("Accounts");
        }

        public PartialViewResult DisplayDepots()
        {
            return PartialView("Depots");
        }

    }
}