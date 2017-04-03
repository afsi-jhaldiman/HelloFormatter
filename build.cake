var solutionFile = "./HelloFormatter.sln";
var projectToPackage = "./HelloFormatter/HelloFormatter.csproj";
var outputDirectory = "./.build";
var packageDirectory = outputDirectory + "/package";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Default").IsDependentOn("Package");

Task("Clean")
  .Does(() =>
  {
    CleanDirectories(new[] { packageDirectory });
  });

Task("PackageRestore")
  .Does(() =>
  {
    NuGetRestore(solutionFile);
  });

Task("Build")
  .IsDependentOn("PackageRestore")
  .Does(() =>
  {
    MSBuild(solutionFile, new MSBuildSettings()
      .SetConfiguration(configuration)
      .SetVerbosity(Verbosity.Minimal)
      .WithProperty("AllowedReferenceRelatedFileExtensions", "none"));
  });

Task("Package")
  .IsDependentOn("Clean")
  .IsDependentOn("Build")
  .Does(() =>
  {
    var nuGetPackSettings = new NuGetPackSettings {
      OutputDirectory = packageDirectory,
      IncludeReferencedProjects = true,
      Properties = new Dictionary<string, string> {
        {"Configuration", configuration}
      }
    };

    NuGetPack(projectToPackage, nuGetPackSettings);
  });

RunTarget(target);