using Microsoft.Extensions.Caching.Memory;

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
            .Setup(x => x.GetEntry<DateTime>(emailPayload.EmailPayloadId))
            .Returns(DateTime.Now);

        var emailService = Fixture.Freeze<Mock<ISendGridEmailService>>();

        var sut = Fixture.Create<EmailIdempotentService>();

        // act
        var result = await sut.SendAsync(emailPayload);

        // assert
        result.IsFaulted.Should().BeTrue();
        emailService.Verify(x => x.SendAsync(It.IsAny<IEmailPayload>()), Times.Never());
    }

    [Fact]
    public async Task SendAsync_OkResult_When_NotAlreadySent()
    {
        // arrange
        var emailPayload = Fixture.Create<EmailPayload>();

        var memoryCacheFacade = Fixture.Freeze<Mock<IMemoryCacheFacade>>();

        memoryCacheFacade
            .Setup(x => x.GetEntry<DateTime>(emailPayload.EmailPayloadId))
            .Returns(new Outcome<DateTime>(new KeyNotFoundException()));

        var emailService = Fixture.Freeze<Mock<ISendGridEmailService>>();

        var sut = Fixture.Create<EmailIdempotentService>();

        // act
        var result = await sut.SendAsync(emailPayload);

        // assert
        result.IsSuccess.Should().BeTrue();
        emailService.Verify(x => x.SendAsync(emailPayload), Times.Once());
        memoryCacheFacade
            .Verify(x =>
                x.Set(emailPayload.EmailPayloadId, It.IsAny<DateTime>(), It.IsAny<MemoryCacheEntryOptions>()),
                Times.Once
            );
    }
}
