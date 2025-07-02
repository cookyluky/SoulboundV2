using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace SoulBound.Editor
{
    /// <summary>
    /// Editor utility to validate project layers and tags configuration
    /// Ensures all required layers and tags are properly set up for SoulBound RPG
    /// </summary>
    public class LayerTagValidator : EditorWindow
    {
        // Required layers for SoulBound project
        private static readonly Dictionary<int, string> RequiredLayers = new Dictionary<int, string>
        {
            { 2, "Ignore Raycast" },    // Unity built-in
            { 8, "Player" },
            { 9, "Enemy" },
            { 10, "Environment" },
            { 11, "UI" }
        };

        // Required tags for SoulBound project
        private static readonly string[] RequiredTags = 
        {
            "Player",
            "Enemy", 
            "NPC",
            "Interactable",
            "Projectile"
        };

        private Vector2 scrollPosition;
        private bool showValidation = true;
        private bool showInstructions = false;

        [MenuItem("SoulBound/Validate Layers & Tags")]
        public static void ShowWindow()
        {
            LayerTagValidator window = GetWindow<LayerTagValidator>("Layer & Tag Validator");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

        [MenuItem("SoulBound/Quick Validate")]
        public static void QuickValidate()
        {
            ValidateConfiguration();
        }

        private void OnGUI()
        {
            GUILayout.Label("SoulBound Layer & Tag Validator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Validation toggle
            showValidation = EditorGUILayout.Foldout(showValidation, "Validation Results", true);
            if (showValidation)
            {
                EditorGUILayout.BeginVertical("box");
                ValidateAndDisplayResults();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            // Instructions toggle
            showInstructions = EditorGUILayout.Foldout(showInstructions, "Setup Instructions", true);
            if (showInstructions)
            {
                EditorGUILayout.BeginVertical("box");
                DisplayInstructions();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Refresh Validation", GUILayout.Height(30)))
            {
                Repaint();
            }
        }

        private void ValidateAndDisplayResults()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // Validate Layers
            EditorGUILayout.LabelField("Layer Validation:", EditorStyles.boldLabel);
            bool allLayersValid = true;

            foreach (var requiredLayer in RequiredLayers)
            {
                int layerIndex = requiredLayer.Key;
                string expectedName = requiredLayer.Value;
                string actualName = LayerMask.LayerToName(layerIndex);

                if (string.IsNullOrEmpty(actualName) || actualName != expectedName)
                {
                    EditorGUILayout.HelpBox($"❌ Layer {layerIndex}: Expected '{expectedName}', Found '{actualName}'", 
                        MessageType.Error);
                    allLayersValid = false;
                }
                else
                {
                    EditorGUILayout.HelpBox($"✅ Layer {layerIndex}: '{expectedName}' configured correctly", 
                        MessageType.Info);
                }
            }

            EditorGUILayout.Space();

            // Validate Tags
            EditorGUILayout.LabelField("Tag Validation:", EditorStyles.boldLabel);
            bool allTagsValid = true;

            foreach (string requiredTag in RequiredTags)
            {
                if (IsTagDefined(requiredTag))
                {
                    EditorGUILayout.HelpBox($"✅ Tag '{requiredTag}' configured correctly", 
                        MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox($"❌ Tag '{requiredTag}' is missing", 
                        MessageType.Error);
                    allTagsValid = false;
                }
            }

            EditorGUILayout.Space();

            // Overall Status
            if (allLayersValid && allTagsValid)
            {
                EditorGUILayout.HelpBox("🎉 All layers and tags are configured correctly!", 
                    MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("⚠️ Configuration issues found. Please follow the setup instructions below.", 
                    MessageType.Warning);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DisplayInstructions()
        {
            EditorGUILayout.LabelField("Setup Instructions:", EditorStyles.boldLabel);
            
            EditorGUILayout.HelpBox(
                "1. Go to Edit → Project Settings → Tags and Layers\n\n" +
                "2. Add the following Layers:\n" +
                "   • Layer 8: Player\n" +
                "   • Layer 9: Enemy\n" +
                "   • Layer 10: Environment\n" +
                "   • Layer 11: UI\n" +
                "   (Note: Layer 2 'Ignore Raycast' should already exist)\n\n" +
                "3. Add the following Tags:\n" +
                "   • Player\n" +
                "   • Enemy\n" +
                "   • NPC\n" +
                "   • Interactable\n" +
                "   • Projectile\n\n" +
                "4. Configure Physics collision matrix in Edit → Project Settings → Physics:\n" +
                "   • Disable: UI with all other layers\n" +
                "   • Disable: Player with Projectile\n" +
                "   • Disable: Enemy with Enemy\n" +
                "   • Enable: Environment with Player, Enemy, Projectile\n" +
                "   • Disable: Ignore Raycast with all layers",
                MessageType.Info);

            if (GUILayout.Button("Open Tags and Layers Settings"))
            {
                SettingsService.OpenProjectSettings("Project/Tags and Layers");
            }

            if (GUILayout.Button("Open Physics Settings"))
            {
                SettingsService.OpenProjectSettings("Project/Physics");
            }
        }

        public static void ValidateConfiguration()
        {
            List<string> issues = new List<string>();

            // Check layers
            foreach (var requiredLayer in RequiredLayers)
            {
                int layerIndex = requiredLayer.Key;
                string expectedName = requiredLayer.Value;
                string actualName = LayerMask.LayerToName(layerIndex);

                if (string.IsNullOrEmpty(actualName) || actualName != expectedName)
                {
                    issues.Add($"Layer {layerIndex}: Expected '{expectedName}', found '{actualName}'");
                }
            }

            // Check tags
            foreach (string requiredTag in RequiredTags)
            {
                if (!IsTagDefined(requiredTag))
                {
                    issues.Add($"Missing tag: '{requiredTag}'");
                }
            }

            if (issues.Count == 0)
            {
                Debug.Log("✅ All layers and tags are configured correctly!");
            }
            else
            {
                Debug.LogWarning($"⚠️ Found {issues.Count} configuration issues:\n" + string.Join("\n", issues));
            }
        }

        private static bool IsTagDefined(string tag)
        {
            try
            {
                // This will throw an exception if the tag doesn't exist
                GameObject.FindGameObjectWithTag(tag);
                return true;
            }
            catch (UnityException)
            {
                // Check if tag exists in the tag list
                var tags = UnityEditorInternal.InternalEditorUtility.tags;
                return tags.Contains(tag);
            }
        }
    }
} 