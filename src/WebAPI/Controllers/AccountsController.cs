using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QueueClient;
using Models;

namespace WebAPI.Controllers;

[ApiController]
public class AccountsController: ControllerBase
{
    private IEventProducer _producer;
    public AccountsController(IEventProducer producer)
    {
        _producer = producer;
    }

    [HttpPost("[controller]")]
    public async Task<ActionResult> CreateAccount([FromBody] Account accountModel)
    {
        await _producer.ProduceAsync("createAccount", JsonSerializer.Serialize(accountModel));
        return Accepted();
    }
}