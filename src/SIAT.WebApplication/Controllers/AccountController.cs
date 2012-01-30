using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Web.Security;
using SIAT.Operations;
using SIAT.WebApplication.Models;
using Facebook.Web;

namespace SIAT.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private const string logoffUrl = "http://localhost:3434/";
        private const string redirectUrl = "http://localhost:3434/Account/OAuth";

        

        //
        // GET: /Account/FacebookLogOn/

        public ActionResult LogOn(string returnUrl)
        {
            return View();
        }


        //
        // GET: /Account/FacebookLogOn/
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateLogOn(model.UserName, model.Password))
                {
                    
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    // Make sure we only follow relative returnUrl parameters to protect against having an open redirector
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }

            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];
            
            if (siatOperations.Login(userName, password) == null)
            {
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return ModelState.IsValid;
        }

        //
        // GET: /Account/FacebookLogOn/
        
        public ActionResult FacebookLogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(redirectUrl);
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri);
        }

        //
        // GET: /Account/OAuth/
        
        public ActionResult OAuth(string code, string state)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
                    oAuthClient.RedirectUri = new Uri(redirectUrl);
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    DateTime expiresOn = DateTime.MaxValue;

                    if (tokenResult.ContainsKey("expires"))
                    {
                        DateTimeConvertor.FromUnixTime(tokenResult.expires);
                    }

                    FacebookWebClient fbClient = new FacebookWebClient(accessToken);
                    dynamic me = fbClient.Get("me?fields=id,name");
                    long facebookId = Convert.ToInt64(me.id);
                    
                    InMemoryUserStore.Add(new FacebookUser
                    {
                        AccessToken = accessToken,
                        Expires = expiresOn,
                        FacebookId = facebookId,
                        Name = (string)me.name,
                    });

                    FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);
                    

                    // prevent open redirection attack by checking if the url is local.
                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOff/

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            
            return Redirect(logoffUrl);
        }

    }
}