using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Destuff.Shared;
using Destuff.Shared.Models;

namespace Destuff.Tests.Auth;

public class AuthRegisterRequestShould : IntegrationTestBase
{
    public AuthRegisterRequestShould() : base(HttpMethod.Post, ApiRoutes.AuthRegister)
    {
    }

    [Fact]
    public async Task Create_New_User()
    {
        // Arrange
        var model = new RegisterRequest { UserName = "user01", Password = "Qwer1234!" };

        // Act
        var result = await SendAsync<IdentityResultModel>(model);

        // Assert
        Assert.True(result?.Succeeded);
    }

    [Theory]
    [InlineData("Qw12!")]
    [InlineData("Qwer12345")]
    [InlineData("qwer1234!")]
    [InlineData("QwerQwer!")]
    public async Task Fail_Weak_Password(string password)
    {
        // Arrange
        var model = new RegisterRequest { UserName = "user01", Password = password };

        // Act
        var result = await SendAsync<IdentityResultModel>(model);

        // Assert
        Assert.False(result?.Succeeded);
    }

    [Fact]
    public async Task Fail_Existing_User()
    {
        // Arrange
        var model = new RegisterRequest { UserName = "user01", Password = "Qwer1234!" };
        await SendAsync(model); // register once

        // Act
        var result = await SendAsync<IdentityResultModel>(model); // register twice

        // Assert
        Assert.False(result?.Succeeded);
    }
}