// -------------------------------------------------------------------------------------------------
// Assets/Editor/JenkinsBuild.cs
// -------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

// ------------------------------------------------------------------------
// https://docs.unity3d.com/ScriptReference/
// ------------------------------------------------------------------------
public class JenkinsBuild 
{
    static string APP_NAME = "src";
    static string TARGET_DIR = "C:\\Jenkins\\BUILDS";

    public static void BuildOnWindows()
    {
        string target_dir = APP_NAME + ".app";
        GenericBuild(TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    static void GenericBuild(string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/ChemClub.unity" };
        buildPlayerOptions.locationPathName = "generic_build";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;

        // EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);

        BuildReport build_report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = build_report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}