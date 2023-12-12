using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Implement;
using System.Collections.Generic;
using System.Linq;

namespace NguyenTanTrungRazorPages.Pages.Carss
{
    public class IndexModel : PageModel
    {
        private ICarProducerRepo _carProducerRepo;
        public IndexModel()
        {
            _carProducerRepo = new CarProducerRepoImpl();
            
            var listProducer = _carProducerRepo.GetAll();
            producers = listProducer.ToList();
        }
        public List<CarProducer> producers { get; set; }
        public void OnGet()
        {
            string userLogin = HttpContext.Session.GetString("USERLOGIN");
            if (!IsAuthorized(userLogin))
            {
                Response.Redirect("/");
            }
        }
        public bool IsAuthorized(string email)
        {
            if (email == null)
            {
                return false;
            }

            IStaffAccountRepo staffRepo = new StaffAccountRepoImpl();
            var staff = staffRepo.getStaffAccountByEmail(email);
            if (staff == null)
            {
                return false;
            }
            return true;
        }
    }
}
