using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameObjectScriptsWindow : EditorWindow
{
    private bool             _includeInactiveObjects;
    private int              _selectedScope;
    private bool             _applyToPrefabs;
    private int              _selectedFeature;
    private string           _searchScriptName;
    private int              _searchScope;
    private List<GameObject> _searchResults = new List<GameObject>();
    private MonoScript       _searchScript;

    
    [MenuItem("CustomTools/Game Object Tools")]
    public static void ShowWindow()
    {
        GetWindow<GameObjectScriptsWindow>("Game Object Tools");
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Object Tools", EditorStyles.boldLabel);

        _selectedFeature = EditorGUILayout.Popup("Feature", _selectedFeature, new string[] { "Search", "Remove" });

        if (_selectedFeature == 0)
        {
            DrawSearchFeature();
        }
        else if (_selectedFeature == 1)
        {
            DrawRemoveFeature();
        }
    }

    private void DrawSearchFeature()
    {
        _searchScope = EditorGUILayout.Popup("Search Scope", _searchScope, new string[] { "Hierarchy", "Assets" });

        // Add drag-and-drop functionality for script
        _searchScript = EditorGUILayout.ObjectField("Script", _searchScript, typeof(MonoScript), false) as MonoScript;

        if (GUILayout.Button("Search", GUILayout.Height(30)))
        {
            SearchForScript();
        }

        GUILayout.Label("Search Results", EditorStyles.boldLabel);
        foreach (var result in _searchResults)
        {
            GUILayout.Label(result.name);
        }
    }

    private void DrawRemoveFeature()
    {
        _includeInactiveObjects = EditorGUILayout.Toggle("Include Inactive Objects", _includeInactiveObjects);

        if (_selectedScope != 0)
            _applyToPrefabs = EditorGUILayout.Toggle("Apply to Prefabs", _applyToPrefabs);

        GUILayout.Label("Scope", EditorStyles.boldLabel);
        _selectedScope = GUILayout.SelectionGrid(_selectedScope,
                new string[] { "Project", "Hierarchy", "Selected Objects" }, 1, EditorStyles.radioButton);

        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            Apply();
        }
    }

    private void SearchForScript()
    {
        _searchResults.Clear();
        if (_searchScope == 0)
        {
            SearchInHierarchy();
        }
        else if (_searchScope == 1)
        {
            SearchInAssets();
        }
    }

    private void SearchInHierarchy()
    {
        if (_searchScript == null) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.GetComponent(_searchScript.GetClass()) != null)
            {
                _searchResults.Add(obj);
            }
        }
    }

    private void SearchInAssets()
    {
        if (_searchScript == null) return;

        string[] allGameObjectGuids = AssetDatabase.FindAssets("t:GameObject");
        foreach (string guid in allGameObjectGuids)
        {
            string     path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj  = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj.GetComponent(_searchScript.GetClass()) != null)
            {
                _searchResults.Add(obj);
            }
        }
    }

    private void Apply()
    {
        switch (_selectedScope)
        {
            case 0:
                RemoveNullScriptsInProject();
                break;
            case 1:
                RemoveNullScriptsInHierarchy();
                break;
            case 2:
                RemoveNullScriptsInSelectedObjects();
                break;
        }
    }

    private void RemoveNullScriptsInProject()
    {
        RemoveNullScriptsInHierarchy();

        string[] allGameObjectGuids = AssetDatabase.FindAssets("t:GameObject");
        List<GameObject> allGameObjects = new List<GameObject>();

        foreach (string guid in allGameObjectGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            allGameObjects.Add(obj);
        }

        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in allPrefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            allGameObjects.Add(prefab);
        }

        FindAndRemoveMissing(allGameObjects.ToArray());
    }

    private void RemoveNullScriptsInHierarchy()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> allRootGameObjects = new List<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.scene.isLoaded || _applyToPrefabs)
            {
                allRootGameObjects.Add(go);
            }
        }
        FindAndRemoveMissing(allRootGameObjects.ToArray());
    }

    private void RemoveNullScriptsInSelectedObjects()
    {
        FindAndRemoveMissing(Selection.gameObjects);
    }

    private void FindAndRemoveMissing(GameObject[] objs)
    {
        var deeperSelection = objs.SelectMany(go => go.GetComponentsInChildren<Transform>(_includeInactiveObjects))
                                  .Select(t => t.gameObject);
        var prefabs = new HashSet<Object>();
        int compCount = 0;
        int goCount = 0;
        var applyToPrefabs = _applyToPrefabs && _selectedScope != 0;

        foreach (var go in deeperSelection)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

            if (count > 0)
            {
                if (applyToPrefabs && PrefabUtility.IsPartOfAnyPrefab(go))
                {
                    RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
                    count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

                    if (count == 0)
                        continue;
                }

                Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                compCount += count;
                goCount++;
            }
        }

        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }

    private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
                                              ref int goCount)
    {
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);

        if (source == null || !prefabs.Add(source))
            return;

        RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

        int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);

        if (count > 0)
        {
            Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
            compCount += count;
            goCount++;
        }
    }
}