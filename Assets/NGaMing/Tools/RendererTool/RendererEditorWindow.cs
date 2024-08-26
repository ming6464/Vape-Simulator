using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace RendererTool
{
    public class RendererEditorWindow : EditorWindow
    {
        private int      _selectedTab     = 0;
        private int      _selectedFeature = 0;
        private int      _multiObjectTab  = 0;
        private string   _shaderFrom      = "";
        private string   _shaderTo        = "";
        private Material _materialToSet;
        private Material _materialFrom;
        private Material _materialTo;
        private Mesh     _meshToSet;
        private Mesh     _meshFrom;
        private Mesh     _meshTo;
        private bool     _applyToChildren = false;

        private Dictionary<GameObject, StackCustom<Material>> _undoMaterials =
                new Dictionary<GameObject, StackCustom<Material>>();

        private int              _maxUndoCount = 10;
        private int              _passUndoCount;
        private bool             _drawUndoInHierarchy;
        private bool             _repaintHierarchy        = true;
        private Vector2          _scrollPositionSetupTab  = Vector2.zero;
        private Vector2          _scrollPositionSearchTab = Vector2.zero;
        private Material         _materialFromProject;
        private Material         _materialToProject;
        private bool             _updateInScenes           = true;
        private bool             _updateInAssets           = true;
        private int              _searchScope              = 0;
        private int              _searchType               = 0;
        private string           _shaderSearch             = "";
        private Material         _materialSearch           = null;
        private List<GameObject> _searchResults            = new List<GameObject>();
        private float            _itemWidth                = 100f;
        private string           _currentResultSearchType  = "";
        private string           _currentResultSearchScope = "";
        private int              _returnType               = 0;
        private int              _itemCount                = 0;

        [MenuItem("CustomTools/Renderer Tool")]
        public static void ShowWindow()
        {
            GetWindow<RendererEditorWindow>("Renderer Tool");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            _selectedFeature =
                    GUILayout.Toolbar(_selectedFeature, new string[] { "Search", "Setup" }, GUILayout.Width(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.Space(15); // Left padding
            GUILayout.BeginVertical();

            switch (_selectedFeature)
            {
                case 0:
                    DrawSearchTab();

                    break;
                case 1:
                    DrawSetupTab();

                    break;
            }

            GUILayout.EndVertical();
            GUILayout.Space(15); // Right padding
            GUILayout.EndHorizontal();
        }

        #region SearchTab

        private GameObject[] GetAllGameObjectsInHierarchy()
        {
            List<GameObject> allObjectsInHierarchy = new List<GameObject>();
            GameObject[]     allObjects            = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.hideFlags == HideFlags.None && obj.scene.isLoaded)
                {
                    allObjectsInHierarchy.Add(obj);
                }
            }

            return allObjectsInHierarchy.ToArray();
        }

        private string[] GetAllShaderNames()
        {
            string[]     guids       = AssetDatabase.FindAssets("t:Shader");
            List<string> shaderNames = new List<string>();

            foreach (string guid in guids)
            {
                string path   = AssetDatabase.GUIDToAssetPath(guid);
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);

                if (shader != null)
                {
                    shaderNames.Add(shader.name);
                }
            }

            return shaderNames.ToArray();
        }

        private void SearchShader(string shaderName)
        {
            Shader shader = Shader.Find(shaderName);

            if (shader == null)
            {
                Debug.LogError("Shader not found.");
                _searchResults.Clear();

                return;
            }

            SearchInScope(renderer => renderer.sharedMaterial != null && renderer.sharedMaterial.shader == shader);
        }

        private void SearchMaterial(Material material)
        {
            if (material == null)
            {
                Debug.LogError("Material not found.");
                _searchResults.Clear();

                return;
            }

            SearchInScope(renderer => renderer.sharedMaterial == material);
        }

        private void SearchInScope(System.Func<Renderer, bool> predicate)
        {
            _searchResults.Clear();

            if (_searchScope == 1) // Hierarchy
            {
                GameObject[] allObjects = GetAllGameObjectsInHierarchy();

                foreach (GameObject obj in allObjects)
                {
                    Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);

                    foreach (Renderer renderer in renderers)
                    {
                        if (predicate(renderer))
                        {
                            _searchResults.Add(renderer.gameObject);
                            EditorGUIUtility.PingObject(renderer.gameObject);
                        }
                    }
                }
            }
            else if (_searchScope == 0) // Assets
            {
                string[] guids = AssetDatabase.FindAssets("t:GameObject");

                foreach (string guid in guids)
                {
                    string     path      = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject obj       = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);

                    foreach (Renderer renderer in renderers)
                    {
                        if (predicate(renderer))
                        {
                            _searchResults.Add(renderer.gameObject);
                        }
                    }
                }
            }
        }


        private void DrawSearchTab()
        {
            DrawSearchScopeAndType();
            DrawSearchInputFields();
            DrawSettings();
            DrawSearchResults();
        }

        private void DrawSearchScopeAndType()
        {
            _searchScope = GUILayout.Toolbar(_searchScope, new string[] { "Assets", "Hierarchy" });
            EditorGUILayout.Space();
            GUILayout.Label("Search Type", EditorStyles.boldLabel);
            _searchType = GUILayout.Toolbar(_searchType, new string[] { "Shader", "Material" });
            EditorGUILayout.Space();
        }

        private void DrawSearchInputFields()
        {
            if (_searchType == 0)
            {
                string[] shaderNames   = GetAllShaderNames();
                int      selectedIndex = Mathf.Max(0, Array.IndexOf(shaderNames, _shaderSearch));
                selectedIndex = EditorGUILayout.Popup("Shader Name", selectedIndex, shaderNames);
                _shaderSearch = shaderNames[selectedIndex];
                DrawSearchButtons(() => SearchShader(_shaderSearch), "Shader");
            }
            else
            {
                _materialSearch =
                        (Material)EditorGUILayout.ObjectField("Material", _materialSearch, typeof(Material), false);
                _returnType = 0; // Set return type to Material when searching for Material
                DrawSearchButtons(() => SearchMaterial(_materialSearch), "Material");
            }
        }

        private void DrawSearchButtons(System.Action searchAction, string searchType)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Search", GUILayout.Height(30), GUILayout.MaxWidth(500)))
            {
                searchAction();
                _currentResultSearchScope = _searchScope == 0 ? "Assets" : "Hierarchy";
                _currentResultSearchType  = searchType;
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Clear Data", GUILayout.Height(30), GUILayout.MaxWidth(500)))
            {
                _currentResultSearchType  = "";
                _currentResultSearchScope = "";
                _searchResults.Clear();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawSettings()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Setting", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Width", GUILayout.Width(100));
            _itemWidth = EditorGUILayout.FloatField(_itemWidth);
            _itemWidth = math.max(100, _itemWidth);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            var tabs = new List<string> { "GameObject" };

            if (_searchScope == 0 && _searchType == 0)
            {
                tabs.Add("Material");
            }

            GUILayout.Label("Return Type", EditorStyles.boldLabel);
            _returnType = GUILayout.SelectionGrid(_returnType, tabs.ToArray(), 1, EditorStyles.radioButton);
            EditorGUILayout.Space();
        }

        private void DrawSearchResults()
        {
            _scrollPositionSearchTab = EditorGUILayout.BeginScrollView(_scrollPositionSearchTab);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search Results", EditorStyles.boldLabel);

            if (_currentResultSearchScope.Length > 0)
            {
                GUIStyle resultStyle = new GUIStyle(GUI.skin.label);
                resultStyle.normal.textColor = _searchResults.Count == 0 ? Color.red : Color.green;
                var type = _returnType == 0 ? "GameObject" : "Material";
                GUILayout.Label($"{_currentResultSearchScope} | {_currentResultSearchType} : {_itemCount} {type}",
                        resultStyle);
            }

            GUILayout.EndHorizontal();

            var   width         = math.min(position.width, _itemWidth);
            int   itemsPerRow   = Mathf.FloorToInt(position.width / width);
            int   count         = 0;
            float totalRowWidth = itemsPerRow * width;
            float padding       = (position.width - totalRowWidth) / 2;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(padding);
            _searchResults = _searchResults.OrderByDescending(IsRoot).ToList();
            _itemCount     = _searchResults.Count;

            if (_returnType == 0) // GameObject
            {
                DrawGameObjectResults(itemsPerRow, width, padding, ref count);
            }
            else if (_returnType == 1) // Material
            {
                DrawMaterialResults(itemsPerRow, width, padding, ref count);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGameObjectResults(int itemsPerRow, float width, float padding, ref int count)
        {
            if (_searchScope == 0) // Assets
            {
                foreach (GameObject obj in _searchResults)
                {
                    DrawGameObjectButton(obj, itemsPerRow, width, padding, ref count);
                }
            }
            else
            {
                foreach (GameObject obj in _searchResults)
                {
                    DrawGameObjectButton(obj, itemsPerRow, width, padding, ref count);
                }
            }
        }

        private void DrawGameObjectButton(GameObject obj, int itemsPerRow, float width, float padding, ref int count)
        {
            if (count % itemsPerRow == 0 && count != 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(padding);
            }

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

            if (IsRoot(obj))
            {
                buttonStyle.normal.textColor = Color.green;
            }

            if (GUILayout.Button(obj.name, buttonStyle, GUILayout.Width(width)))
            {
                Selection.activeGameObject = obj;
            }

            count++;
        }

        private void DrawMaterialResults(int itemsPerRow, float width, float padding, ref int count)
        {
            var materials = new List<Material>();

            foreach (var obj in _searchResults)
            {
                if (!obj.TryGetComponent<Renderer>(out var renderer))
                    continue;
                if (materials.Contains(renderer.sharedMaterial))
                    continue;
                materials.Add(renderer.sharedMaterial);
            }

            _itemCount = materials.Count;

            foreach (Material mat in materials)
            {
                if (count % itemsPerRow == 0 && count != 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(padding);
                }

                if (GUILayout.Button(mat.name, GUILayout.Width(width)))
                {
                    Selection.activeObject = mat;
                }

                count++;
            }
        }

        private bool IsRoot(GameObject obj)
        {
            return obj.transform.parent == null;
        }

        #endregion

        #region SetupTab

        private void DrawSetupTab()
        {
            EditorGUILayout.Space();
            var selectedTab = GUILayout.Toolbar(_selectedTab,
                    new string[] { "Project Scope", "Hierarchy Scope", "Setting" },
                    GUILayout.Height(30));

            EditorGUILayout.Space();
            _scrollPositionSetupTab = EditorGUILayout.BeginScrollView(_scrollPositionSetupTab);
            EditorGUILayout.BeginVertical("box");

            if (selectedTab != _selectedTab)
            {
                _scrollPositionSetupTab = Vector2.zero;
                _selectedTab            = selectedTab;
            }

            switch (_selectedTab)
            {
                case 0:
                    DrawProjectScopeTab();

                    break;
                case 1:
                    DrawHierarChyScopeTab();

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
                _repaintHierarchy                    = false;
                DataRendererTool.DrawUndoInHierarchy = _drawUndoInHierarchy;
                DataRendererTool.UndoMaterials       = _undoMaterials;
                DataRendererTool.MaxUndoCount        = _maxUndoCount;
                RepaintHierarchy();
            }

            EditorGUILayout.EndScrollView();
        }

        #region Setting Tab

        private void DrawSettingTab()
        {
            GUILayout.Label("Setting", EditorStyles.boldLabel);
            _drawUndoInHierarchy = EditorGUILayout.Toggle("Draw Undo In Hierarchy", _drawUndoInHierarchy);
            _maxUndoCount        = EditorGUILayout.IntField("Max Undo Count", _maxUndoCount);

            if (_passUndoCount != _maxUndoCount)
            {
                if (_maxUndoCount <= 0)
                {
                    _maxUndoCount = 1;
                    EditorGUILayout.IntField("Max Undo Count", _maxUndoCount);
                }

                _repaintHierarchy             = true;
                _passUndoCount                = _maxUndoCount;
                DataRendererTool.MaxUndoCount = _maxUndoCount;

                foreach (var undoMaterial in _undoMaterials)
                {
                    undoMaterial.Value.ChangeSize(_maxUndoCount);
                }
            }

            if (DataRendererTool.DrawUndoInHierarchy != _drawUndoInHierarchy)
            {
                _repaintHierarchy                    = true;
                DataRendererTool.DrawUndoInHierarchy = _drawUndoInHierarchy;
            }
        }

        private void RepaintHierarchy()
        {
            EditorApplication.RepaintHierarchyWindow();
        }

        #endregion

        #region Project Scope Func

        private void DrawProjectScopeTab()
        {
            GUILayout.Label("Change Shader", EditorStyles.boldLabel);

            string[] shaderNames = GetAllShaderNames();

            // Shader From
            int selectedFromIndex = Mathf.Max(0, Array.IndexOf(shaderNames, _shaderFrom));
            selectedFromIndex = EditorGUILayout.Popup("Select Shader From", selectedFromIndex, shaderNames);
            _shaderFrom       = shaderNames[selectedFromIndex];

            // Shader To
            int selectedToIndex = Mathf.Max(0, Array.IndexOf(shaderNames, _shaderTo));
            selectedToIndex = EditorGUILayout.Popup("Select Shader To", selectedToIndex, shaderNames);
            _shaderTo       = shaderNames[selectedToIndex];

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply Shader", GUILayout.Height(30)))
            {
                ChangeShaderInProject(_shaderFrom, _shaderTo);
            }

            EditorGUILayout.Space();
            GUILayout.Label("Change Material", EditorStyles.boldLabel);
            _materialFromProject =
                    (Material)EditorGUILayout.ObjectField("Material From", _materialFromProject, typeof(Material),
                            false);
            _materialToProject =
                    (Material)EditorGUILayout.ObjectField("Material To", _materialToProject, typeof(Material), false);

            EditorGUILayout.Space();
            var updateInScene  = EditorGUILayout.Toggle("Update in Scenes", _updateInScenes);
            var updateInAssets = EditorGUILayout.Toggle("Update in Assets", _updateInAssets);

            if (!updateInAssets && !updateInScene)
            {
                if (_updateInAssets)
                {
                    updateInScene = true;
                }
                else
                {
                    updateInAssets = true;
                }
            }

            _updateInScenes = updateInScene;
            _updateInAssets = updateInAssets;

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply Material", GUILayout.Height(30)))
            {
                ChangeMaterialInProject(_materialFromProject, _materialToProject);
            }
        }

        private void ChangeMaterialInProject(Material fromMaterial, Material toMaterial)
        {
            if (fromMaterial == null || toMaterial == null)
            {
                Debug.LogError("Invalid material provided.");

                return;
            }

            if (_updateInAssets)
            {
                // Change materials in all assets
                string[] guids = AssetDatabase.FindAssets("t:Material");

                foreach (string guid in guids)
                {
                    string   path     = AssetDatabase.GUIDToAssetPath(guid);
                    Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                    if (material == fromMaterial)
                    {
                        material = toMaterial;
                        EditorUtility.SetDirty(material);
                    }
                }
            }

            if (_updateInScenes)
            {
                // Change materials in all scenes
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    if (scene.enabled)
                    {
                        Scene      currentScene = EditorSceneManager.OpenScene(scene.path);
                        Renderer[] renderers    = GameObject.FindObjectsOfType<Renderer>();

                        foreach (Renderer renderer in renderers)
                        {
                            if (renderer.sharedMaterial == fromMaterial)
                            {
                                renderer.sharedMaterial = toMaterial;
                                EditorUtility.SetDirty(renderer);
                            }
                        }

                        EditorSceneManager.SaveScene(currentScene);
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }

        #endregion

        #region Hierarchy Tab

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

        private void DrawHierarChyScopeTab()
        {
            _applyToChildren = EditorGUILayout.Toggle("Apply to Children", _applyToChildren);

            EditorGUILayout.Space();
            _multiObjectTab =
                    GUILayout.Toolbar(_multiObjectTab, new string[] { "Material", "Mesh" }, GUILayout.Height(30));

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
            _materialFrom =
                    (Material)EditorGUILayout.ObjectField("Material From", _materialFrom, typeof(Material), false);
            _materialTo = (Material)EditorGUILayout.ObjectField("Material To", _materialTo, typeof(Material), false);

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
                    if (!_undoMaterials.ContainsKey(obj))
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
                    foreach (var transform in obj.GetComponentsInChildren<Transform>(true))
                    {
                        objects.Add(transform.gameObject);
                    }
                }

                return objects.ToArray();
            }

            return Selection.gameObjects;
        }

        private void DrawSeparator(float thickness = 2, float spaceUp = 10, float spaceDown = 10)
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
            _meshTo   = (Mesh)EditorGUILayout.ObjectField("Mesh To", _meshTo, typeof(Mesh), false);

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply", GUILayout.Height(30)))
            {
                ChangeMeshInSelectedObjects(_meshFrom, _meshTo);
            }
        }

        private void ChangeShaderInProject(string shaderFrom, string shaderTo)
        {
            Shader fromShader = Shader.Find(shaderFrom);
            Shader toShader   = Shader.Find(shaderTo);

            if (fromShader == null || toShader == null)
            {
                Debug.LogError("Invalid shader names provided.");

                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:Material");

            foreach (string guid in guids)
            {
                string   path     = AssetDatabase.GUIDToAssetPath(guid);
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
                Renderer[] renderers = _applyToChildren
                        ? obj.GetComponentsInChildren<Renderer>()
                        : obj.GetComponents<Renderer>();

                if (renderers.Length == 0)
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

            foreach (Renderer renderer in renderers)
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
                var renderersFilter = GetRenderersFilter(obj, fromMaterial);

                if (renderersFilter.Length == 0)
                {
                    continue;
                }

                SetMaterial(renderersFilter, toMaterial);
            }
        }

        private Renderer[] GetRenderersFilter(GameObject obj, Material fromMaterial)
        {
            Renderer[] renderers =
                    _applyToChildren ? obj.GetComponentsInChildren<Renderer>() : obj.GetComponents<Renderer>();

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
            MeshFilter[] meshFilters = _applyToChildren
                    ? obj.GetComponentsInChildren<MeshFilter>()
                    : obj.GetComponents<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters)
            {
                meshFilter.sharedMesh = mesh;
            }
        }

        private void ChangeMesh(GameObject obj, Mesh fromMesh, Mesh toMesh)
        {
            MeshFilter[] meshFilters = _applyToChildren
                    ? obj.GetComponentsInChildren<MeshFilter>()
                    : obj.GetComponents<MeshFilter>();

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

        #endregion

        #endregion
    }
}