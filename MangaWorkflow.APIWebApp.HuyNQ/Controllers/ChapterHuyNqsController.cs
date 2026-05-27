using Azure;
using MangaWorkflow.APIWebApp.HuyNQ.Commons;
using MangaWorkflow.Entities.HuyNQ.Models;
using MangaWorkflow.Services.HuyNQ;
using MangaWorkflow.Services.HuyNQ.DTOs.Chapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<List<ChapterHuyNq>> Get()
    {
        try
        {
            return await _chapterHuyNqService.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return [];
    }

    // GET api/<ChapterHuyNqsController>/5
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
            Console.WriteLine(ex.Message);
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
            Console.WriteLine(ex.Message);
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
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ChapterUpdateRequest request)
    {
        try
        {
            var existingChapter = await _chapterHuyNqService.GetByIdAsync(id) ?? throw new Exception("Chapter not found");
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
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the chapter.");
        }
    }

    // DELETE api/<ChapterHuyNqsController>/5
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
            Console.WriteLine(ex.Message);
            var apiResponse = new ApiResponse<string?>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Chapter deleted unsuccessfully",
                Data = null
            };
            return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }
}
