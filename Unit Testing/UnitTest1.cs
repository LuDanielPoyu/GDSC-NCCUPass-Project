namespace NCCUPass.Tests;

using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using NCCUPass.Infrastructure.UnitOfWorks;
using NCCUPass.Share.Attributes;
using NCCUPass.Share.Extensions;
// help set up testing environment
using NCCUPass.Tests.Fixtures;
using Xunit;

//remind that this test belongs to the testing collection - BaseTestCollection
[Collection("BaseTestCollection")]
public class UnitTest1
{
    //serviceprovider is used for dependency injection (DI)
    private readonly IServiceProvider serviceProvider;
    //use injection then we don't have to instantiate a class
    [Inject] protected readonly IUnitOfWork UnitOfWork = null!;

    public UnitTest1(BaseTestFixture fixture)
    {
        //the serviceProvider returned has the configured MongoDB Database 
        serviceProvider = fixture.ServiceProvider;
        //purpose of this line is to ensure that the UnitTest1 class has access to the necessary dependencies it requires to run its tests. 
        serviceProvider.Inject(this);
    }

    [Fact]
    public async Task Test1Async()
    {

        string? userId = "64e0c49d1450cd53a62cc61c";
        ObjectId objUserId = new ObjectId(userId);
        // UnitOfWork provides access to the repositories
        // Repositories contain methods to interact with the MongoDB
        var userInfo = await UnitOfWork.Users.GetUserInfoAsync(objUserId);

        //Console.WriteLine(userInfo.AccountName);
        Assert.NotNull(userInfo);
    }
}