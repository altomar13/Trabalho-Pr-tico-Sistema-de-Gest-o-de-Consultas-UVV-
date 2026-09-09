using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestaoConsultasUVV.Data;
using SistemaGestaoConsultasUVV.Models;

namespace SistemaGestaoConsultasUVV.Controllers;

[Authorize]
public class ConsultasController : Controller
{
    private readonly AppDbContext _context;

    public ConsultasController(AppDbContext context) => _context = context;

    private int UsuarioAtualId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var consultas = await _context.Consultas
            .AsNoTracking()
            .Where(c => c.UsuarioId == UsuarioAtualId)
            .OrderBy(c => c.DataHora)
            .ToListAsync();

        return View(consultas);
    }

    [HttpGet]
    public IActionResult Create() => View(new Consulta { DataHora = DateTime.Now.AddDays(1).Date.AddHours(9) });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Consulta consulta)
    {
        ModelState.Remove(nameof(Consulta.UsuarioId));
        ModelState.Remove(nameof(Consulta.Usuario));

        if (!ModelState.IsValid) return View(consulta);

        consulta.UsuarioId = UsuarioAtualId;
        consulta.Usuario = null;
        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Consulta cadastrada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var consulta = await _context.Consultas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioAtualId);

        return consulta is null ? NotFound() : View(consulta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Consulta model)
    {
        if (id != model.Id) return NotFound();

        ModelState.Remove(nameof(Consulta.UsuarioId));
        ModelState.Remove(nameof(Consulta.Usuario));
        if (!ModelState.IsValid) return View(model);

        var consulta = await _context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioAtualId);

        if (consulta is null) return NotFound();

        consulta.Especialidade = model.Especialidade.Trim();
        consulta.DataHora = model.DataHora;
        consulta.Descricao = model.Descricao.Trim();
        await _context.SaveChangesAsync();

        TempData["Success"] = "Consulta atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var consulta = await _context.Consultas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioAtualId);

        return consulta is null ? NotFound() : View(consulta);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var consulta = await _context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioAtualId);

        if (consulta is null) return NotFound();

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Consulta excluída com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
