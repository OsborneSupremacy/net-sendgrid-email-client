using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Services.Tests;

public class AttachmentAdapterTests
{
    [Fact]
    public async Task AddAsync_Expected_Behavior()
    {
        // arrange
        SendGridMessage msg = new();

        StoredAttachment attachment = new
        (
            Guid.NewGuid(),
            "VGVzdA==",
            "Test.txt",
            "text/plain"
        );

        AttachmentAdapter sut = new();

        // act
        await sut.AddAsync(msg, attachment);

        // assert
        msg.Attachments.Count.Should().Be(1);
        msg.Attachments.Single().Filename.Should().Be("Test.txt");
    }

    [Fact]
    public async Task AddAsync_Collection_Expected_Behavior()
    {
        // arrange
        SendGridMessage msg = new();

        var attachments = new List<IAttachment>() {
            new StoredAttachment(
                Guid.NewGuid(),
                "VGVzdA==",
                "Test1.txt",
                "text/plain"
            ),
            new StoredAttachment(
                Guid.NewGuid(),
                "VGVzdA==",
                "Test2.txt",
                "text/plain"
            )
        };

        AttachmentAdapter sut = new();

        // act
        await sut.AddAsync(msg, attachments!);

        // assert
        msg.Attachments.Count.Should().Be(2);
        msg.Attachments.First().Filename.Should().Be("Test1.txt");
    }
}
