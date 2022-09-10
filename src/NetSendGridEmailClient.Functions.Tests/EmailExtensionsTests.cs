using System.Diagnostics.CodeAnalysis;
using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Functions.Tests;

[ExcludeFromCodeCoverage]
public class EmailExtensionsTests
{
    [Fact]
    public void AnyEmails_True_When_One_Valid_String()
    {
        // arrange
        List<string> input = new() { "hey@you.com", "", "     " };

        // act
        var result = input.AnyEmails();

        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AnyEmails_False_When_Input_Empty()
    {
        // arrange
        List<string> input = new();

        // act
        var result = input.AnyEmails();

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AnyEmails_False_When_No_Valid_Strings()
    {
        // arrange
        List<string> input = new() { "", "     " };

        // act
        var result = input.AnyEmails();

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToEmailList_Expected_Behavior()
    {
        // arrange
        List<string> input = new() { "", "hey@you.com", "hey@you.org" };

        List<EmailAddress> expectedResult = new() { 
            new EmailAddress("hey@you.com"),
            new EmailAddress("hey@you.org")
        };

        // act
        var result = input.ToEmailList();

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void ToEmail_Expected_Behavior()
    {
        // arrange
        var input = "hey@you.com";
        var expectedResult = new EmailAddress("hey@you.com");

        // act
        var result = input.ToEmail();

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }

}
