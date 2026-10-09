using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Revemar.Domain.Entities;
using Revemar.Domain.Enums;
using Revemar.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revemar.Web.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            // Armazena o termo pesquisado na ViewData para manter o texto na barra após o recarregamento
            ViewData["CurrentFilter"] = searchString;

            // 1. Monta a query base (mantendo a regra do Soft Delete que já fizemos)
            // Usamos AsQueryable() para podermos adicionar os filtros dinamicamente
            var query = _context.Veiculos
                .Where(v => v.Situacao != SituacaoEstoque.Inativo)
                .AsQueryable();

            // 2. Se o usuário digitou algo, adicionamos a condição de filtro (WHERE)
            if (!string.IsNullOrEmpty(searchString))
            {
                // Transformamos tudo em maiúsculo (ToUpper) para evitar problemas com Case Sensitive no Oracle 
                // Ex: pesquisar por "fiat" achará "Fiat", "FIAT", etc.
                searchString = searchString.ToUpper();

                query = query.Where(v =>
                    v.Marca.ToUpper().Contains(searchString) ||
                    v.Modelo.ToUpper().Contains(searchString));
            }

            // 3. Configurações de Paginação
            int pageSize = 10; // Quantidade de veículos por página
            int currentPage = pageNumber ?? 1; // Se a página for nula, assume a página 1

            // Conta o total de registros (após o filtro) para saber quantas páginas teremos
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // 4. Aplica a ordenação, pula (Skip) os registros das páginas anteriores e pega (Take) os da atual
            var veiculos = await query
                .OrderByDescending(v => v.Id)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 5. Envia as informações da paginação para a View através da ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;

            return View(veiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Ano,Cor,Preco,Tipo,Situacao,DataCadastro")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Veículo cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,Ano,Cor,Preco,Tipo,Situacao,DataCadastro")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                TempData["ErrorMessage"] = "Veículo não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Veículo atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                // EXCLUSÃO LÓGICA: Em vez de deletar, mudamos a situação para Inativo
                veiculo.Situacao = SituacaoEstoque.Inativo;

                _context.Update(veiculo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Veículo inativado (excluído) com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao tentar inativar o veículo.";
            }

            return RedirectToAction(nameof(Index));

        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
