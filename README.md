## Project Description ##

Library containing functionality useful in tests.

TODO: Getting started

TODO: Describe development process: send pull requests to dev, master gets published after integration of PRs from dev

## TestFixture ##

A TestFixture is a base class for registering several test features. All SetUp and TearDown methods of registered test features will be correctly called for every test. At the moment we support xUnit, NUnit and MSTest frameworks.

TestFixture provides only two public methods: `Register` for registering new features (which can be at least every regular object) by type and `Use` for requesting a registered object by it's type.

**Why do I need this?**

You know the situation when your test methods become too complex? Wouldn't it be nice to extend your tests with re-usable features which get automatically initialized and destroyed as your tests do? Yes, you can use SetUp and TearDown methods of your unit testing framework - but what if I have features which I need in several test classes? Using a base class isn't a solution either, because I'm only able to use one of it.

With TestFixture you are able to extend your class with unlimited re-usable features! That makes you tests clean and improves the readability.

## FileSandbox ##

You certainly know legacy code which is hard to test because it uses direct file system access. You can start to abstract every access to file system so you are able to execute unit tests. But there are situations where you want to test the *real* work with files on the hard disc. Then we don't talk about unit tests anymore, because you have external dependencies and of course your tests will execute slower. We talk about integration tests.

What happens if you want to test code with real files? You might have problems with tests which change your test data, so you have to copy them before. Depending on your environment (think about continuous integration systems) there will be differences between relative and absolute paths. Executing tests in parallel can be a problem when working with same test data. Furthermore you have to think about cleaning up your environment after test execution, e.g. deleting temporary files etc.

A FileSandbox creates a temporary directory on your local environment for each test case. With given FileLocator's you can automatically resolve files from e.g. Assembly resources or a network share. Then you get the absolute path to the file independent from your environment.

For a unit testing framework this means you can execute tests even in parallel because every test will create it's own temporary directory. After test execution the temporary directory will be automatically deleted.

### EmptyFileLocator ###

That's the default for the FileSandbox. If you don't have to provide files because you only create new files or directories, then use this FileLocator.

```csharp
var sandbox = new FileSandbox();

// Creates an empty temporary file and returns the path to the file.
var tempFile = sandbox.CreateTempFile();
```

### ResourceFileLocator ###

With `ResourceFileLocator` you need no special deployment of test data files because everything needed is inside your test assembly. Just add files to your test assembly as *Embedded Resource* using the file properties. That's very useful for e.g. small text files.

```csharp
var sandbox = new FileSandbox(new ResourceFileLocator(GetType()));

// Locates the given file using the ResourceFileLocator, provides the file in the sandbox
// and returns the absolute path.
var absolutePath = sandbox.ProvideFile("testdata/test.txt");
```

The path `testdata/test.txt` is the path to your file inside your test assembly. The `ResourceFileLocator` will resolve it by using the given type in it's constructor. The resource name will be built by using type's namespace and the path like this: `type.Namespace + "." + Escape(path)`.

With `sandbox.Dispose()` the `FileSandbox` will delete the temporary directory and all it's containing files. Note that this will fail if your SUT didn't close file handles.

### FolderBasedFileLocator ###

The `FolderBasedFileLocator` is useful for large test data files which can be used by everyone using a network share. The FileSandbox will copy each required file to the local temporary directory. Additionally there is a `TargetFolderBasedFileLocator` which automatically points to your execution directory.

```csharp
var sandbox = new FileSandbox(new FolderBasedFileLocator("\\nas.local\\testdata"));

// Locates the given file using the FolderBasedFileLocator, provides the file in the sandbox
// and returns the absolute path.
var absolutePath = sandbox.ProvideFile("sample/test.txt");
```

This example will copy the file *\\\nas.local\testdata\sample\test.txt* to a local temporary directory. The returned absolute path will be something like *%TEMP%\\&lt;GUID&gt;\sample\test.txt*.

## Test Features ##

### FileSandboxFeature ###

Automatically initialize a FileSandbox.

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
