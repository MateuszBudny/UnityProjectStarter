#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class GoogleAuthPrefsClearer
{
    [MenuItem("Tools/Clear Google Auth Preferences")]
    public static void ClearGoogleAuthPrefs()
    {
        string[] keys = new[] {
            "GoogleSheets_OAuth_Token",
            "GoogleSheets_OAuth_RefreshToken"
        };
        foreach(string key in keys)
        {
            if(EditorPrefs.HasKey(key))
            {
                EditorPrefs.DeleteKey(key);
                Debug.Log($"Deleted EditorPrefs key: {key}");
            }
        }
    }
}
#endif