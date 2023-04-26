using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiTennancy.Models;
using MultiTennancy.Servicios;
using System.Security.Claims;

namespace MultiTennancy.Controllers
{
    public class UsuariosController : Controller
    {


        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager; 

        public UsuariosController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager; 
            this.userManager = userManager; 
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            var claimsPersonalizados = new List<Claim>()
            {
                new Claim(Constantes.ClaimTenantId, usuario.Id),
            };

            await userManager.AddClaimsAsync(usuario, claimsPersonalizados);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();  
        }
    }
}
