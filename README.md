## Project Description ##

Library containing functionality useful in tests.

TODO: Getting started

TODO: Describe development process: send pull requests to dev, master gets published after integration of PRs from dev

## TestFixture ##

A TestFixture is a base class for registering several test features. All SetUp and TearDown methods of registered test features will be correctly called for every test. At the moment we support xUnit, NUnit and MSTest frameworks.

```csharp
public class TestFixture_Test : TestFixture
{
  public TestFixture_Test()
  {
    Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
    Register(new AutoMockFeature());
  }

  [Fact]
  public void When_Providing_File__Should_Contain_Expected_Content()
  {
    var sut = Use<FileSandboxFeature>();
    
    var absoluteFile = sut.Sandbox.ProvideFile("testdata/Readme.txt");
	  
    File.Exists(absoluteFile).Should().BeTrue();
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

## Test Features ##

### FileSandboxFeature ###

The FileSandbox creates a temporary directory on your local environment for each test case. With given FileLocator's you can automatically resolve files from e.g. Assembly resources or a network share.

Provided FileLocator's:

* EmptyFileLocator
* ResourceFileLocator
* FolderBasedFileLocator
* TargetFolderBasedFileLocator

### AutoMockFeature ###

Automatically initialize [AutoFixture](https://github.com/AutoFixture/AutoFixture) with a mocking framework as [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy).

### AppConfigFeature ###

TODO

### SqlServerFeature ###

TODO 

### LocalDbContextFeature ###

TODO
