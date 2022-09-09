using NetSendGridEmailClient.Interface;

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
    [RequestSizeLimit(20971520)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
    public async Task<IResultIota> Upload(Guid emailPayloadId, IFormFile attachment)
    {
        if (attachment == null) // will be null when request exceeds limit
            return new BadResultIota(StatusCodes.Status413PayloadTooLarge, "File is too large. Limit is 20MB.");

        using var ms = new MemoryStream();
        attachment.CopyTo(ms);

        StoredAttachment storedAttachment = new
        (
            Guid.NewGuid(),
            Convert.ToBase64String(ms.ToArray()),
            attachment.FileName,
            attachment.ContentType
        );

        return await _attachmentStorageService
            .SaveAttachmentAsync(emailPayloadId, storedAttachment);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResultIota> Remove(Guid emailPayloadId, Guid attachmentId) =>
        await _attachmentStorageService
            .RemoveAttachmentAsync(emailPayloadId, attachmentId);

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IList<IAttachment>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNames(Guid emailPayloadId)
    {
        var result = await _attachmentStorageService
            .GetAttachmentCollectionAsync(emailPayloadId);

        return new OkObjectResult(
            result
                .GetAll()
                .Select(x => new { x.FileName, x.AttachmentId })
        );
    }
}
