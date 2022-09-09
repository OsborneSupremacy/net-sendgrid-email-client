using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services.Tests;

public class AttachmentStorageServiceTests
{
    [Fact]
    public async Task GetAttachmentsAsync_Expected_Behavior()
    {
        // arrange
        var emailPayloadId = Guid.NewGuid();

        var autoFixture = new Fixture()
            .Customize(new AutoMoqCustomization() { ConfigureMembers = true, GenerateDelegates = true });

        var attachmentCollection = new AttachmentCollection();
        for (int i = 0; i < 10; i++)
            attachmentCollection.Add(autoFixture.Create<StoredAttachment>());

        autoFixture.Freeze<Mock<IMemoryCacheAdapter>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var sut = autoFixture.Create<AttachmentStorageService>();

        // act
        var result = await sut.GetAttachmentCollectionAsync(emailPayloadId);

        // assert
        result.Should().Be(attachmentCollection);
    }
}
