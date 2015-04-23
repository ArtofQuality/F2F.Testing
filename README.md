## Project Description ##

Library containing functionality useful in tests. All libraries are provided via [NuGet](http://www.nuget.org/packages?q=f2f).

## TestFixture ##

A TestFixture is a base class for registering several test features. All SetUp and TearDown methods of registered test features will be correctly called for every test. At the moment we support xUnit, NUnit and MSTest frameworks.

TestFixture provides only two public methods: `Register` for registering new features by type (where a feature can be of any type) and `Use` for requesting a registered object by it's type.

*NuGet packages*:
* F2F.Testing.Xunit
* F2F.Testing.NUnit
* F2F.Testing.MSTest

**Why do I need this?**

You know the situation when your test methods become too complex? Wouldn't it be nice to extend your tests with re-usable features which get automatically initialized and destroyed as your tests do? Yes, you can use SetUp and TearDown methods of your unit testing framework - but what if I have features which I need in several test classes? Using a base class isn't a solution either, because we can only have one base class. We went through the pain of having class hierarchies that provided test functionality of different shape, but it just doesn't scale and needs a lot of maintenance.

The main idea of the `TestFixture` class is to have only one base class that just acts as an access point for a set of test features used in a single test class.

With TestFixture it is possible to extend a class with unlimited re-usable features! That makes our tests clean, improves the readability and menas less maintenance pain.

## Test Features ##

### FileSandboxFeature ###

Provide a [FileSandbox](https://github.com/ArtofQuality/F2F.Sandbox) for your test.

*NuGet packages*:
* F2F.Testing.Xunit.Sandbox
* F2F.Testing.NUnit.Sandbox
* F2F.Testing.MSTest.Sandbox

```csharp
public class FileSandboxFeature_Test : TestFixture
{
  public FileSandboxFeature_Test()
  {
    Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
  }

  [Fact]
  public void When_Providing_File__Then_File_Should_Exist()
  {
    var sut = Use<FileSandboxFeature>();
    
    var absoluteFile = sut.Sandbox.ProvideFile("testdata/Readme.txt");
	  
    File.Exists(file).Should().BeTrue();
  }
}
```

### AutoMockFeature ###

Provide [AutoFixture](https://github.com/AutoFixture/AutoFixture) initialized with a mocking framework as [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy) or [Moq](https://github.com/Moq/moq4).

*NuGet packages*:
* F2F.Testing.Xunit.FakeItEasy
* F2F.Testing.NUnit.FakeItEasy
* F2F.Testing.MSTest.Moq

```csharp
public class AutoMockFeature_Test : TestFixture
{
  public AutoMockFeature_Test()
  {
    Register(new AutoMockFeature());
  }

  [Fact]
  public void When_Create_Interface_With_Fixture__Should_Not_Be_Null()
  {
    var sut = Use<AutoMockFeature>();
    
    var f = sut.Fixture.Create<ISample>();
	  
    f.Should().NotBeNull();
  }
}
```

### LocalDbContextFeature ###

The `LocalDbFeature` provides a connection string to a localdb data source pointing to a temporary file (using `FileSandbox`). The `LocalDbContextFeature` initializes the [EntityFramework](https://entityframework.codeplex.com/) using this connection string. Now you can execute tests in parallel with any given DbContext.

*NuGet packages*:
* F2F.Testing.Xunit.EF
* F2F.Testing.NUnit.EF
* F2F.Testing.MSTest.EF

```csharp
public class LocalDbContextFeature_Test : TestFixture
{
  public LocalDbContextFeature_Test()
  {
    Register(new LocalDbContextFeature();
  }

  [Fact]
  public void When_Creating_Context__Should_Not_Be_Null()
  {
    var sut = Use<LocalDbContextFeature>();

    var ctx = sut.CreateContext<CustomerContext>();

    ctx.Should().NotBeNull();
  }
}
```

### PostgreSQLFeature ###

This feature targets integration tests against an existing PostgreSQL server. Instead of using an existing database this feature creates a temporary database on the server, provides the connection string and drops the database after test execution. Now you are able to execute tests in parallel without side effects. Furthermore you get a method to import test data by using a SQL dump file. A SQL dump file can easily be provided by using `FileSandboxFeature`.

*NuGet packages*:
* F2F.Testing.Xunit.Npgsql
* F2F.Testing.NUnit.Npgsql
* F2F.Testing.MSTest.Npgsql

```csharp
public class PostgreSQLFeature_Test : TestFixture
{
  public PostgreSQLFeature_Test()
  {
    Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
    Register(new PostgreSQLFeature("Server=postgres.local;Userid=postgres;Password=xxx"));
  }

  [Fact]
  public void When_Requesting_ConnectionString__Should_Not_Be_Null()
  {
    var sandbox = Use<FileSandboxFeature>();
    var sut = Use<PostgreSQLFeature>();

    sut.Import(sandbox.ProvideFile("testdata/data.sql"));

    sut.ConnectionString.Should().NotBeNull();
  }
}
```

### SqlServerFeature ###

As `PostgreSQLFeature` the `SqlServerFeature` provides a temporary database on an existing MS SQL Server.

*NuGet packages*:
* F2F.Testing.Xunit
* F2F.Testing.NUnit
* F2F.Testing.MSTest

### AppConfigFeature ###

With `AppConfigFeature` you are able to test different `app.config` files for your test assembly. The feature replaces the `app.config` during test execution. Different `app.config` files can easily be provided by using `FileSandboxFeature`.

*NuGet packages*:
* F2F.Testing.Xunit
* F2F.Testing.NUnit
* F2F.Testing.MSTest

```csharp
public class AppConfigFeature_Test : TestFixture
{
  public AppConfigFeature_Test()
  {
    Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
    Register(new AppConfigFeature());
  }

  [Fact]
  public void When_Installing_Different_AppConfig__Should_Be_Accessible()
  {
    var sandbox = Use<FileSandboxFeature>();
    var sut = Use<AppConfigFeature>();

    sut.Install(sandbox.ProvideFile("testdata/test.config"));

    // work with app.config data
  }
}
```
