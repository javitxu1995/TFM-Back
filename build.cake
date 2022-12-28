#addin "nuget:?package=Cake.Sonar&version=1.1.18"
#addin nuget:?package=Cake.Incubator&version=3.1.0
#addin nuget:?package=Cake.Coverlet&version=2.1.2

#tool "nuget:?package=JetBrains.dotCover.CommandLineTools&version=2018.3.1"
#tool "nuget:?package=xunit.runner.console&version=2.3.1"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.3.1"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var projectName = "Auxquimia";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var sonarUrl = Argument("sonarurl", "https://sonar.izertis.com");
var sonarLogin = Argument("sonarlogin", "notset");
var currentBranch = Argument("branch", "develop");
var parsedProject = ParseProject($"./src/{projectName}/{projectName}.csproj", configuration, "AnyCPU");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir = Directory("./artifacts");
var testResultsOutput = artifactsDir.Path.FullPath + "/{0}.xml";
var webzip = File(artifactsDir.Path.FullPath + $"/{projectName}.zip");
EnsureDirectoryExists(artifactsDir);
CleanDirectory(artifactsDir);

var unitResults = new [] 
{
    artifactsDir.Path.FullPath + string.Format("/{0}.xUnit.xml", projectName),
    artifactsDir.Path.FullPath + string.Format("/{0}.Service.xUnit.xml", projectName)
};

var coverageResults = new [] 
{
    artifactsDir.Path.FullPath + string.Format("/coverage-{0}.xUnit.xml", projectName),
    artifactsDir.Path.FullPath + string.Format("/coverage-{0}.Service.xUnit.xml", projectName)
};

// Settings
var msBuildSettings = new DotNetCoreMSBuildSettings();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };

    var projectFiles = GetFiles("./**/*.csproj");
    foreach(var project in projectFiles)
    {
        DotNetCoreClean(project.FullPath, settings);
    }
    
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore($"./{projectName}.sln", new DotNetCoreRestoreSettings() {
        Verbosity = DotNetCoreVerbosity.Minimal,
        MSBuildSettings = msBuildSettings
    });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var path = MakeAbsolute(new DirectoryPath($"./{projectName}.sln"));

    var buildSettings = new DotNetCoreBuildSettings()
    {
        Configuration = configuration,
        NoRestore = true,
        MSBuildSettings = msBuildSettings
    };

    if (configuration == "Debug") 
    {
        buildSettings.ArgumentCustomization = args => args.AppendSwitch("/p:DebugType","=","Full");
    }

    DotNetCoreBuild(path.FullPath, buildSettings);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projectFiles = GetFiles("./tests/**/*.csproj");
    foreach(var project in projectFiles)
    {
        /*if (project.GetFilenameWithoutExtension().ToString().Contains("Integration")) 
        {
            continue;
        }*/
        try 
        {
            var projectFile = MakeAbsolute(project).ToString();
            var dotNetTestSettings = new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoBuild = true,
                VSTestReportPath = string.Format(testResultsOutput, project.GetFilenameWithoutExtension())
            };

            var coverletSettings = new CoverletSettings {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.opencover,
                CoverletOutputDirectory = artifactsDir,
                CoverletOutputName = $"coverage-{project.GetFilenameWithoutExtension()}.xml",
                Exclude = new List<string> { "[xunit.*]*" }
            };

            DotNetCoreTest(projectFile, dotNetTestSettings, coverletSettings);
        }
        catch(Exception ex)
        {
            Error("There was an error while running the tests", ex);
        }
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Build libraries
    var projects = GetFiles(string.Format("./src/**/{0}.csproj", projectName));
    foreach(var project in projects)
    {
        var name = project.GetDirectory().FullPath;

        DotNetCorePack(project.FullPath, new DotNetCorePackSettings 
        {
            Configuration = configuration,
            OutputDirectory = artifactsDir,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            MSBuildSettings = msBuildSettings
        });
    }

});

Task("Publish")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles(string.Format("./src/**/{0}.csproj", projectName));
    foreach(var project in projects)
    {
        var outputDirectory = artifactsDir + Directory(project.GetDirectory().GetDirectoryName());
        var settings = new DotNetCorePublishSettings
        {
            Framework = parsedProject.TargetFrameworkVersions.First(),
            Configuration = configuration,
            OutputDirectory = outputDirectory
        };

        DotNetCorePublish(project.GetDirectory().FullPath, settings);

        Zip(outputDirectory.Path.FullPath, webzip);
    }
    
});

Task("SonarBegin")
  .Does(() => 
{
    SonarBegin(new SonarBeginSettings 
    {
        //Supported parameters
        Key = $"{parsedProject.AssemblyName}:{currentBranch.Replace("/","_")}",
        Version = parsedProject.NetCore.Version,
        Name = $"{parsedProject.AssemblyName} {currentBranch}",
        Url = sonarUrl,
        Login = sonarLogin,
        Verbose = false,
        ArgumentCustomization = args => args
            .Append(string.Format("/d:sonar.cs.vstest.reportsPaths=\"{0}\"", string.Join(",", unitResults)))
            .Append(string.Format("/d:sonar.cs.opencover.reportsPaths=\"{0}\"", string.Join(",", coverageResults)))
    });
});

Task("SonarEnd")
  .Does(() => 
{
    SonarEnd(new SonarEndSettings
    {
        Login = sonarLogin
    });
});

Task("Analyze")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("Run-Unit-Tests")
  .IsDependentOn("SonarEnd");

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Create-NuGet-Packages");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
