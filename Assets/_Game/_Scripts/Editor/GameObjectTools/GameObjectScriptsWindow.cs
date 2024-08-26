using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameObjectScriptsWindow : EditorWindow
{
    private bool _includeInactiveObjects;
    private int  _selectedScope;
    private bool _applyToPrefabs;

    [MenuItem("CustomTools/Game Object Tools")]
    public static void ShowWindow()
    {
        GetWindow<GameObjectScriptsWindow>("Remove Null Scripts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Null Scripts", EditorStyles.boldLabel);

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
        
        string[]         allGameObjectGuids = AssetDatabase.FindAssets("t:GameObject");
        List<GameObject> allGameObjects     = new List<GameObject>();

        foreach (string guid in allGameObjectGuids)
        {
            string     path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj  = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            allGameObjects.Add(obj);
        }

        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in allPrefabGuids)
        {
            string     path   = AssetDatabase.GUIDToAssetPath(guid);
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
        var prefabs        = new HashSet<Object>();
        int compCount      = 0;
        int goCount        = 0;
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

                    // if count == 0 the missing scripts has been removed from prefabs
                    if (count == 0)
                        continue;
                    // if not the missing scripts must be prefab overrides on this instance
                }

                Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                compCount += count;
                goCount++;
            }
        }

        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }

    // Prefabs can both be nested or variants, so best way to clean all is to go through them all
    // rather than jumping straight to the original prefab source.
    private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
                                              ref int    goCount)
    {
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);

        // Only visit if source is valid, and hasn't been visited before
        if (source == null || !prefabs.Add(source))
            return;

        // go deep before removing, to differantiate local overrides from missing in source
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