#if UNITY_IOS
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.IO;

public class PostProcessorBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string buildPath)
    {
        Debug.log("Adservice not added");
        // if (target == BuildTarget.iOS)
        // {
        //     // Path to the Xcode project
        //     string projectPath = PBXProject.GetPBXProjectPath(buildPath);
        //     PBXProject project = new PBXProject();
        //     project.ReadFromFile(projectPath);

        //     // Get the target GUID (compatible with Unity 2019 and later)
        //     string targetGuid = project.GetUnityMainTargetGuid();

        //     // Add AdServices framework in weak link mode
        //     project.AddFrameworkToProject(targetGuid, "AdServices.framework", weak: true);

        //     // Save the modified Xcode project
        //     project.WriteToFile(projectPath);
        // }
    }
}
#endif
