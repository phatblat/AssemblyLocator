using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Tasks;
using Microsoft.Build.Shared;
using System.IO;
using Microsoft.Build.Utilities;
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.build.utilities.toollocationhelper?view=msbuild-17-netcore
//using Microsoft.Build.Utilities.ToolLocationHelper;
using FrameworkNameVersioning = System.Runtime.Versioning.FrameworkName;
using Xunit;

//namespace Microsoft.Build.UnitTests;


//string targetFrameworkMoniker = ".NetFramework, Version=v6.0";
string targetFrameworkMoniker = "Xamarin.iOS, Version=v15.8";

GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
getReferencePaths.Execute();
string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;


Console.WriteLine(returnedPaths);

/*
sealed public class GetReferenceAssmeblyPath_Tests
{
    [Fact]
    public void TestGeneralFrameworkMonikerGood()
    {
        string targetFrameworkMoniker = ".NetFramework, Version=v4.5";
        MockEngine engine = new MockEngine();
        GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
        getReferencePaths.BuildEngine = engine;
        getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
        getReferencePaths.Execute();
        string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
        Assert.Equal(ToolLocationHelper.GetPathToReferenceAssemblies(new FrameworkNameVersioning(targetFrameworkMoniker)).Count, returnedPaths.Length);
        Assert.Equal(0, engine.Errors);
    }

    [Fact]
    public void TestGeneralFrameworkMonikerGoodWithRoot()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), "TestGeneralFrameworkMonikerGoodWithRoot");
        string framework41Directory = Path.Combine(tempDirectory, "MyFramework\\v4.1\\");
        string redistListDirectory = Path.Combine(framework41Directory, "RedistList");
        string redistListFile = Path.Combine(redistListDirectory, "FrameworkList.xml");
        try
        {
            Directory.CreateDirectory(framework41Directory);
            Directory.CreateDirectory(redistListDirectory);
            string redistListContents =
                    "<FileList Redist='Microsoft-Windows-CLRCoreComp' Name='.NET Framework 4.1'>" +
                        "<File AssemblyName='System.Xml' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                            "<File AssemblyName='Microsoft.Build.Engine' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                    "</FileList >";
            File.WriteAllText(redistListFile, redistListContents);
            string targetFrameworkMoniker = "MyFramework, Version=v4.1";
            MockEngine engine = new MockEngine();
            GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
            getReferencePaths.BuildEngine = engine;
            getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
            getReferencePaths.RootPath = tempDirectory;
            getReferencePaths.Execute();
            string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
            string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
            Assert.Equal(1, returnedPaths.Length);
            Assert.True(returnedPaths[0].Equals(framework41Directory, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(0, engine.Log.Length);
            Assert.True(displayName.Equals(".NET Framework 4.1", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (Directory.Exists(framework41Directory))
            {
                Directory.Delete(framework41Directory, true);
            }
        }
    }

    [Fact]
    public void TestGeneralFrameworkMonikerGoodWithRootWithProfile()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), "TestGeneralFrameworkMonikerGoodWithRootWithProfile");
        string framework41Directory = Path.Combine(tempDirectory, "MyFramework\\v4.1\\Profile\\Client");
        string redistListDirectory = Path.Combine(framework41Directory, "RedistList");
        string redistListFile = Path.Combine(redistListDirectory, "FrameworkList.xml");
        try
        {
            Directory.CreateDirectory(framework41Directory);
            Directory.CreateDirectory(redistListDirectory);
            string redistListContents =
                    "<FileList Redist='Microsoft-Windows-CLRCoreComp' Name='.NET Framework 4.1 Client'>" +
                        "<File AssemblyName='System.Xml' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                            "<File AssemblyName='Microsoft.Build.Engine' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                    "</FileList >";
            File.WriteAllText(redistListFile, redistListContents);
            FrameworkNameVersioning name = new FrameworkNameVersioning("MyFramework", new Version("4.1"), "Client");
            string targetFrameworkMoniker = name.FullName;
            MockEngine engine = new MockEngine();
            GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
            getReferencePaths.BuildEngine = engine;
            getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
            getReferencePaths.RootPath = tempDirectory;
            getReferencePaths.Execute();
            string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
            string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
            Assert.Equal(1, returnedPaths.Length);
            Assert.True(returnedPaths[0].Equals(framework41Directory + "\\", StringComparison.OrdinalIgnoreCase));
            Assert.True(displayName.Equals(".NET Framework 4.1 Client", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (Directory.Exists(framework41Directory))
            {
                Directory.Delete(framework41Directory, true);
            }
        }
    }

    [Fact]
    public void TestGeneralFrameworkMonikerNull()
    {
        MockEngine engine = new MockEngine();
        GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
        getReferencePaths.BuildEngine = engine;
        getReferencePaths.TargetFrameworkMoniker = null;
        getReferencePaths.Execute();
        string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
        Assert.Null(getReferencePaths.TargetFrameworkMonikerDisplayName);
        Assert.Equal(0, returnedPaths.Length);
        Assert.Equal(1, engine.Errors);
    }

    [Fact]
    public void TestGeneralFrameworkMonikerNonExistent()
    {
        MockEngine engine = new MockEngine();
        GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
        getReferencePaths.BuildEngine = engine;
        getReferencePaths.TargetFrameworkMoniker = ".NetFramewok, Version=v99.0";
        bool success = getReferencePaths.Execute();
        Assert.False(success);
        string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
        Assert.Equal(0, returnedPaths.Length);
        string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
        Assert.Null(displayName);
        FrameworkNameVersioning frameworkMoniker = new FrameworkNameVersioning(getReferencePaths.TargetFrameworkMoniker);
        string message = ResourceUtilities.FormatResourceString("GetReferenceAssemblyPaths.NoReferenceAssemblyDirectoryFound", frameworkMoniker.ToString());
        engine.AssertLogContains("ERROR MSB3644: " + message);
    }

    [Fact]
    public void TestGeneralFrameworkMonikerNonExistentOverrideError()
    {
        MockEngine engine = new MockEngine();
        GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
        getReferencePaths.BuildEngine = engine;
        getReferencePaths.TargetFrameworkMoniker = ".NetFramewok, Version=v99.0";
        try
        {
            Environment.SetEnvironmentVariable("MSBUILDWARNONNOREFERENCEASSEMBLYDIRECTORY", "1");
            bool success = getReferencePaths.Execute();
            Assert.True(success);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MSBUILDWARNONNOREFERENCEASSEMBLYDIRECTORY", null);
        }
        string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
        Assert.Equal(0, returnedPaths.Length);
        string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
        Assert.Null(displayName);
        FrameworkNameVersioning frameworkMoniker = new FrameworkNameVersioning(getReferencePaths.TargetFrameworkMoniker);
        string message = ResourceUtilities.FormatResourceString("GetReferenceAssemblyPaths.NoReferenceAssemblyDirectoryFound", frameworkMoniker.ToString());
        engine.AssertLogContains("WARNING MSB3644: " + message);
    }

    [Fact]
    public void TestGeneralFrameworkMonikerGoodWithInvalidIncludePath()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), "TestGeneralFrameworkMonikerGoodWithInvalidIncludePath");
        string framework41Directory = Path.Combine(tempDirectory, "MyFramework\\v4.1\\");
        string redistListDirectory = Path.Combine(framework41Directory, "RedistList");
        string redistListFile = Path.Combine(redistListDirectory, "FrameworkList.xml");
        try
        {
            Directory.CreateDirectory(framework41Directory);
            Directory.CreateDirectory(redistListDirectory);
            string redistListContents =
                    "<FileList Redist='Microsoft-Windows-CLRCoreComp' IncludeFramework='..\\Mooses' Name='Chained oh noes'>" +
                        "<File AssemblyName='System.Xml' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                            "<File AssemblyName='Microsoft.Build.Engine' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                    "</FileList >";
            File.WriteAllText(redistListFile, redistListContents);
            string targetFrameworkMoniker = "MyFramework, Version=v4.1";
            MockEngine engine = new MockEngine();
            GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
            getReferencePaths.BuildEngine = engine;
            getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
            getReferencePaths.RootPath = tempDirectory;
            getReferencePaths.Execute();
            string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
            Assert.Equal(0, returnedPaths.Length);
            string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
            Assert.Null(displayName);
            FrameworkNameVersioning frameworkMoniker = new FrameworkNameVersioning(getReferencePaths.TargetFrameworkMoniker);
            string message = ResourceUtilities.FormatResourceString("GetReferenceAssemblyPaths.NoReferenceAssemblyDirectoryFound", frameworkMoniker.ToString());
            engine.AssertLogContains(message);
        }
        finally
        {
            if (Directory.Exists(framework41Directory))
            {
                Directory.Delete(framework41Directory, true);
            }
        }
    }

    [Fact]
    public void TestGeneralFrameworkMonikerGoodWithInvalidCharInIncludePath()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), "TestGeneralFrameworkMonikerGoodWithInvalidCharInIncludePath");
        string framework41Directory = Path.Combine(tempDirectory, "MyFramework\\v4.1\\");
        string redistListDirectory = Path.Combine(framework41Directory, "RedistList");
        string redistListFile = Path.Combine(redistListDirectory, "FrameworkList.xml");
        try
        {
            Directory.CreateDirectory(framework41Directory);
            Directory.CreateDirectory(redistListDirectory);
            string redistListContents =
                    "<FileList Redist='Microsoft-Windows-CLRCoreComp' IncludeFramework='v4.*' Name='Chained oh noes'>" +
                        "<File AssemblyName='System.Xml' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                            "<File AssemblyName='Microsoft.Build.Engine' Version='2.0.0.0' PublicKeyToken='b03f5f7f11d50a3a' Culture='Neutral' FileVersion='2.0.50727.208' InGAC='true' />" +
                    "</FileList >";
            File.WriteAllText(redistListFile, redistListContents);
            string targetFrameworkMoniker = "MyFramework, Version=v4.1";
            MockEngine engine = new MockEngine();
            GetReferenceAssemblyPaths getReferencePaths = new GetReferenceAssemblyPaths();
            getReferencePaths.BuildEngine = engine;
            getReferencePaths.TargetFrameworkMoniker = targetFrameworkMoniker;
            getReferencePaths.RootPath = tempDirectory;
            getReferencePaths.Execute();
            string[] returnedPaths = getReferencePaths.ReferenceAssemblyPaths;
            Assert.Equal(0, returnedPaths.Length);
            string displayName = getReferencePaths.TargetFrameworkMonikerDisplayName;
            Assert.Null(displayName);
            FrameworkNameVersioning frameworkMoniker = new FrameworkNameVersioning(getReferencePaths.TargetFrameworkMoniker);
            engine.AssertLogContains("MSB3643");
        }
        finally
        {
            if (Directory.Exists(framework41Directory))
            {
                Directory.Delete(framework41Directory, true);
            }
        }
    }
}
*/