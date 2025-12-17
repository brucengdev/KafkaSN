using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QueueClient;
using Models;

namespace WebAPI.Controllers;

[ApiController]
public class PostsController: ControllerBase
{
    private IEventProducer _producer;
    public PostsController(IEventProducer producer)
    {
        _producer = producer;
    }

    [HttpPost("[controller]")]
    public async Task<ActionResult> CreatePost([FromBody] Post postModel)
    {
        postModel.SentTime = DateTime.Now;
        await _producer.ProduceAsync("createPost", JsonSerializer.Serialize(postModel));
        return Ok();
    }
}