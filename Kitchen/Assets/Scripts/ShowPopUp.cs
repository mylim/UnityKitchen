using UnityEngine;
using System.Collections;

public class ShowPopUp : UnityEditor.EditorWindow
{

    [UnityEditor.MenuItem("Example/ShowPopup Example")]
    static void Init()
    {
        ShowPopUp window = ScriptableObject.CreateInstance<ShowPopUp>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        UnityEditor.EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup", UnityEditor.EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("Agree!")) this.Close();
    }
}
