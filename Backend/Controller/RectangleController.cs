﻿using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Backend.Controller;

[Authorize(AuthenticationSchemes = "ApiKey")]
[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowSpecificOrigins")]
public class RectangleController : ControllerBase
{
    private readonly IRectangleService _rectangleService;
    private readonly ILogger<RectangleController> _logger;

    public RectangleController(IRectangleService rectangleService, ILogger<RectangleController> logger)
    {
        _rectangleService = rectangleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Rectangle>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Getting all rectangles");
            var rectangles = await _rectangleService.GetAll();
            _logger.LogInformation("Successfully retrieved {Count} rectangles", rectangles.Count);
            return Ok(rectangles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rectangles");
            return StatusCode(500, new { error = "An error occurred while retrieving rectangles", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Rectangle>> Add(Rectangle rectangle)
    {
        try
        {
            var result = await _rectangleService.Add(rectangle);
            _logger.LogInformation($"Added rectangle: Color={rectangle.Color}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding rectangle: {ex.Message}");
            throw;
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        try
        {
            await _rectangleService.DeleteAll();
            _logger.LogInformation("All rectangles deleted.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting rectangles: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }

    }
}

