using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Unity Editor script for exporting scene and prefab metadata to markdown files
/// Implements requirements from UnityMetadataExport.mdc rule
/// Exports to Documentation/UnityExports/ directory for AI context awareness
/// </summary>
public class MetadataExporter : EditorWindow
{
    private static readonly string ExportBasePath = "Documentation/UnityExports";
    private static readonly string ScenesPath = "Documentation/UnityExports/Scenes";
    private static readonly string PrefabsPath = "Documentation/UnityExports/Prefabs";
    
    private Vector2 scrollPosition;
    private bool autoExportOnSave = true;
    private bool autoExportOnHierarchyChange = false;

    [MenuItem("Tools/Export/Scene Metadata")]
    public static void ExportCurrentSceneMetadata()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            Debug.LogError("No active scene to export.");
            return;
        }
        
        ExportSceneMetadata(activeScene);
    }

    [MenuItem("Tools/Export/Prefab Metadata")]
    public static void ExportAllPrefabMetadata()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        
        if (prefabGuids.Length == 0)
        {
            Debug.LogWarning("No prefabs found in Assets/Prefabs/ directory.");
            return;
        }
        
        int exportedCount = 0;
        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (ExportPrefabMetadata(assetPath))
            {
                exportedCount++;
            }
        }
        
        Debug.Log($"Exported metadata for {exportedCount} prefabs to {PrefabsPath}/");
    }

    [MenuItem("Tools/Export/Export All Metadata")]
    public static void ExportAllMetadata()
    {
        Debug.Log("Starting bulk metadata export...");
        
        // Export current scene
        ExportCurrentSceneMetadata();
        
        // Export all prefabs
        ExportAllPrefabMetadata();
        
        // Export all scenes in project
        ExportAllSceneMetadata();
        
        Debug.Log("Bulk metadata export completed.");
    }

    [MenuItem("Tools/Export/Metadata Exporter Window")]
    public static void ShowWindow()
    {
        GetWindow<MetadataExporter>("Metadata Exporter");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Unity Metadata Exporter", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("Export Options:", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Export Current Scene Metadata"))
        {
            ExportCurrentSceneMetadata();
        }
        
        if (GUILayout.Button("Export All Prefab Metadata"))
        {
            ExportAllPrefabMetadata();
        }
        
        if (GUILayout.Button("Export All Metadata (Scenes + Prefabs)"))
        {
            ExportAllMetadata();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Auto-Export Settings:", EditorStyles.boldLabel);
        
        autoExportOnSave = EditorGUILayout.Toggle("Auto-export on Scene Save", autoExportOnSave);
        autoExportOnHierarchyChange = EditorGUILayout.Toggle("Auto-export on Hierarchy Change", autoExportOnHierarchyChange);
        
        if (GUILayout.Button("Apply Auto-Export Settings"))
        {
            ApplyAutoExportSettings();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Export Directories:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Scenes:", ScenesPath);
        EditorGUILayout.LabelField("Prefabs:", PrefabsPath);
        
        if (GUILayout.Button("Create Export Directories"))
        {
            CreateExportDirectories();
        }
        
        EditorGUILayout.EndScrollView();
    }

    private static void ExportAllSceneMetadata()
    {
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        
        if (sceneGuids.Length == 0)
        {
            Debug.LogWarning("No scenes found in Assets/Scenes/ directory.");
            return;
        }
        
        int exportedCount = 0;
        foreach (string guid in sceneGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = Path.GetFileNameWithoutExtension(assetPath);
            
            // Don't export the currently active scene twice
            if (SceneManager.GetActiveScene().name == sceneName)
                continue;
                
            if (ExportSceneMetadataByPath(assetPath))
            {
                exportedCount++;
            }
        }
        
        Debug.Log($"Exported metadata for {exportedCount} additional scenes to {ScenesPath}/");
    }

    private static bool ExportSceneMetadataByPath(string scenePath)
    {
        try
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            bool success = ExportSceneMetadata(scene);
            EditorSceneManager.CloseScene(scene, true);
            return success;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to export scene metadata for {scenePath}: {e.Message}");
            return false;
        }
    }

    private static bool ExportSceneMetadata(Scene scene)
    {
        if (!scene.IsValid())
        {
            Debug.LogError($"Invalid scene: {scene.name}");
            return false;
        }
        
        try
        {
            CreateExportDirectories();
            
            string fileName = $"Scene_{scene.name}_Metadata.md";
            string filePath = Path.Combine(ScenesPath, fileName);
            
            StringBuilder metadata = new StringBuilder();
            
            // Header information
            metadata.AppendLine($"# {scene.name} Metadata");
            metadata.AppendLine();
            metadata.AppendLine("## Export Information");
            metadata.AppendLine($"**Type**: Scene");
            metadata.AppendLine($"**Unity Path**: {scene.path}");
            metadata.AppendLine($"**Last Exported**: {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
            metadata.AppendLine($"**Unity Version**: {Application.unityVersion}");
            metadata.AppendLine($"**Export Script**: MetadataExporter v1.0.0");
            metadata.AppendLine();
            
            // Hierarchy structure
            metadata.AppendLine("## Hierarchy Structure");
            metadata.AppendLine("```");
            
            var rootObjects = scene.GetRootGameObjects().OrderBy(go => go.name);
            foreach (GameObject rootObj in rootObjects)
            {
                ExportGameObjectHierarchy(rootObj, metadata, 0);
            }
            
            metadata.AppendLine("```");
            metadata.AppendLine();
            
            // Asset dependencies
            var dependencies = CollectSceneDependencies(scene);
            ExportDependencies(metadata, dependencies);
            
            // Write file
            File.WriteAllText(filePath, metadata.ToString(), Encoding.UTF8);
            
            // Create timestamp file
            string timestampFile = Path.Combine(ScenesPath, $"Scene_{scene.name}_LastExported.txt");
            File.WriteAllText(timestampFile, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            
            Debug.Log($"Exported scene metadata: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to export scene metadata for {scene.name}: {e.Message}");
            return false;
        }
    }

    private static bool ExportPrefabMetadata(string prefabPath)
    {
        try
        {
            CreateExportDirectories();
            
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefabAsset == null)
            {
                Debug.LogError($"Failed to load prefab at {prefabPath}");
                return false;
            }
            
            string prefabName = prefabAsset.name;
            string fileName = $"Prefab_{prefabName}_Metadata.md";
            string filePath = Path.Combine(PrefabsPath, fileName);
            
            StringBuilder metadata = new StringBuilder();
            
            // Header information
            metadata.AppendLine($"# {prefabName} Metadata");
            metadata.AppendLine();
            metadata.AppendLine("## Export Information");
            metadata.AppendLine($"**Type**: Prefab");
            metadata.AppendLine($"**Unity Path**: {prefabPath}");
            metadata.AppendLine($"**Last Exported**: {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}");
            metadata.AppendLine($"**Unity Version**: {Application.unityVersion}");
            metadata.AppendLine($"**Export Script**: MetadataExporter v1.0.0");
            metadata.AppendLine();
            
            // Hierarchy structure
            metadata.AppendLine("## Hierarchy Structure");
            metadata.AppendLine("```");
            ExportGameObjectHierarchy(prefabAsset, metadata, 0);
            metadata.AppendLine("```");
            metadata.AppendLine();
            
            // Asset dependencies
            var dependencies = CollectPrefabDependencies(prefabAsset);
            ExportDependencies(metadata, dependencies);
            
            // Write file
            File.WriteAllText(filePath, metadata.ToString(), Encoding.UTF8);
            
            // Create timestamp file
            string timestampFile = Path.Combine(PrefabsPath, $"Prefab_{prefabName}_LastExported.txt");
            File.WriteAllText(timestampFile, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            
            Debug.Log($"Exported prefab metadata: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to export prefab metadata for {prefabPath}: {e.Message}");
            return false;
        }
    }

    private static void ExportGameObjectHierarchy(GameObject obj, StringBuilder metadata, int depth)
    {
        if (obj == null) return;
        
        string indent = new string(' ', depth * 2);
        string objInfo = $"{obj.name} (Layer: {LayerMask.LayerToName(obj.layer)}, Tag: {obj.tag})";
        if (!obj.activeInHierarchy)
        {
            objInfo += " [INACTIVE]";
        }
        
        metadata.AppendLine($"{indent}{objInfo}");
        
        // Export transform data
        var transform = obj.transform;
        string transformInfo = $"Transform: Pos({transform.position.x:F2},{transform.position.y:F2},{transform.position.z:F2}) " +
                              $"Rot({transform.rotation.x:F2},{transform.rotation.y:F2},{transform.rotation.z:F2},{transform.rotation.w:F2}) " +
                              $"Scale({transform.localScale.x:F2},{transform.localScale.y:F2},{transform.localScale.z:F2})";
        metadata.AppendLine($"{indent}├── {transformInfo}");
        
        // Export components
        var components = obj.GetComponents<Component>().Where(c => c != null && !(c is Transform));
        foreach (var component in components)
        {
            ExportComponentInfo(component, metadata, depth + 1);
        }
        
        // Recursively export children
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i).gameObject;
            ExportGameObjectHierarchy(child, metadata, depth + 1);
        }
    }

    private static void ExportComponentInfo(Component component, StringBuilder metadata, int depth)
    {
        if (component == null) return;
        
        string indent = new string(' ', depth * 2);
        string componentName = component.GetType().Name;
        
        // Generate component link with markdown
        string componentLink = GetComponentMarkdownLink(component);
        
        metadata.AppendLine($"{indent}├── {componentLink}");
        
        // Export serialized fields for MonoBehaviours
        if (component is MonoBehaviour)
        {
            ExportSerializedFields(component, metadata, depth + 1);
        }
        else
        {
            // Export key properties for built-in components
            ExportBuiltInComponentProperties(component, metadata, depth + 1);
        }
    }

    private static string GetComponentMarkdownLink(Component component)
    {
        if (component == null) return "`Unknown Component`";
        
        string componentName = component.GetType().Name;
        
        // Handle MonoBehaviour scripts with file linking
        if (component is MonoBehaviour monoBehaviour)
        {
            var script = MonoScript.FromMonoBehaviour(monoBehaviour);
            if (script != null)
            {
                string scriptPath = AssetDatabase.GetAssetPath(script);
                if (!string.IsNullOrEmpty(scriptPath))
                {
                    // Convert absolute asset path to relative path from Documentation/UnityExports/
                    string relativePath = GetRelativeScriptPath(scriptPath);
                    return $"[{script.name}.cs]({relativePath})";
                }
                else
                {
                    return $"`{script.name}.cs`";
                }
            }
            else
            {
                return $"`{componentName}`";
            }
        }
        
        // Handle built-in Unity components with documentation links
        if (IsBuiltInUnityComponent(component))
        {
            string unityDocsUrl = $"https://docs.unity3d.com/ScriptReference/{componentName}.html";
            return $"[{componentName}]({unityDocsUrl})";
        }
        
        // Fallback to monospace formatting
        return $"`{componentName}`";
    }

    private static string GetRelativeScriptPath(string assetPath)
    {
        // Convert from Assets/Scripts/... to ../../Scripts/...
        // Since exports are in Documentation/UnityExports/, we need to go up two levels
        if (assetPath.StartsWith("Assets/"))
        {
            return "../../" + assetPath.Substring(7); // Remove "Assets/" and add relative path
        }
        return assetPath;
    }

    private static bool IsBuiltInUnityComponent(Component component)
    {
        var type = component.GetType();
        var assembly = type.Assembly;
        
        // Check if the component is from Unity's assemblies
        return assembly.FullName.StartsWith("UnityEngine") || 
               assembly.FullName.StartsWith("UnityEditor") ||
               type.Namespace != null && type.Namespace.StartsWith("UnityEngine");
    }

    private static void ExportSerializedFields(Component component, StringBuilder metadata, int depth)
    {
        string indent = new string(' ', depth * 2);
        
        var serializedObject = new SerializedObject(component);
        var property = serializedObject.GetIterator();
        
        if (property.NextVisible(true))
        {
            do
            {
                // Skip script reference and other Unity internals
                if (property.name == "m_Script" || property.name.StartsWith("m_"))
                    continue;
                
                string valueString = GetSerializedPropertyValueString(property);
                if (!string.IsNullOrEmpty(valueString))
                {
                    metadata.AppendLine($"{indent}│   └── {property.displayName}: {valueString}");
                }
            }
            while (property.NextVisible(false));
        }
    }

    private static void ExportBuiltInComponentProperties(Component component, StringBuilder metadata, int depth)
    {
        string indent = new string(' ', depth * 2);
        
        // Export key properties for common Unity components
        // Note: More specific types must come before general types (CharacterController before Collider)
        switch (component)
        {
            case Renderer renderer:
                if (renderer.sharedMaterial != null)
                {
                    metadata.AppendLine($"{indent}│   └── Material: {renderer.sharedMaterial.name}");
                }
                break;
                
            case CharacterController controller:
                metadata.AppendLine($"{indent}│   └── Height: {controller.height:F2}");
                metadata.AppendLine($"{indent}│   └── Radius: {controller.radius:F2}");
                metadata.AppendLine($"{indent}│   └── Center: ({controller.center.x:F2},{controller.center.y:F2},{controller.center.z:F2})");
                break;
                
            case Collider collider:
                metadata.AppendLine($"{indent}│   └── IsTrigger: {collider.isTrigger}");
                if (collider is BoxCollider box)
                {
                    metadata.AppendLine($"{indent}│   └── Size: ({box.size.x:F2},{box.size.y:F2},{box.size.z:F2})");
                }
                else if (collider is SphereCollider sphere)
                {
                    metadata.AppendLine($"{indent}│   └── Radius: {sphere.radius:F2}");
                }
                break;
        }
    }

    private static string GetSerializedPropertyValueString(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue.ToString();
            case SerializedPropertyType.Boolean:
                return property.boolValue.ToString();
            case SerializedPropertyType.Float:
                return property.floatValue.ToString("F2");
            case SerializedPropertyType.String:
                return $"\"{property.stringValue}\"";
            case SerializedPropertyType.Vector2:
                var v2 = property.vector2Value;
                return $"({v2.x:F2},{v2.y:F2})";
            case SerializedPropertyType.Vector3:
                var v3 = property.vector3Value;
                return $"({v3.x:F2},{v3.y:F2},{v3.z:F2})";
            case SerializedPropertyType.Vector2Int:
                var v2i = property.vector2IntValue;
                return $"({v2i.x},{v2i.y})";
            case SerializedPropertyType.Vector3Int:
                var v3i = property.vector3IntValue;
                return $"({v3i.x},{v3i.y},{v3i.z})";
            case SerializedPropertyType.Rect:
                var rect = property.rectValue;
                return $"Rect({rect.x:F2},{rect.y:F2},{rect.width:F2},{rect.height:F2})";
            case SerializedPropertyType.Color:
                var color = property.colorValue;
                return $"Color({color.r:F2},{color.g:F2},{color.b:F2},{color.a:F2})";
            case SerializedPropertyType.ObjectReference:
                if (property.objectReferenceValue != null)
                {
                    return property.objectReferenceValue.name;
                }
                return "null";
            case SerializedPropertyType.Enum:
                if (property.enumNames != null && property.enumValueIndex >= 0 && property.enumValueIndex < property.enumNames.Length)
                {
                    return property.enumNames[property.enumValueIndex];
                }
                return "Unknown";
            case SerializedPropertyType.LayerMask:
                return property.intValue.ToString();
            default:
                return property.type ?? "Unknown";
        }
    }

    private static HashSet<string> CollectSceneDependencies(Scene scene)
    {
        var dependencies = new HashSet<string>();
        
        var rootObjects = scene.GetRootGameObjects();
        foreach (GameObject rootObj in rootObjects)
        {
            CollectGameObjectDependencies(rootObj, dependencies);
        }
        
        return dependencies;
    }

    private static HashSet<string> CollectPrefabDependencies(GameObject prefab)
    {
        var dependencies = new HashSet<string>();
        CollectGameObjectDependencies(prefab, dependencies);
        return dependencies;
    }

    private static void CollectGameObjectDependencies(GameObject obj, HashSet<string> dependencies)
    {
        if (obj == null) return;
        
        // Collect material dependencies
        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (renderer.sharedMaterial != null)
            {
                string materialPath = AssetDatabase.GetAssetPath(renderer.sharedMaterial);
                if (!string.IsNullOrEmpty(materialPath))
                {
                    dependencies.Add($"Material: {renderer.sharedMaterial.name} ({materialPath})");
                }
            }
        }
        
        // Collect script dependencies
        var monoBehaviours = obj.GetComponentsInChildren<MonoBehaviour>();
        foreach (var monoBehaviour in monoBehaviours)
        {
            if (monoBehaviour != null)
            {
                var script = MonoScript.FromMonoBehaviour(monoBehaviour);
                if (script != null)
                {
                    string scriptPath = AssetDatabase.GetAssetPath(script);
                    if (!string.IsNullOrEmpty(scriptPath))
                    {
                        dependencies.Add($"Script: {script.name}.cs ({scriptPath})");
                    }
                }
            }
        }
        
        // Collect texture dependencies from materials
        foreach (var renderer in renderers)
        {
            if (renderer.sharedMaterial != null)
            {
                var material = renderer.sharedMaterial;
                var texturePropertyNames = material.GetTexturePropertyNames();
                foreach (string propertyName in texturePropertyNames)
                {
                    var texture = material.GetTexture(propertyName);
                    if (texture != null)
                    {
                        string texturePath = AssetDatabase.GetAssetPath(texture);
                        if (!string.IsNullOrEmpty(texturePath))
                        {
                            dependencies.Add($"Texture: {texture.name} ({texturePath})");
                        }
                    }
                }
            }
        }
        
        // Collect audio dependencies
        var audioSources = obj.GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip != null)
            {
                string audioPath = AssetDatabase.GetAssetPath(audioSource.clip);
                if (!string.IsNullOrEmpty(audioPath))
                {
                    dependencies.Add($"Audio: {audioSource.clip.name} ({audioPath})");
                }
            }
        }
    }

    private static void ExportDependencies(StringBuilder metadata, HashSet<string> dependencies)
    {
        metadata.AppendLine("## Asset Dependencies");
        
        var materials = dependencies.Where(d => d.StartsWith("Material:")).ToList();
        var scripts = dependencies.Where(d => d.StartsWith("Script:")).ToList();
        var textures = dependencies.Where(d => d.StartsWith("Texture:")).ToList();
        var audio = dependencies.Where(d => d.StartsWith("Audio:")).ToList();
        
        if (materials.Any())
        {
            metadata.AppendLine("### Materials");
            foreach (string material in materials.OrderBy(m => m))
            {
                metadata.AppendLine($"- {material.Substring(10)}"); // Remove "Material: " prefix
            }
            metadata.AppendLine();
        }
        
        if (scripts.Any())
        {
            metadata.AppendLine("### Scripts");
            foreach (string script in scripts.OrderBy(s => s))
            {
                metadata.AppendLine($"- {script.Substring(8)}"); // Remove "Script: " prefix
            }
            metadata.AppendLine();
        }
        
        if (textures.Any())
        {
            metadata.AppendLine("### Textures");
            foreach (string texture in textures.OrderBy(t => t))
            {
                metadata.AppendLine($"- {texture.Substring(9)}"); // Remove "Texture: " prefix
            }
            metadata.AppendLine();
        }
        
        if (audio.Any())
        {
            metadata.AppendLine("### Audio");
            foreach (string audioClip in audio.OrderBy(a => a))
            {
                metadata.AppendLine($"- {audioClip.Substring(7)}"); // Remove "Audio: " prefix
            }
            metadata.AppendLine();
        }
        
        if (!dependencies.Any())
        {
            metadata.AppendLine("No external asset dependencies found.");
            metadata.AppendLine();
        }
    }

    private static void CreateExportDirectories()
    {
        try
        {
            if (!Directory.Exists(ExportBasePath))
            {
                Directory.CreateDirectory(ExportBasePath);
            }
            
            if (!Directory.Exists(ScenesPath))
            {
                Directory.CreateDirectory(ScenesPath);
            }
            
            if (!Directory.Exists(PrefabsPath))
            {
                Directory.CreateDirectory(PrefabsPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create export directories: {e.Message}");
        }
    }

    private void ApplyAutoExportSettings()
    {
        if (autoExportOnSave)
        {
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }
        else
        {
            EditorSceneManager.sceneSaved -= OnSceneSaved;
        }
        
        if (autoExportOnHierarchyChange)
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        else
        {
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        }
        
        Debug.Log($"Auto-export settings applied: OnSave={autoExportOnSave}, OnHierarchy={autoExportOnHierarchyChange}");
    }

    private static void OnSceneSaved(Scene scene)
    {
        Debug.Log($"Auto-exporting metadata for saved scene: {scene.name}");
        ExportSceneMetadata(scene);
    }

    private static void OnHierarchyChanged()
    {
        // Optional: Could implement logic to detect prefab changes and auto-export
        // For now, this is just a placeholder for future implementation
    }
}

/// <summary>
/// Auto-initialization class for setting up metadata export callbacks
/// Implements the auto-trigger requirements from UnityMetadataExport.mdc
/// </summary>
[InitializeOnLoad]
public class MetadataExporterAutoInit
{
    static MetadataExporterAutoInit()
    {
        // Auto-setup can be configured through the MetadataExporter window
        // Default to manual export to avoid unwanted automatic exports
        Debug.Log("MetadataExporter initialized. Use Tools > Export > Metadata Exporter Window to configure auto-export.");
    }
} 