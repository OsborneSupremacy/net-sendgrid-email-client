using System.Collections.Generic;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetSendGridEmailClient.Interface;
using NetSendGridEmailClient.Models;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]
[Authorize(Policy = "Admin")]
public class AttachmentController : Controller
{
    private readonly ILogger<EmailController> _logger;

    private readonly IAttachmentStorageService _attachmentStorageService;

    public AttachmentController(
        ILogger<EmailController> logger,
        IAttachmentStorageService attachmentStorageService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _attachmentStorageService = attachmentStorageService ?? throw new ArgumentNullException(nameof(attachmentStorageService));
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(Guid emailPayloadId, IFormFile attachment)
    {
        using var ms = new MemoryStream();
        attachment.CopyTo(ms);

        StoredAttachment storedAttachment = new
        (
            Guid.NewGuid(),
            Convert.ToBase64String(ms.ToArray()),
            attachment.FileName,
            attachment.ContentType
        );

        await _attachmentStorageService.SaveAttachmentAsync(emailPayloadId, storedAttachment);

        return new OkObjectResult(storedAttachment.AttachmentId);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IList<IAttachment>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNames(string emailPayloadId)
    {
        IList<IAttachment> result;

        if (!Guid.TryParse(emailPayloadId, out var emailPayloadId2))
            result = new List<IAttachment>();
        else
            result = await _attachmentStorageService.GetAttachmentsAsync(emailPayloadId2);

        return new OkObjectResult(
            result.Select(x => new { x.FileName, x.AttachmentId })
        );
    }
}
