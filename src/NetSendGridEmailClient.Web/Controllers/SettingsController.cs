using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetSendGridEmailClient.Web.Controllers
{
    [GoogleScopedAuthorize]
    [Authorize(Policy = "Admin")]
    public class SettingsController
    {
        private readonly SendGridSettings _sendGridSettings;

        public SettingsController(SendGridSettings sendGridSettings)
        {
            _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<SendGridDomainModel>), StatusCodes.Status200OK)]
        public IActionResult GetDomainModels() =>
            new OkObjectResult(
                _sendGridSettings
                .Domains
                .Select(x => new SendGridDomainModel()
                {
                    Domain = x.Domain,
                    DefaultUser = x.DefaultUser
                })
            );
    }
}
