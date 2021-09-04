using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BlazorCookieAuth.Server.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public string ReturnUrl { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string paramUsername, string paramPassword)
        {
            string returnUrl = Url.Content("~/");

            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            // *** !!! This is where you would validate the user !!! ***
            // In this example we just log the user in
            // (Always log the user in for this demo)
            try
            {
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyC17liZERnPsPKzQ_2LF0G6u3KA8RTMZO8"));
                var user = await auth.SignInWithEmailAndPasswordAsync(paramUsername, paramPassword);
                Console.WriteLine(JsonConvert.SerializeObject(user));
                if (user.User.IsEmailVerified)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, paramUsername),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        RedirectUri = this.Request.Host.Value
                    };

                    try
                    {
                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                        return Redirect("/");
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Redirect("/logincontrol");
        }
    }
}