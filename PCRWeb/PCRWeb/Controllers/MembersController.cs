using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PCRWeb.Security;
using PCRWeb.Services;
using PCRWeb.ViewModels;

namespace PCRWeb.Controllers
{
    public class MembersController : Controller
    {
        //宣告Members資料表的Service物件
        private readonly MembersDBService membersService = new MembersDBService();

        // GET: Members
        public ActionResult Index()
        {
            return View();
        }

        #region 註冊
        //註冊一開始顯示頁面
        public ActionResult Register()
        {
            //判斷使用者是否已經過登入驗證
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PrincessConnect");
            //已登入則重新導向
            //否則進入註冊畫面
            return View();
        }
        //傳入註冊資料的Action
        [HttpPost]
        //設定此Action只接受頁面POST資料傳入
        public ActionResult Register(MembersRegisterViewModel RegisterMember)
        {
            //判斷頁面資料是否都經過驗證
            if (ModelState.IsValid)
            {
                //將頁面資料中的密碼欄位填入
                RegisterMember.newMember.Password = RegisterMember.Password;
                //呼叫Serrvice註冊新會員
                membersService.Register(RegisterMember.newMember);
                //重新導向頁面
                return RedirectToAction("RegisterResult");
            }
            //未經驗證清空密碼相關欄位
            RegisterMember.Password = null;
            RegisterMember.PasswordCheck = null;
            //將資料回填至View中
            return View("Login",RegisterMember);
        }

        //註冊結果顯示頁面
        public ActionResult RegisterResult()
        {
            return RedirectToAction("Login", "Members");
        }

        //判斷註冊帳號是否已被註冊過Action
        public JsonResult AccountCheck(MembersRegisterViewModel RegisterMember)
        {
            //呼叫Service來判斷，並回傳結果
            return Json(membersService.AccountCheck(RegisterMember.newMember.Account),
                JsonRequestBehavior.AllowGet);
        }

        //接收驗證信連結傳進來的Action
        #endregion

        #region 登入
        //登入一開始載入畫面
        public ActionResult Login()
        {
            //判斷使用者是否已經過登入驗證
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PrincessConnect"); //已登入則重新導向
            return View();//否則進入登入畫面
        }
        //傳入登入資料的Action
        [HttpPost] //設定此Action只接受頁面POST資料傳入
        public ActionResult Login(MembersLoginViewModel LoginMember)
        {
            //使用Service裡的方法來驗證登入的帳號密碼
            string ValidateStr = membersService.LoginCheck(LoginMember.Account, LoginMember.Password);
            //判斷驗證後結果是否有錯誤訊息
            if (String.IsNullOrEmpty(ValidateStr))
            {
                //無錯誤訊息，則登入
                //先藉由Service取得登入者角色資料
                string RoleData = membersService.GetRole(LoginMember.Account);
                JwtService jwtService = new JwtService();
                //從Web.Config撈出資料
                //Cookie名稱
                string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
                string Token = jwtService.GenerateToken(LoginMember.Account, RoleData);
                ////產生一個Cookie
                HttpCookie cookie = new HttpCookie(cookieName);
                //設定單值
                cookie.Value = Server.UrlEncode(Token);
                //寫到用戶端
                Response.Cookies.Add(cookie);
                //設定Cookie期限
                Response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));
                //重新導向頁面
                return RedirectToAction("Index", "PrincessConnect");
            }
            else
            {
                //有驗證錯誤訊息，加入頁面模型中
                ModelState.AddModelError("", ValidateStr);
                //將資料回填至View中
                return View(LoginMember);
            }
        }
        #endregion

        #region 登出
        //登出Action
        [Authorize] //設定此Action須登入
        public ActionResult Logout()
        {
            //使用者登出
            //Cookie名稱
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
            //清除Cookie
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Values.Clear();
            Response.Cookies.Set(cookie);
            //重新導向至登入Action
            return RedirectToAction("Login");
        }
        #endregion
    }
}