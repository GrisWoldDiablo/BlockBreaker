/// <summary>
/// Description: Add a menu to the editor (BSGMenu > SceneS in build editor)
///              Build setting editor, add same scene multiple time in the build settings
/// Author: Alexandre Lepage
/// Date: 08 Oct 2018
/// GitHub: https://github.com/GrisWoldDiablo
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BSGEditorMenu : MonoBehaviour {
    
    [MenuItem("BSGMenu/Scenes in Build Generator")]
    static void GetGenerator()
    {
        Object asset = AssetDatabase.LoadAssetAtPath("Assets/BSG/BSG.prefab", typeof(Object));
        if (asset != null)
        {
            Selection.activeObject = asset;
        }
        else
        {
            Debug.Log("Assets/BSG/BSG.prefab | Not Found!");
        }
    }
}

public class BSGWindow : EditorWindow
{
    private Vector2 scrollBarPos;

    [MenuItem("BSGMenu/Scenes in Build Generator")]
    static void Init()
    {
        BSGWindow myWindow = (BSGWindow)GetWindow(typeof(BSGWindow));
        myWindow.titleContent = new GUIContent("B.S.G.");
    }

    void OnGUI()
    {
        scrollBarPos = GUILayout.BeginScrollView(scrollBarPos, false, true, GUILayout.ExpandHeight(true));
        GameObject asset = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/BSG/BSG.prefab", typeof(GameObject));
        if (asset != null)
        {
            BSGButtonScript targetComp = asset.GetComponent<BSGButtonScript>();
            if (targetComp != null)
            {
                var editor = Editor.CreateEditor(targetComp);
                editor.OnInspectorGUI();
            }
            else
            {
                Debug.Log("BSGButtonScript | Not Found!");
            }
        }
        else
        {
            Debug.Log("Assets/BSG/BSG.prefab | Not Found!");
        }
        GUILayout.EndScrollView();
    }
}
