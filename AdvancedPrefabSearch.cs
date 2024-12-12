using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AdvancedPrefabSearch : EditorWindow
{
    private GameObject basePrefab;
    private string componentName;
    private int layer = 0;
    private string tagName = "Select Tag";
    private List<string> results = new List<string>();
    private bool searchVariantsRecursively = false;
    private Vector2 scrollPosition = Vector2.zero;
    private bool filterByLayer = false;

    [MenuItem("Tools/Advanced Prefab Search")]
    public static void ShowWindow()
    {
        GetWindow<AdvancedPrefabSearch>("Advanced Prefab Search");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Search Prefabs with Advanced Options", EditorStyles.boldLabel);

        // Prefab Variant Search
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Search for Prefab Variants", EditorStyles.boldLabel);
        basePrefab = (GameObject)EditorGUILayout.ObjectField("Base Prefab:", basePrefab, typeof(GameObject), false);
        searchVariantsRecursively = EditorGUILayout.Toggle("Search Recursively", searchVariantsRecursively);

        // Component Search
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Search by Component", EditorStyles.boldLabel);
        componentName = EditorGUILayout.TextField("Component Name:", componentName);

        // Layer and Tag Search
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Search by Layer and Tag", EditorStyles.boldLabel);
        filterByLayer = EditorGUILayout.Toggle("Filter by Layer", filterByLayer);
        if (filterByLayer)
        {
            layer = EditorGUILayout.LayerField("Layer:", layer);
        }
        tagName = EditorGUILayout.TagField("Tag:", tagName);

        // Search Button
        if (GUILayout.Button("Search"))
        {
            PerformSearch();
        }

        // Scrollable Results Section
        EditorGUILayout.Space();
        if (results.Count > 0)
        {
            EditorGUILayout.LabelField("Results:", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
            foreach (var result in results)
            {
                if (GUILayout.Button(result, EditorStyles.linkLabel))
                {
                    // On click, select the prefab in the Project window
                    SelectPrefab(result);
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void PerformSearch()
    {
        results.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            // Check Prefab Variants
            if (basePrefab != null)
            {
                if (searchVariantsRecursively)
                {
                    if (!IsVariantOfRecursively(prefab, basePrefab)) continue;
                }
                else
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(prefab) != basePrefab) continue;
                }
            }

            // Check Component
            if (!string.IsNullOrEmpty(componentName))
            {
                if (prefab.GetComponent(componentName) == null) continue;
            }

            // Check Layer
            if (filterByLayer && layer != -1 && prefab.layer != layer) continue;

            // Check Tag
            if (!string.IsNullOrEmpty(tagName) && tagName != "Select Tag" && prefab.tag != tagName) continue;

            results.Add(assetPath);
        }

        Debug.Log($"Search completed. Found {results.Count} matching prefabs.");
    }

    private bool IsVariantOfRecursively(GameObject prefab, GameObject targetBase)
    {
        GameObject currentBase = PrefabUtility.GetCorrespondingObjectFromSource(prefab);

        while (currentBase != null)
        {
            if (currentBase == targetBase)
            {
                return true;
            }
            currentBase = PrefabUtility.GetCorrespondingObjectFromSource(currentBase);
        }

        return false;
    }

    private void SelectPrefab(string assetPath)
    {
        // This selects the prefab in the Project window
        Object prefab = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        EditorGUIUtility.PingObject(prefab);
        Selection.activeObject = prefab;
    }
}