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
        // 设置字体样式
        GUIStyle logStyle = new GUIStyle();
        logStyle.fontSize = 24;   // 设置字体大小
        logStyle.normal.textColor = Color.white;  // 设置字体颜色
        logStyle.wordWrap = true; // 自动换行

        // 设置显示区域大小和位置
        float logWidth = Screen.width * 0.8f;
        float logHeight = Screen.height * 0.5f;
        float topMargin = Screen.height * 0.1f; // 顶部10%位置

        GUILayout.BeginArea(new Rect((Screen.width - logWidth) / 2, topMargin, logWidth, logHeight));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(logWidth), GUILayout.Height(logHeight));
        GUILayout.Label(logContent, logStyle);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}