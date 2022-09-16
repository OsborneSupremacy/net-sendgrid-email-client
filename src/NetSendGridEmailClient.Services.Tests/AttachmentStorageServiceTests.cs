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

        Fixture.Freeze<Mock<IMemoryCacheFacade>>()
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

        Fixture.Freeze<Mock<IMemoryCacheFacade>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var attachment = Fixture.Create<StoredAttachment>();
        var notAddedAttachment = Fixture.Create<StoredAttachment>();

        var sut = Fixture.Create<AttachmentStorageService>();

        // act
        var result = await sut.SaveAttachmentAsync(emailPayloadId, attachment);

        // asssert
        var success = result.Match
        (
            success =>
            {
                return true;
            },
            error =>
            {
                return false;
            }
        );

        success.Should().BeTrue();
        attachmentCollection.GetAll().Contains(attachment);
        attachmentCollection.GetAll().Should().NotContain(notAddedAttachment);
    }

    [Fact]
    public async Task RemoveAttachmentAsync_Should_Remove_Attachment_In_Collection()
    {
        // arrange
        var emailPayloadId = Guid.NewGuid();

        var attachmentToRemove = Fixture.Create<StoredAttachment>();

        var attachmentCollection = new AttachmentCollection();
        attachmentCollection.Add(attachmentToRemove);

        Fixture.Freeze<Mock<IMemoryCacheFacade>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var sut = Fixture.Create<AttachmentStorageService>();

        // act
        await sut.RemoveAttachmentAsync(
            emailPayloadId,
            attachmentToRemove.AttachmentId
        );

        // assert
        attachmentCollection.GetAll().Should().NotContain(attachmentToRemove);
    }


    [Fact]
    public async Task RemoveAttachmentAsync_Should_Not_Remove_Unaffected_Attachment()
    {
        // arrange
        var emailPayloadId = Guid.NewGuid();

        var attachmentToRemove = Fixture.Create<StoredAttachment>();
        var unaffectedAttachment = Fixture.Create<StoredAttachment>();

        var attachmentCollection = new AttachmentCollection();
        attachmentCollection.Add(attachmentToRemove);
        attachmentCollection.Add(unaffectedAttachment);

        Fixture.Freeze<Mock<IMemoryCacheFacade>>()
            .Setup(x => x.GetOrCreate<AttachmentCollection>(emailPayloadId, It.IsAny<MemoryCacheEntryOptions>()))
            .Returns(attachmentCollection);

        var sut = Fixture.Create<AttachmentStorageService>();

        // act
        await sut.RemoveAttachmentAsync(
            emailPayloadId,
            attachmentToRemove.AttachmentId
        );

        // assert
        attachmentCollection.GetAll().Should().Contain(unaffectedAttachment);
    }
}
