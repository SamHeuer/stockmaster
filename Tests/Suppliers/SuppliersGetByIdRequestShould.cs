namespace Destuff.Tests.Suppliers;

public class SuppliersGetByIdRequestShould : IntegrationTestBase
{
    public SuppliersGetByIdRequestShould() : base(HttpMethod.Get, ApiRoutes.Suppliers)
    {
    }

    [Fact]
    public async Task Get_Supplier_Details()
    {
        // Arrange
        var create = new SupplierRequest { ShortName = "supplier", Name = "Supplier 001" };
        var model = await AuthorizedSendAsync<SupplierModel>(create, HttpMethod.Post);

        // Act
        var result = await AuthorizedGetAsync<SupplierModel>($"{ApiRoutes.Suppliers}/{model?.Id}");

        // Assert
        Assert.NotNull(result?.Id);
        Assert.Equal(model?.Id, result?.Id);
    }

    [Fact]
    public async Task Fail_Nonexistent_Supplier_Details()
    {
        // Act
        var result = await AuthorizedGetAsync($"{ApiRoutes.Suppliers}/xxx");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}