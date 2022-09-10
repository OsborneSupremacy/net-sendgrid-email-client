using Microsoft.Extensions.Caching.Memory;
using NetSendGridEmailClient.Functions;

namespace NetSendGridEmailClient.Services.Tests;

[ExcludeFromCodeCoverage]
public class EmailIdempotentServiceTests : TestBase
{
    [Fact]
    public async Task SendAsync_BadResult_When_AlreadySent()
    {
        // arrange
        var emailPayload = Fixture.Create<EmailPayload>();

        Fixture.Freeze<Mock<IMemoryCacheFacade>>()
            .Setup(x => x.EntryExists<DateTime>(emailPayload.EmailPayloadId))
            .Returns((true, DateTime.Now));

        var emailService = Fixture.Freeze<Mock<ISendGridEmailService>>();

        var sut = Fixture.Create<EmailIdempotentService>();

        // act
        var result = await sut.SendAsync(emailPayload);

        // assert
        result.Should().BeOfType<BadResultIota>();
        emailService.Verify(x => x.SendAsync(It.IsAny<IEmailPayload>()), Times.Never());
    }

    [Fact]
    public async Task SendAsync_OkResult_When_NotAlreadySent()
    {
        // arrange
        var emailPayload = Fixture.Create<EmailPayload>();

        var memoryCacheFacade = Fixture.Freeze<Mock<IMemoryCacheFacade>>();

        memoryCacheFacade
            .Setup(x => x.EntryExists<DateTime>(emailPayload.EmailPayloadId))
            .Returns((false, default(DateTime)));

        var emailService = Fixture.Freeze<Mock<ISendGridEmailService>>();

        var sut = Fixture.Create<EmailIdempotentService>();

        // act
        var result = await sut.SendAsync(emailPayload);

        // assert
        result.Should().BeOfType<OkResultIota>();
        emailService.Verify(x => x.SendAsync(emailPayload), Times.Once());
        memoryCacheFacade
            .Verify(x =>
                x.Set(emailPayload.EmailPayloadId, It.IsAny<DateTime>(), It.IsAny<MemoryCacheEntryOptions>()),
                Times.Once
            );
    }

}
