using Azure;
using MangaWorkflow.APIWebApp.HuyNQ.Commons;
using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Services.HuyNQ;
using MangaWorkflow.Services.HuyNQ.DTOs.Chapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MangaWorkflow.APIWebApp.HuyNQ.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChapterHuyNqsController(IChapterHuyNqService chapterHuyNqService) : ControllerBase
{
    private readonly IChapterHuyNqService _chapterHuyNqService = chapterHuyNqService;

    // GET: api/<ChapterHuyNqsController>
    [HttpGet]
    [Authorize]
    [EnableQuery]
    public async Task<List<ChapterHuyNq>> Get()
    {
        try
        {
            return await _chapterHuyNqService.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return [];
    }

    // GET api/<ChapterHuyNqsController>/5
    [Authorize(Roles = "1, 2")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var response = await _chapterHuyNqService.GetByIdAsync(id);
            var apiResponse = new ApiResponse<ChapterGetByIdResponse>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Chapter retrieved successfully",
                Data = response
            };
            return StatusCode(StatusCodes.Status200OK, apiResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var apiResponse = new ApiResponse<ChapterGetByIdResponse>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Chapter retrieved unsuccessfully",
                Data = null
            };
            return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }

    // POST api/<ChapterHuyNqsController>
    [Authorize(Roles = "1")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChapterCreateRequest request)
    {
        try
        {
            var response = await _chapterHuyNqService.CreateAsync(request);

            if (response > 0)
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Chapter created successfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status201Created, apiResponse);
            }
            else
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Chapter created unsuccessfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var apiResponse = new ApiResponse<string?>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Chapter created unsuccessfully",
                Data = null
            };
            return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }

    // PUT api/<ChapterHuyNqsController>/5
    [Authorize(Roles = "1")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ChapterUpdateRequest request)
    {
        try
        {
            var existingChapter = await _chapterHuyNqService.GetByIdAsync(id) ?? throw new InvalidDataException("Chapter is not found");
            var response = await _chapterHuyNqService.UpdateAsync(id, request);

            if (response > 0)
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Chapter updated successfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status200OK, apiResponse);
            } 
            else
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Chapter updated unsuccessfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the chapter.");
        }
    }

    // DELETE api/<ChapterHuyNqsController>/5
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var isDeleted = await _chapterHuyNqService.DeleteAsync(id);
            if (isDeleted)
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode= StatusCodes.Status200OK,
                    Message = "Chapter deleted successfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status200OK, apiResponse);
            }
            else
            {
                var apiResponse = new ApiResponse<string?>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Chapter deleted unsuccessfully",
                    Data = null
                };
                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var apiResponse = new ApiResponse<string?>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Chapter deleted unsuccessfully",
                Data = null
            };
            return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] ChapterSearchRequest request)
    {
        try
        {
            var response = await _chapterHuyNqService.SearchAsync(request);
            var apiResponse = new ApiResponse<List<ChapterHuyNq>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Chapters retrieved successfully",
                Data = response
            };
            return StatusCode(StatusCodes.Status200OK, apiResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            var apiResponse = new ApiResponse<List<ChapterHuyNq>>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Chapters retrieved unsuccessfully",
                Data = null
            };
            return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }
}
