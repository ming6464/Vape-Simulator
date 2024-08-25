using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RendererTool
{
    public class RendererEditorWindow : EditorWindow
{
    private int _selectedTab;
    private int _multiObjectTab;
    private string _shaderFrom;
    private string _shaderTo;
    private Material _materialToSet;
    private Material _materialFrom;
    private Material _materialTo;
    private Mesh _meshToSet;
    private Mesh _meshFrom;
    private Mesh _meshTo;
    private bool _applyToChildren;
    private Dictionary<GameObject,StackCustom<Material>> _undoMaterials = new Dictionary<GameObject, StackCustom<Material>>();
    private int _maxUndoCount = 10;
    private int _passUndoCount = 0;
    private bool _drawUndoInHierarchy = false;
    private bool _repaintHierarchy = true;
    private Vector2 _scrollPosition;
    
    [MenuItem("CustomTools/Renderer Tool")]
    public static void ShowWindow()
    {
        GetWindow<RendererEditorWindow>("Renderer Tool");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.Space();
        _selectedTab = GUILayout.Toolbar(_selectedTab, new string[] { "Project Scope", "Object Scope", "Setting" }, GUILayout.Height(30));

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        switch (_selectedTab)
        {
            case 0:
                DrawProjectScopeTab();
                break;
            case 1:
                DrawObjectScopeTab();
                break;
            case 2:
                DrawSettingTab();
                break;
        }
        EditorGUILayout.EndVertical();

        if (_selectedTab == 1)
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Clear All Data", GUILayout.Height(30)))
            {
                _repaintHierarchy = true;
                ClearData();
            }
        }

        if (_repaintHierarchy)
        {
            ClearDataObjectNull();
            _repaintHierarchy       = false;
            DataRendererTool.DrawUndoInHierarchy = _drawUndoInHierarchy;
            DataRendererTool.UndoMaterials = _undoMaterials;
            DataRendererTool.MaxUndoCount  = _maxUndoCount;
            RepaintHierarchy();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawSettingTab()
    {
        GUILayout.Label("Setting", EditorStyles.boldLabel);
        _drawUndoInHierarchy = EditorGUILayout.Toggle("Draw Undo In Hierarchy", _drawUndoInHierarchy);
        _maxUndoCount        = EditorGUILayout.IntField("Max Undo Count", _maxUndoCount);
        if(_passUndoCount != _maxUndoCount)
        {
            if(_maxUndoCount <= 0)
            {
                _maxUndoCount = 1;
                EditorGUILayout.IntField("Max Undo Count", _maxUndoCount);
            }
            _repaintHierarchy      = true;
            _passUndoCount         = _maxUndoCount;
            DataRendererTool.MaxUndoCount = _maxUndoCount;
            foreach (var undoMaterial in _undoMaterials)
            {
                undoMaterial.Value.ChangeSize(_maxUndoCount);
            }
        }

        if (DataRendererTool.DrawUndoInHierarchy != _drawUndoInHierarchy)
        {
            _repaintHierarchy             = true;
            DataRendererTool.DrawUndoInHierarchy = _drawUndoInHierarchy;
        }
    }
    private void RepaintHierarchy()
    {
        EditorApplication.RepaintHierarchyWindow();
    }

    private void ClearDataObjectNull()
    {
        var keys = _undoMaterials.Keys.ToList();
        foreach (var key in keys)
        {
            if (!key)
            {
                _undoMaterials.Remove(key);
            }
        }
    }

    private void DrawProjectScopeTab()
    {
        GUILayout.Label("Change Shader", EditorStyles.boldLabel);
        _shaderFrom = EditorGUILayout.TextField("Shader From", _shaderFrom);
        _shaderTo = EditorGUILayout.TextField("Shader To", _shaderTo);

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            ChangeShaderInProject(_shaderFrom, _shaderTo);
        }
    }

    private void DrawObjectScopeTab()
    {
        _applyToChildren = EditorGUILayout.Toggle("Apply to Children", _applyToChildren);

        EditorGUILayout.Space();
        _multiObjectTab = GUILayout.Toolbar(_multiObjectTab, new string[] { "Material", "Mesh" }, GUILayout.Height(30));

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        switch (_multiObjectTab)
        {
            case 0:
                DrawMaterialTab();
                break;
            case 1:
                DrawMeshTab();
                break;
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawMaterialTab()
    {
        GUILayout.Label("Set Material", EditorStyles.boldLabel);
        _materialToSet = (Material)EditorGUILayout.ObjectField("Material", _materialToSet, typeof(Material), false);

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            SetMaterialToSelectedObjects(_materialToSet);
        }

        EditorGUILayout.Space();
        GUILayout.Label("Change Material", EditorStyles.boldLabel);
        _materialFrom = (Material)EditorGUILayout.ObjectField("Material From", _materialFrom, typeof(Material), false);
        _materialTo   = (Material)EditorGUILayout.ObjectField("Material To", _materialTo, typeof(Material), false);

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            ChangeMaterialInSelectedObjects(_materialFrom, _materialTo);
        }
        
        DrawUndoButton();
        DrawClearData();
    }

    private void DrawClearData()
    {
        EditorGUILayout.Space();
        var undoAvailable = IsUndoAvailable();
        GUI.enabled         = undoAvailable;
        GUI.backgroundColor = undoAvailable ? Color.white : Color.gray;
        if (GUILayout.Button("Clear Data", GUILayout.Height(30)))
        {
            _repaintHierarchy = true;
            GameObject[] selectedObjects = GetObjectsSet();
            foreach (GameObject obj in selectedObjects)
            {
                if(!_undoMaterials.ContainsKey(obj))
                {
                    continue;
                }
                _undoMaterials.Remove(obj);
            }
        }
        GUI.enabled         = true;
        GUI.backgroundColor = Color.white;
    }

    private void DrawUndoButton()
    {
        DrawSeparator();
        
        var undoAvailable = IsUndoAvailable();
        GUI.enabled         = undoAvailable;
        GUI.backgroundColor = undoAvailable ? Color.white : Color.gray;
        if (GUILayout.Button("Undo", GUILayout.Height(30)))
        {
            _repaintHierarchy = true;
            UndoMaterialChange();
        }
        GUI.enabled         = true;
        GUI.backgroundColor = Color.white;
    }

    private bool IsUndoAvailable()
    {
        GameObject[] selectedObjects = GetObjectsSet();
        foreach (GameObject obj in selectedObjects)
        {
            if (!_undoMaterials.ContainsKey(obj))
            {
                continue;
            }

            StackCustom<Material> undoStack = _undoMaterials[obj];
            if (!undoStack.IsEmpty())
            {
                return true;
            }
        }
        return false;
    }

    private void UndoMaterialChange()
    {
        GameObject[] selectedObjects = GetObjectsSet();
        foreach (GameObject obj in selectedObjects)
        {
            if (!_undoMaterials.ContainsKey(obj))
            {
                continue;
            }

            StackCustom<Material> undoStack = _undoMaterials[obj];
            if (undoStack.IsEmpty())
            {
                continue;
            }
            
            Material material = undoStack.Pop();
            ApplyMaterial(obj.GetComponents<Renderer>(), material);
        }
    }

    private GameObject[] GetObjectsSet()
    {
        if (_applyToChildren)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (GameObject obj in Selection.gameObjects)
            {
                foreach(var transform in obj.GetComponentsInChildren<Transform>(true))
                {
                    objects.Add(transform.gameObject);
                }
            }
            return objects.ToArray();
        }
        return Selection.gameObjects;
    }

    private void DrawSeparator(float thickness = 2,float spaceUp = 10,float spaceDown = 10)
    {
        EditorGUILayout.Space(spaceUp);
        var rect = EditorGUILayout.GetControlRect(false, thickness);
        rect.height = thickness;
        EditorGUI.DrawRect(rect, new Color(0, 1f, 0f, 1));
        EditorGUILayout.Space(spaceDown);
    }

    private void DrawMeshTab()
    {
        GUILayout.Label("Set Mesh", EditorStyles.boldLabel);
        _meshToSet = (Mesh)EditorGUILayout.ObjectField("Mesh", _meshToSet, typeof(Mesh), false);

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            SetMeshToSelectedObjects(_meshToSet);
        }

        EditorGUILayout.Space();
        GUILayout.Label("Change Mesh", EditorStyles.boldLabel);
        _meshFrom = (Mesh)EditorGUILayout.ObjectField("Mesh From", _meshFrom, typeof(Mesh), false);
        _meshTo = (Mesh)EditorGUILayout.ObjectField("Mesh To", _meshTo, typeof(Mesh), false);

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            ChangeMeshInSelectedObjects(_meshFrom, _meshTo);
        }
    }

    private void ChangeShaderInProject(string shaderFrom, string shaderTo)
    {
        Shader fromShader = Shader.Find(shaderFrom);
        Shader toShader = Shader.Find(shaderTo);

        if (fromShader == null || toShader == null)
        {
            Debug.LogError("Invalid shader names provided.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material.shader == fromShader)
            {
                material.shader = toShader;
                EditorUtility.SetDirty(material);
            }
        }
        AssetDatabase.SaveAssets();
    }

    private void SetMaterialToSelectedObjects(Material material)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer[] renderers = _applyToChildren ? obj.GetComponentsInChildren<Renderer>() : obj.GetComponents<Renderer>();
            if(renderers.Length == 0)
            {
                continue;
            }
            SetMaterial(renderers, material);
        }
    }

    private void SetMaterial(Renderer[] renderers, Material material)
    {
        _repaintHierarchy = true;
        ApplyMaterial(renderers, material);
        foreach(Renderer renderer in renderers)
        {
            var objSet = renderer.gameObject;
            _undoMaterials.TryAdd(objSet, new StackCustom<Material>(_maxUndoCount));

            StackCustom<Material> undoStack = _undoMaterials[objSet];
            undoStack.Push(renderer.sharedMaterial);
        }
    }

    private void ChangeMaterialInSelectedObjects(Material fromMaterial, Material toMaterial)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var renderersFilter = GetRenderersFilter(obj,fromMaterial);
            if(renderersFilter.Length == 0)
            {
                continue;
            }
            SetMaterial(renderersFilter, toMaterial);
        }
    }

    private Renderer[] GetRenderersFilter(GameObject obj, Material fromMaterial)
    {
        Renderer[] renderers = _applyToChildren ? obj.GetComponentsInChildren<Renderer>() : obj.GetComponents<Renderer>();

        return renderers.Where(renderer => renderer.sharedMaterial == fromMaterial).ToArray();
    }

    private void ApplyMaterial(Renderer[] renderers, Material material)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = material;
        }
    }

    private void SetMeshToSelectedObjects(Mesh mesh)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ApplyMesh(obj, mesh);
        }
    }

    private void ChangeMeshInSelectedObjects(Mesh fromMesh, Mesh toMesh)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ChangeMesh(obj, fromMesh, toMesh);
        }
    }

    private void ApplyMesh(GameObject obj, Mesh mesh)
    {
        MeshFilter[] meshFilters = _applyToChildren ? obj.GetComponentsInChildren<MeshFilter>() : obj.GetComponents<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            meshFilter.sharedMesh = mesh;
        }
    }

    private void ChangeMesh(GameObject obj, Mesh fromMesh, Mesh toMesh)
    {
        MeshFilter[] meshFilters = _applyToChildren ? obj.GetComponentsInChildren<MeshFilter>() : obj.GetComponents<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.sharedMesh == fromMesh)
            {
                meshFilter.sharedMesh = toMesh;
            }
        }
    }

    private void ClearData()
    {
        _undoMaterials.Clear();
    }
}
}