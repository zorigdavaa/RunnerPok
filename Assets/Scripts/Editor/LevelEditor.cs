using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    SerializedProperty sectionDataSP;
    string relateviePath;

    private void OnEnable()
    {
        relateviePath = "Assets/Prefabs/Level" + $"/{target.name} Data";
        sectionDataSP = serializedObject.FindProperty("SectionDatas");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Add New Section"))
        {
            CreateDirectory();
            ShowContextMenu();
        }
        EditorGUILayout.PropertyField(sectionDataSP, new GUIContent("Section Data"), true);
        serializedObject.ApplyModifiedProperties();
        DrawPropertiesExcluding(serializedObject, sectionDataSP.name);
        if (GUILayout.Button("Remove Unused Sections"))
        {
            RemoveUnused();
        }
    }

    private void RemoveUnused()
    {
        if (!AssetDatabase.IsValidFolder(relateviePath))
        {
            Debug.LogWarning($"The path {relateviePath} does not exist. Please create the directory first.");
            return;
        }
        Level script = (Level)target;
        List<string> toRemove = new List<string>();
        // var files = Directory.GetFiles(relateviePath, "Section*.asset");
        string[] guids = AssetDatabase.FindAssets("Section", new[] { relateviePath });
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            SectionData section = AssetDatabase.LoadAssetAtPath<SectionData>(assetPath);
            if (!script.SectionDatas.Contains(section))
            {
                toRemove.Add(guid);
            }
        }
        foreach (var guid in toRemove)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.DeleteAsset(assetPath);
        }
        EditorUtility.SetDirty(script);
        AssetDatabase.SaveAssets();
    }

    private void ShowContextMenu()
    {
        GenericMenu menu = new GenericMenu();

        // Main menu
        menu.AddItem(new GUIContent("Create/Fight"), false, () => AddToSection(SectionType.Fight));
        menu.AddItem(new GUIContent("Create/ChooseSkill"), false, () => AddToSection(SectionType.Choose));
        menu.AddItem(new GUIContent("Create/Normal"), false, () => AddToSection(SectionType.None));
        menu.AddItem(new GUIContent("Create/Obsacles"), false, () => AddToSection(SectionType.Obstacle));
        menu.AddItem(new GUIContent("Insert Last"), false, () => sectionDataSP.InsertArrayElementAtIndex(sectionDataSP.arraySize));
        menu.AddItem(new GUIContent("Reset Value"), false, ResetValue);
        menu.AddItem(new GUIContent("Remove null"), false, RemoveNull);

        menu.ShowAsContext();
    }

    private void RemoveNull()
    {
        Level script = (Level)target;
        script.SectionDatas.RemoveAll(section => section == null);
        EditorUtility.SetDirty(script);
        serializedObject.Update();
    }

    public void AddToSection(SectionType type)
    {
        SectionData newSection;
        if (type == SectionType.Fight)
        {
            newSection = CreateInstance<SectionDataFight>();
        }
        else if (type == SectionType.Choose)
        {
            var original = Addressables.LoadAssetAsync<SecDataChoose>("SkillChooseSecDefault").WaitForCompletion();
            if (original == null)
            {
                Debug.LogError("Failed to load the default Choose Section data.");
                return;
            }
            sectionDataSP.arraySize++;
            SerializedProperty newItem = sectionDataSP.GetArrayElementAtIndex(sectionDataSP.arraySize - 1);
            newItem.objectReferenceValue = original;
            serializedObject.ApplyModifiedProperties();
            return;
        }
        else if (type == SectionType.Obstacle)
        {
            newSection = CreateInstance<SectionObsData>();
        }
        else
        {
            newSection = CreateInstance<SectionData>();
        }
        newSection.Type = type;
        Level script = (Level)target;
        // string name = GetUniqueName("Section", directoryInfo.FullName);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{relateviePath}/Section.asset");
        AssetDatabase.CreateAsset(newSection, assetPath);
        script.SectionDatas.Add(newSection);
        newSection.FillSelf();
        EditorUtility.SetDirty(script);
        AssetDatabase.SaveAssets();
    }

    private void ResetValue()
    {
        Level script = (Level)target;
        script.SectionDatas = new List<SectionData>();
        EditorUtility.SetDirty(script);
    }

    public void CreateDirectory()
    {
        var fileName = target.name;
        bool sameNameDirec = AssetDatabase.IsValidFolder($"Assets/Prefabs/Level/{fileName} Data");
        if (!sameNameDirec)
        {
            AssetDatabase.CreateFolder("Assets/Prefabs/Level", fileName + " Data");
            AssetDatabase.Refresh();
        }
        relateviePath = "Assets/Prefabs/Level" + $"/{fileName} Data";
    }
    public string GetUniqueName(string baseName, string path)
    {
        var files = Directory.GetFiles(path).ToList();
        if (!files.Contains(baseName))
            return baseName;

        int counter = 1;
        string newName;
        do
        {
            newName = $"{baseName} {counter}";
            counter++;
        } while (files.Contains(newName));

        return newName;
    }
}
