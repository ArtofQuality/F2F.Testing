## Project Description ##

Library containing functionality useful in tests.

TODO: Getting started

TODO: Describe development process: send pull requests to dev, master gets published after integration of PRs from dev

## TestFixture ##

A TestFixture is a base class for registering several test features. All SetUp and TearDown methods of registered test features will be correctly called for every test. At the moment we support xUnit, NUnit and MSTest frameworks.

### FileSandboxFeature ###

The FileSandbox creates a temporary directory on your local environment for each test case. With given FileLocator's you can automatically resolve files from e.g. Assembly resources or a network share.

Provided FileLocator's:

* EmptyFileLocator
* ResourceFileLocator
* FolderBasedFileLocator
* TargetFolderBasedFileLocator

Your benefits are:
* Parallel test execution
* Automatic cleanup of your file system
* With ResourceFileLocator: No special deployment of test data files, everything needed is inside your test assembly!
* With FolderBasedFileLocator: Large test data files can be used by everyone using a network share

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

Automatically initialize [AutoFixture](https://github.com/AutoFixture/AutoFixture) with a mocking framework as [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy).

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

Initialize EntityFramework using localdb data source, configured with a temporary file (using FileSandbox). With this parallel test execution with any given DbContext is possible.

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

### AppConfigFeature ###

TODO

### SqlServerFeature ###

TODO 
