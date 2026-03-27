namespace test;

using System.Transactions;
using Back_end.Util;
using DotNetEnv;
using Microsoft.Extensions.Configuration;

public class IntegrationTest
{
    protected TransactionScope transactionScope;
    protected IConfiguration config;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Env.Load("../../../Integration-tests/.env");
        config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
    }

    [SetUp]
    public virtual void Setup()
    {
        transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
    }

    [TearDown]
    public void Teardown()
    {
        transactionScope.Dispose();
    }
}