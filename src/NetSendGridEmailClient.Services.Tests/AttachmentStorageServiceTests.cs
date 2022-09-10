using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;

namespace NetSendGridEmailClient.Services.Tests;

[ExcludeFromCodeCoverage]
public class AttachmentStorageServiceTests : TestBase
{
    [Fact]
    public async Task GetAttachmentsAsync_Expected_Behavior()
    {
        // arrange
        var emailPayloadId = Guid.NewGuid();

        var attachmentCollection = new AttachmentCollection();
        for (int i = 0; i < 10; i++)
            attachmentCollection.Add(Fixture.Create<StoredAttachment>());

        Fixture.Freeze<Mock<IMemoryCacheAdapter>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var sut = Fixture.Create<AttachmentStorageService>();

        // act
        var result = await sut.GetAttachmentCollectionAsync(emailPayloadId);

        // assert
        result.Should().Be(attachmentCollection);
    }

    [Fact]
    public async Task SaveAttachmentAsync_Expected_Behavior()
    {
        // arrange
        var emailPayloadId = Guid.NewGuid();

        var attachmentCollection = new AttachmentCollection();

        Fixture.Freeze<Mock<IMemoryCacheAdapter>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var attachment = Fixture.Create<StoredAttachment>();
        var notAddedAttachment = Fixture.Create<StoredAttachment>();

        var sut = Fixture.Create<AttachmentStorageService>();

        // act
        var result = await sut.SaveAttachmentAsync(emailPayloadId, attachment);

        // asssert
        result.Success.Should().BeTrue();
        attachmentCollection.GetAll().Contains(attachment);
        attachmentCollection.GetAll().Should().NotContain(notAddedAttachment);
    }
}
