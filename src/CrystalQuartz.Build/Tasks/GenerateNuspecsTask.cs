﻿namespace CrystalQuartz.Build.Tasks
{
    using CrystalQuartz.Build.Common;
    using CrystalQuartz.Build.Extensions;
    using Rosalia.Core.Api;
    using Rosalia.TaskLib.NuGet.Tasks;

    public class GenerateNuspecsTask : Subflow
    {
        private readonly SolutionStructure _solution;
        private readonly string _configuration;
        private readonly string _version;

        public GenerateNuspecsTask(SolutionStructure solution, string configuration, string version)
        {
            _solution = solution;
            _configuration = configuration;
            _version = version;
        }

        protected override bool IsSequence
        {
            get { return true; }
        }

        protected override void RegisterTasks()
        {
            var merged = _configuration + "_Merged";

            Task(
                "Generate simple package spec",
                new GenerateNuGetSpecTask(_solution.Artifacts / "CrystalQuartz.Simple.nuspec")
                    .Id("CrystalQuartz.Simple")
                    
                    .FillCommonProperties(
                        _solution.Src/"CrystalQuartz.Web", 
                        _version,
                        new TargetedFile(_solution.Artifacts/"bin_400"/merged/"CrystalQuartz.Web.dll", "net40"),
                        new TargetedFile(_solution.Artifacts/"bin_400"/_configuration /"CrystalQuartz.Core.dll", "net40"),
                        new TargetedFile(_solution.Artifacts/"bin_452"/merged/"CrystalQuartz.Web.dll", "net452"),
                        new TargetedFile(_solution.Artifacts/"bin_452"/_configuration /"CrystalQuartz.Core.dll", "net452"))

                    .Description("Installs CrystalQuartz panel (pluggable Qurtz.NET viewer) using simple scheduler provider. This approach is appropriate for scenarios where the scheduler and a web application works in the same AppDomain.")
                    .WithFiles((_solution.BuildAssets/"Simple").AsDirectory().Files, "content"));
            
            Task(
                "Generate remote package spec",
                new GenerateNuGetSpecTask(_solution.Artifacts/"CrystalQuartz.Remote.nuspec")
                    .Id("CrystalQuartz.Remote")

                    .FillCommonProperties(
                        _solution.Src/"CrystalQuartz.Web",
                        _version,
                        new TargetedFile(_solution.Artifacts/"bin_400"/merged/"CrystalQuartz.Web.dll", "net40"),
                        new TargetedFile(_solution.Artifacts/"bin_400"/_configuration/"CrystalQuartz.Core.dll", "net40"),
                        new TargetedFile(_solution.Artifacts/"bin_452"/merged/"CrystalQuartz.Web.dll", "net452"),
                        new TargetedFile(_solution.Artifacts/"bin_452"/_configuration/"CrystalQuartz.Core.dll", "net452"))
                    
                    .Description("Installs CrystalQuartz panel (pluggable Qurtz.NET viewer) using remote scheduler provider. Note that you should set remote scheduler URI after the installation.")
                    .WithFiles(_solution.BuildAssets.GetDirectory("Remote").Files, "content"));
            
            Task(
                "Generate Owin package spec",
                new GenerateNuGetSpecTask(_solution.Artifacts/"CrystalQuartz.Owin.nuspec")
                    .Id("CrystalQuartz.Owin")

                    .FillCommonProperties(
                        _solution.Src/"CrystalQuartz.Owin",
                        _version,
                        new TargetedFile(_solution.Artifacts/"bin_450"/merged/"CrystalQuartz.Owin.dll", "net45"),
                        new TargetedFile(_solution.Artifacts/"bin_452"/merged/"CrystalQuartz.Owin.dll", "net452"))
                    
                    .Description("Installs CrystalQuartz panel (pluggable Qurtz.NET viewer) to any application (web or self-hosted) that uses OWIN environment."));
        }
    }
}