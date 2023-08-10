using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[ExecuteInEditMode]
[CustomEditor(typeof(Writer))]
public class WritingEditor : Editor
{

    [SerializeField] static float thickness = 0.1f;
    [SerializeField] static float scale = 1.0f;
    [SerializeField] static float width = 0.5f;
    [SerializeField] static float rotation = 0f;
    [SerializeField] static float shearValue = 1f;
    [SerializeField] static float inflate= 1f;


    private void OnEnable()
    {
        Writer myScript = (Writer)target;
        thickness = myScript._thickness;
        scale = myScript._scale;
        width = myScript._width;
        rotation = myScript._rotation;
        shearValue = myScript._shear;
    }

    public override void OnInspectorGUI()
    {
        GUI.skin.button.wordWrap = true;

        serializedObject.Update();
        Texture TextureLogo = (Texture)AssetDatabase.LoadAssetAtPath("Assets/3DText/Editor/Capture.png", typeof(Texture));
        GUILayout.Box(TextureLogo, GUILayout.MaxWidth(Screen.width), GUILayout.MinWidth(Screen.width * 0.1f)/*,GUILayout.MaxHeight(Screen.height * 0.1f)*/, GUILayout.MinHeight(Screen.height * 0.125f));

        base.OnInspectorGUI();

        Writer myScript = (Writer)target;

        EditorGUILayout.LabelField("Create", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal("Create");
        GUI.backgroundColor = new Color(3/100,255/100,119/100,0.5f);
        if (GUILayout.Button("Build Word", GUILayout.Height(50)))
        {
            myScript.ClearWord();
            myScript.CreateWord();
        }
        GUI.backgroundColor = new Color(222 / 100, 59 / 100, 18 / 100, 0.5f);
        if (GUILayout.Button("Clear Word", GUILayout.Height(50)))
        {
            myScript.ClearWord();
        }

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = new Color(3 / 100, 255 / 100, 119 / 100, 0.5f);
        EditorGUILayout.LabelField("Editables", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Depth");
        thickness = EditorGUILayout.Slider(thickness, 0.1f, 20.0f);

        EditorGUILayout.LabelField("Shear");
        shearValue = EditorGUILayout.Slider(shearValue, 0.1f, 1.0f);

        EditorGUILayout.LabelField("Scale");
        scale = EditorGUILayout.Slider(scale, 1.0f, 50.0f);

        EditorGUILayout.LabelField("Spacing");
        width = EditorGUILayout.Slider(width, 0.1f, 2.0f);

        EditorGUILayout.LabelField("Rotation");
        rotation = EditorGUILayout.Slider(rotation, -90.0f, 90.0f);

        myScript.Scale(shearValue,scale,thickness);
        myScript.width(width);
        myScript.Rotation(rotation);
        

        EditorGUILayout.LabelField("Animation Module", EditorStyles.boldLabel);
        
        GUI.backgroundColor = new Color(3 / 100, 255 / 100, 119 / 100, 0.5f);
        EditorGUILayout.BeginHorizontal("Modules");
        if (GUILayout.Button("Add Module to Word", GUILayout.Height(50)))
        {
            myScript.AddModuleToWord();
        }

        if (GUILayout.Button("Add Module to Letters", GUILayout.Height(50)))
        {
            myScript.AddModuleToLetters();
        }
        
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = new Color(222 / 100, 59 / 100, 18 / 100, 0.5f);


        if (GUILayout.Button("Remove Modules", GUILayout.Height(25)))
        {
            myScript.RemoveModules();
        }

        GUI.backgroundColor = new Color(3 / 100, 255 / 100, 119 / 100, 0.5f);
        EditorGUILayout.LabelField("Rigidbodys", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal("Rigidbodys");
        if (GUILayout.Button("Add Rigidbody to Word", GUILayout.Height(50)))
        {
            myScript.addRigidbodyToWord();
        }

        if (GUILayout.Button("Add Rigibody to Letters", GUILayout.Height(50)))
        {
            myScript.addRigidbodyToLetters();
        }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = new Color(222 / 100, 59 / 100, 18 / 100, 0.5f);
        
        if (GUILayout.Button("Remove Rigidbodys", GUILayout.Height(25)))
        {
            myScript.RemoveRigidbodys();
        }

        GUI.backgroundColor = new Color(3 / 100, 255 / 100, 119 / 100, 0.5f);
EditorGUILayout.LabelField(" ", EditorStyles.boldLabel);
        if (GUILayout.Button("Complete", GUILayout.Height(50)))
        {
            myScript.complete();
        }
    }



}
