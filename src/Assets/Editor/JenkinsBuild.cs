// -------------------------------------------------------------------------------------------------
// Assets/Editor/JenkinsBuild.cs
// -------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
 
// ------------------------------------------------------------------------
// https://docs.unity3d.com/Manual/CommandLineArguments.html
// ------------------------------------------------------------------------
public class JenkinsBuild 
{
    static string[] EnabledScenes = FindEnabledEditorScenes();
    static string APP_NAME = "src";
    static string TARGET_DIR = "C:\\Jenkins\\BUILDS";

    public static void BuildOnWindows()
    {
        string target_dir = APP_NAME + ".app";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        if (res.Length > 0) 
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }

    private static string[] FindEnabledEditorScenes() 
    {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) 
        {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}
}