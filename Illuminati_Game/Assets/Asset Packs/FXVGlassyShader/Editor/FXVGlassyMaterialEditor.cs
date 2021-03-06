using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public enum BLEND_MODE_OPTIONS
{
    FXV_ALPHA_BLEND = 0,
    FXV_ADDITIVE_BLEND = 1,
}

public class FXVGlassyMaterialEditor : ShaderGUI 
{
    bool firstTimeApply = true;

    MaterialEditor materialEditor;

    MaterialProperty _ColorProperty = null;
    MaterialProperty _TransparencyRimMinProperty = null;
    MaterialProperty _TransparencyRimMaxProperty = null;

    MaterialProperty _MainTexProperty = null;
    MaterialProperty _MainTexColorProperty = null;

    MaterialProperty _TextureRimMinProperty = null;
    MaterialProperty _TextureRimMaxProperty = null;

    MaterialProperty _EnviroTexProperty = null;
    MaterialProperty _ReflectionColorRProperty = null;
    MaterialProperty _ReflectionColorGProperty = null;
    MaterialProperty _ReflectionColorBProperty = null;

    MaterialProperty _NormalTexProperty = null;

    MaterialProperty _RefractionScaleProperty = null;
    MaterialProperty _RefractionFromNormalProperty = null;

    MaterialProperty _SubsurfaceOffsetProperty = null;
    MaterialProperty _SubsurfacePowerProperty = null;

    public void FindProperties(MaterialProperty[] props)
    {
        _ColorProperty = FindProperty("_Color", props);

        _TransparencyRimMinProperty = FindProperty("_TransparencyRimMin", props, false);
        _TransparencyRimMaxProperty = FindProperty("_TransparencyRimMax", props, false);

        _MainTexProperty = FindProperty("_MainTex", props);
        _MainTexColorProperty = FindProperty("_MainTexColor", props);

        _TextureRimMinProperty = FindProperty("_TextureRimMin", props);
        _TextureRimMaxProperty = FindProperty("_TextureRimMax", props);

        _EnviroTexProperty = FindProperty("_Enviro", props);
        _ReflectionColorRProperty = FindProperty("_ReflectionColorR", props);
        _ReflectionColorGProperty = FindProperty("_ReflectionColorG", props);
        _ReflectionColorBProperty = FindProperty("_ReflectionColorB", props);

        _NormalTexProperty = FindProperty("_Normal", props);

        _RefractionScaleProperty = FindProperty("_RefractionStrength", props, false);
        _RefractionFromNormalProperty = FindProperty("_RefractionFromNormal", props, false);

        _SubsurfaceOffsetProperty = FindProperty("_SubsurfaceOffset", props);
        _SubsurfacePowerProperty = FindProperty("_SubsurfacePower", props);
    }

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly

        materialEditor = editor;
        Material material = materialEditor.target as Material;

        // Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
        // material to a standard shader.
        // Do this before any GUI code has been issued to prevent layout issues in subsequent GUILayout statements (case 780071)
        if (firstTimeApply)
        {
            //MaterialChanged(material, m_WorkflowMode);
            firstTimeApply = false;
        }

        ShaderPropertiesGUI(material);
    }

    public void ShaderPropertiesGUI(Material targetMat)
    {
        string[] keyWords = targetMat.shaderKeywords;

        bool useMainTexture = keyWords.Contains("USE_MAIN_TEXTURE");
        bool useNormalTexture = keyWords.Contains("USE_NORMAL_MAP");
        bool useEnviroTexture = keyWords.Contains("USE_ENVIRO_MAP");
        bool refractionEnabled = keyWords.Contains("USE_REFRACTION");
        bool subsurfaceScatteringEnabled = keyWords.Contains("USE_SUBSURFACE_SCATTERING");

        EditorGUI.BeginChangeCheck();

        targetMat.SetTexture("_CameraDepthTexture", EditorGUIUtility.whiteTexture);

        GUILayout.Label("Color", EditorStyles.boldLabel);

        materialEditor.ColorProperty(_ColorProperty, "Color");

        materialEditor.ShaderProperty(_TransparencyRimMinProperty, "Transparency Rim Min");
        materialEditor.ShaderProperty(_TransparencyRimMaxProperty, "Transparency Rim Max");

        if (_RefractionScaleProperty != null)
        {
            GUILayout.Label("Refraction", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(_RefractionScaleProperty, "Enviro Refraction");
            materialEditor.ShaderProperty(_RefractionFromNormalProperty, "Normals Refraction");
        }

        GUILayout.Label("Texture", EditorStyles.boldLabel);

        useMainTexture = EditorGUILayout.Toggle("Enabled", useMainTexture);

        if (useMainTexture)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent("Texture", ""), _MainTexProperty);
            materialEditor.ColorProperty(_MainTexColorProperty, "Color");
            materialEditor.ShaderProperty(_TextureRimMinProperty, "Texture Rim Min");
            materialEditor.ShaderProperty(_TextureRimMaxProperty, "Texture Rim Max");
            materialEditor.TextureScaleOffsetProperty(_MainTexProperty);
        }

        GUILayout.Label("Enviro", EditorStyles.boldLabel);

        useEnviroTexture = EditorGUILayout.Toggle("Enabled", useEnviroTexture);

        if (useEnviroTexture)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent("Texture", ""), _EnviroTexProperty);
            materialEditor.ColorProperty(_ReflectionColorRProperty, "Reflection Color 1");
            materialEditor.ColorProperty(_ReflectionColorGProperty, "Reflection Color 2");
            materialEditor.ColorProperty(_ReflectionColorBProperty, "Reflection Color 3");
        }

        GUILayout.Label("Normal", EditorStyles.boldLabel);

        useNormalTexture = EditorGUILayout.Toggle("Enabled", useNormalTexture);

        if (useNormalTexture)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent("Texture", ""), _NormalTexProperty);
            materialEditor.TextureScaleOffsetProperty(_NormalTexProperty);
        }

        GUILayout.Label("Subsurface Scattering", EditorStyles.boldLabel);

        subsurfaceScatteringEnabled = EditorGUILayout.Toggle("Enabled", subsurfaceScatteringEnabled);

        if (subsurfaceScatteringEnabled)
        {
            materialEditor.ShaderProperty(_SubsurfaceOffsetProperty, "Dispersion");
            materialEditor.ShaderProperty(_SubsurfacePowerProperty, "Power");
        }


        if (EditorGUI.EndChangeCheck())
        {
            List<string> keywords = new List<string>();

            if (useMainTexture)
                keywords.Add("USE_MAIN_TEXTURE");

            if (useNormalTexture)
                keywords.Add("USE_NORMAL_MAP");

            if (useEnviroTexture)
                keywords.Add("USE_ENVIRO_MAP");

            if (refractionEnabled)
                keywords.Add("USE_REFRACTION");

            if (subsurfaceScatteringEnabled)
                keywords.Add("USE_SUBSURFACE_SCATTERING");

            targetMat.shaderKeywords = keywords.ToArray();
            EditorUtility.SetDirty(targetMat);
        }
    }
}
