using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flock.DAL;
using Flock.Models;
using Flock.ViewModels;
using NLog;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Flock.Controllers
{
    public class MembersController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private UnitOfWork<FlockDataContext> UnitOfWork = new UnitOfWork<FlockDataContext>();

        public MembersController()
        {

        }

        public MembersController(UserManager userManager)
        {
            UserManager = userManager;
        }

        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!UnitOfWork.Disposed)
                UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return RedirectToAction("members", "members");
        }

        public ActionResult Search(string SearchString, byte? MemberSegmentID)
        {
            var Model = UnitOfWork.Context.Member_Search(SearchString, MemberSegmentID).ToList();
            ViewData["SearchString"] = SearchString;

            return PartialView("~/Views/Shared/MemberSearchDataViewPartial.cshtml", Model);
        }

        public ActionResult MemberSearchPartial(string SearchString, byte? MemberSegmentID)
        {
            var Model = UnitOfWork.Context.Member_Search(SearchString, MemberSegmentID).ToList();

            ViewData["SearchString"] = SearchString;
            return PartialView("MemberSearchPartial", Model);
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                MemberViewModel MVM = null;
                if (UnitOfWork.GetRepository<Member>().GetAll().Where(p => p.MemberID == id).SingleOrDefault() != null)
                {
                    MemberViewModelBuilder Builder = new MemberViewModelBuilder(UnitOfWork);
                    MVM = Builder.Build(id);
                }
                else
                {
                    MVM = new MemberViewModel();
                    ViewData["InvalidMemberIDError"] = "Member ID " + id.ToString() + " does not exist.";
                }
                return View(MVM);
            }
            else
            {
                MemberViewModel MVM = new MemberViewModel();
                return View(MVM);
            }
        }

        public ActionResult MemberSelectorGridPartial()
        {
            return PartialView();
        }

        public ActionResult MemberDetailsPartial(int MemberID)
        {
            MemberDetailViewModelBuilder Builder = new MemberDetailViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(MemberID);
            return PartialView("MemberDetailsPartial", Model);
        }

        [HttpPost]
        public ActionResult ChangeMember(int MemberID)
        {
            MemberDetailViewModelBuilder Builder = new MemberDetailViewModelBuilder(UnitOfWork);
            var Model = Builder.Build(MemberID);
            return PartialView("MemberDetailsPartial", Model);
        }

        public ActionResult MemberAddressEditorPartial(long MemberAddressID)
        {
            var Model = new MemberViewModelBuilder(UnitOfWork).BuildAddressEditModel(MemberAddressID);

            return PartialView(Model);
        }

        public ActionResult BasketCountryComboBoxPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CountrySpecificBasket(int MemberID, short? CountryID)
        {
            var Model = UnitOfWork.Context.Member_Basket_Select(MemberID, CountryID).ToList();

            return PartialView("MemberBasketPartial", Model);
        }

        [HttpPost]
        public ActionResult ForceVisitorLogout(int MemberID, int VisitorID)
        {
            try
            {
                UnitOfWork.Context.Member_Logout(VisitorID);

                return PartialView("MemberDetailsPartial", new MemberDetailViewModelBuilder(UnitOfWork).Build(MemberID));
            }
            catch(Exception e)
            {
                Logger.Error(e, "ForceVisitorLogout Exception");
                Response.StatusCode = 500;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateMemberAddress(MemberAddressEditModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemberAddress ma = UnitOfWork.GetRepository<MemberAddress>().GetAll().Where(p => p.MemberAddressID == item.MemberAddressID).Single();

                    ma.FullName = item.FullName;
                    ma.AddressLine1 = item.AddressLine1;
                    ma.AddressLine2 = item.AddressLine2;
                    ma.Town = item.Town;
                    ma.County = item.County;
                    ma.Postcode = item.Postcode;
                    ma.CountryID = item.CountryID;
                    ma.PhoneNumber = item.PhoneNumber;

                    UnitOfWork.Save();

                    return PartialView("MemberDetailsPartial", new MemberDetailViewModelBuilder(UnitOfWork).Build(item.MemberID));
                }
                else
                {
                    return PartialView("MemberAddressEditorPartial", new MemberViewModelBuilder(UnitOfWork).BuildAddressEditModel(item.MemberAddressID));
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }

        public ActionResult MemberOverviewGridViewPartial(string SourceIDs)
        {
            short[] Sources = null;

            if (SourceIDs != null && SourceIDs != "")
                Sources = SourceIDs.Split(',').ToList().ConvertAll(p => Convert.ToInt16(p)).ToArray();


            return PartialView("_MemberOverviewGridViewPartial", new MemberOverviewSettings() { SourceIDs = Sources });
        }

        public ActionResult MemberOverviewUserSourceDropDownPartial()
        {
            List<Flock.Models.UserSource> Model = UnitOfWork.GetRepository<UserSource>().GetAll().Where(p => p.UserSourceID != 1).ToList();
            return PartialView("~/Views/Shared/SearchFilters/UserSourceDropDownPartial.cshtml", Model);
        }

        public ActionResult Admins()
        {

            return View();
        }

        public ActionResult AdminOverviewGridViewPartial(string RoleID)
        {
            return PartialView("_AdminOverviewGridViewPartial", new AdminOverviewSettings() { RoleID = RoleID });
        }

        public ActionResult AdminOverviewUserRoleDropDownPartial()
        {
            var Model = UnitOfWork.GetRepository<UserRole>().GetAll().Where(p => p.Name != "Member").ToList();
            return PartialView("~/Views/Shared/SearchFilters/UserRoleDropDownPartial.cshtml", Model);
        }

        public async Task<ActionResult> AdminOverviewGridAddNew(FlockUser user, string RoleID)//string RoleIDs)
        {
            //string[] Roles = null;

            //if (Roles != null && RoleIDs != "")
            //    Roles = RoleIDs.Split(',');

            if(ModelState.IsValid)
            {
                user.UserName = user.Email;
                //user.UserSourceID = 1;
                IdentityResult creationResult = UserManager.Create(user);
                if(creationResult.Succeeded)
                {
                    var addRolesResult = await UserManager.AddUserToRolesAsync(user.Id, new List<string>() { UnitOfWork.GetRepository<UserRole>().GetAll().Where(p => p.Id == RoleID).Select(p => p.Name).Single() });
                    if(!addRolesResult.Succeeded)
                    {
                        foreach (string s in addRolesResult.Errors)
                            ViewData["EditErrors"] += s + System.Environment.NewLine;
                    }
                    else
                    {
                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("confirmemail", "account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    }
                }
                else
                {
                    foreach(string s in creationResult.Errors)
                        ViewData["EditErrors"] += s + System.Environment.NewLine;
                }
            }

            return PartialView("_AdminOverviewGridViewPartial", new AdminOverviewSettings() { RoleID = RoleID });
        }

        public async Task<ActionResult> AdminOverviewGridViewCustomCommand(string commandType, string userId, string RoleID)
        {
            switch(commandType)
            {
                case "Unlock":
                    FlockUser u = UserManager.FindById(userId);
                    u.LockoutEndDateUtc = null;
                    u.AccessFailedCount = 0;
                    UserManager.Update(u);
                    break;

                case "PasswordReminder":
                    
                    if (ModelState.IsValid)
                    {
                        var user = await UserManager.FindByIdAsync(userId);
                        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                        {
                            // Don't reveal that the user does not exist or is not confirmed
                            return View("~/Views/Account/ForgotPasswordConfirmation.cshtml");
                        }

                        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");

                        ViewData["ResetPasswordSent"] = "Password Reset Email Sent";
                    }

                    break;

                default: break;
            }

            return PartialView("_AdminOverviewGridViewPartial", new AdminOverviewSettings() { RoleID = RoleID });
        }

        public ActionResult Stockists()
        {

            return View();
        }









        /* Bronto. */
        public ActionResult BadEmails()
        {
            return View("BadEmails/Index");
        }

        public ActionResult BadEmailGridViewPartial()
        {
            var Model = UnitOfWork.Context.InvalidEmail_Select().ToList();

            return PartialView("BadEmails/GridView", Model);
        }
    }

    public class AdminOverviewSettings
    {
        public string RoleID { get; set; }
    }

    public class MemberOverviewSettings
    {
        public short[] SourceIDs { get; set; }
    }
}
