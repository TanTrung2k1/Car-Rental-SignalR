using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Implement;
using Repository;

namespace NguyenTanTrungRazorPages.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        private IStaffAccountRepo _staffAccountRepo;
        private ICustomerRepo _customerRepo;

        public LoginModel()
        {
            _staffAccountRepo = new StaffAccountRepoImpl();
            _customerRepo = new CustomerRepoImpl();
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            
            StaffAccount checkAdmin = _staffAccountRepo.checkAdminLogin(Email, Password);
            StaffAccount checkStaff = _staffAccountRepo.checkStaffLogin(Email, Password);
            Customer checkCustomer = _customerRepo.checkCustomerLogin(Email, Password);
            if (checkAdmin != null)
            {
                HttpContext.Session.SetString("USERLOGIN", checkAdmin.Email);
                return RedirectToPage("Admin/Index");
            }
            else if (checkStaff != null)
            { 
                HttpContext.Session.SetString("USERLOGIN", checkStaff.Email);
                return RedirectToPage("Staff/Index");
            }
            else if(checkCustomer != null)
            {
                HttpContext.Session.SetString("USERLOGIN", checkCustomer.CustomerEmail);
                return RedirectToPage("Customers/Index");
            }
            else{
                ErrorMessage = "Invalid username or password";
                return Page();
            }
        }
    }
}
