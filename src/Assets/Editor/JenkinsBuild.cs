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
        EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        string build_name = APP_NAME + ".app";
        Debug.Log("Testing....");
        GenericBuild(TARGET_DIR + "/" + build_name, BuildTarget.WSAPlayer, BuildOptions.None);
    }

    static void GenericBuild(string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/ChemClub.unity" };
        buildPlayerOptions.locationPathName = target_dir;
        buildPlayerOptions.targetGroup = BuildTargetGroup.WSA; 
        buildPlayerOptions.target = BuildTarget.WSAPlayer;
        buildPlayerOptions.options = BuildOptions.None;

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