using Microsoft.AspNetCore.Mvc;
using QueueClient;

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
    public async Task<ActionResult> CreateAccount()
    {
        await _producer.ProduceAsync("createAccount", "A new account is created");
        return Ok();
    }
}