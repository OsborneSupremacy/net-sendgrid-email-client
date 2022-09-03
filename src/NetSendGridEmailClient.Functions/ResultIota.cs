using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetSendGridEmailClient.Functions;

/// <summary>
/// A result interface that contains the success / failure status, Http Status Code, and error details.
/// 
/// Makes it easier to pass around a more complex result without using a tuple.
/// </summary>
public interface IResultIota
{
    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public List<string> Messages { get; set; }
}

public record OkResultIota : ResultIota, IResultIota
{
    public OkResultIota() : base(true)
    {
    }
}

public record BadResultIota : ResultIota, IResultIota
{
    public BadResultIota(int statuscode, string message) : base(false, statuscode, message)
    {
    }

    public BadResultIota(int statuscode, List<string> messages) : base(false, statuscode, messages)
    {
    }
}

public record ResultIota : IResultIota, IActionResult
{
    public ResultIota(bool success)
    {
        Success = success;
        StatusCode = success ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
    }

    public ResultIota(bool success, int statuscode, string message)
    {
        Success = success;
        StatusCode = statuscode;
        Messages.Add(message);
    }

    public ResultIota(bool success, int statuscode, List<string> messages)
    {
        Success = success;
        StatusCode = statuscode;
        Messages.AddRange(messages);
    }

    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public List<string> Messages { get; set; } = new List<string>();

    public async Task ExecuteResultAsync(ActionContext context) =>
        await new ObjectResult(new { Messages })
        {
            StatusCode = StatusCode
        }
        .ExecuteResultAsync(context);
}
