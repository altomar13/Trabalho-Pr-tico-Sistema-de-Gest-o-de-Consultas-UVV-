using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestaoConsultasUVV.Data;
using SistemaGestaoConsultasUVV.Models;
using SistemaGestaoConsultasUVV.ViewModels;

namespace SistemaGestaoConsultasUVV.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AccountController(AppDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Register() => View(new RegistroUsuarioViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistroUsuarioViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        if (await _context.Usuarios.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
            return View(model);
        }

        var usuario = new Usuario
        {
            Nome = model.Nome.Trim(),
            Email = email,
            DataCadastro = DateTime.UtcNow
        };
        usuario.Senha = _passwordHasher.HashPassword(usuario, model.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Cadastro realizado com sucesso. Faça login para continuar.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);

        if (usuario is null || _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, model.Senha) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = model.LembrarDeMim });

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Consultas");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
