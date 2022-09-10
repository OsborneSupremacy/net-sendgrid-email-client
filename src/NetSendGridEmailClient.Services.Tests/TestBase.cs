using System.Diagnostics.CodeAnalysis;

namespace NetSendGridEmailClient.Services.Tests;

[ExcludeFromCodeCoverage]
public abstract class TestBase
{
    public IFixture Fixture { get; set; }

    protected TestBase()
    {
        Fixture = new Fixture()
            .Customize(
                new AutoMoqCustomization()
                {
                    ConfigureMembers = true,
                    GenerateDelegates = true
                }
            );
    }
}
