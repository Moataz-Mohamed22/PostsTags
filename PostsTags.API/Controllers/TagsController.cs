
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsTags.Core.DTOs.Tags;
using PostsTags.Core.Entities;
using PostsTags.Core.Interfaces;

namespace PostsTags.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _repository;

    public TagsController(ITagRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("Get-All")]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _repository.GetAllAsync();

        return Ok(tags);
    }

    [HttpGet("Get-by-id/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tag = await _repository.GetByIdAsync(id);

        if (tag is null)
        {
            return NotFound(new
            {
                message = "Tag not found."
            });
        }

        return Ok(new
        {
            tag.Id,
            tag.Name
        });
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Create(TagRequest request)
    {
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new
            {
                message = "Tag name cannot be empty."
            });
        }

        var exists = await _repository.ExistsByNameAsync(name);

        if (exists)
        {
            return Conflict(new
            {
                message = "A tag with this name already exists."
            });
        }

        var tag = new Tag
        {
            Name = name
        };

        await _repository.AddAsync(tag);
        await _repository.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = tag.Id },
            new { tag.Id, tag.Name });
    }
}
