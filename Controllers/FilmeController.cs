using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    
    private FilmeContext _context;
    private readonly IMapper _mapper;

    public FilmeController (FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RetornaFilme), 
            new {id = filme.Id}, filme);
    }

    [HttpGet]
    public IEnumerable<ReadFilmeDto>? RetornaFilmes([FromQuery] int skip = 0, 
        [FromQuery] int take = 10)
    {
        return _mapper.Map<List<ReadFilmeDto>>
            (_context.Filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RetornaFilme(int id)
    {
        var filme = _context.Filmes
            .FirstOrDefault(filme => filme.Id == id);
        if (filme == null) { return NotFound(); }
        return Ok(_mapper.Map<ReadFilmeDto>(filme));

    }

    [HttpPut("{id}")]
    public IActionResult AtualizandoFilme(int id, 
        UpdateFilmeDto filmedto)
    {
        Filme? filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) { return NotFound(); }
        _mapper.Map(filmedto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizandoFilmePatch(int id, 
        JsonPatchDocument<UpdateFilmeDto> patch)
    {
        Filme? filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) { return NotFound(); }

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) { return NotFound(); }

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
