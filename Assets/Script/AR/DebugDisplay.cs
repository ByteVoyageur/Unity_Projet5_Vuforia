using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    private string logContent = "";
    private Vector2 scrollPosition = Vector2.zero;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logContent += logString + "\n";
    }

    void OnGUI()
    {
        GUIStyle logStyle = new GUIStyle();
        logStyle.fontSize = 24;   
        logStyle.normal.textColor = Color.white;  
        logStyle.wordWrap = true; 

        float logWidth = Screen.width * 0.4f;
        float logHeight = Screen.height * 0.5f;
        float topMargin = Screen.height * 0.1f; 
        float leftMargin = Screen.width * 0.05f;

        GUILayout.BeginArea(new Rect(leftMargin, topMargin, logWidth, logHeight));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(logWidth), GUILayout.Height(logHeight));
        GUILayout.Label(logContent, logStyle);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}