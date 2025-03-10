using UnityEditor;
using UnityEngine;

public class CreateFromHierarchyMenu_AdvancedButton
{
    [MenuItem("GameObject/\u2764 My UI/Advanced Button #%a", true, 0)]
    public static bool CreateFromHierarchyMenu()
    {
        return Selection.activeGameObject != null &&
               Selection.activeGameObject.GetComponentInParent<Canvas>() == true;
    }
    
    [MenuItem("GameObject/\u2764 My UI/Advanced Button #%a", false, 0)]
    public static void CreateFromHierarchyMenu(MenuCommand menuCommand)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/My UI/Advanced Button.prefab");
        if (prefab == null)
        {
            Debug.LogError("AdvancedButton.prefab not found at the specified path.");
            return;
        }
        
        GameObject parent = Selection.activeGameObject;
        
        if (menuCommand.context as GameObject != null)
            parent = menuCommand.context as GameObject;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        
        if (parent != null)
            GameObjectUtility.SetParentAndAlign(instance, parent);
        else
            Debug.LogWarning("No GameObject selected. Placing Advanced Button in the root.");
        
        Undo.RegisterCreatedObjectUndo(instance, "Create Advanced Button Prefab");
        
        Selection.activeGameObject = instance;
    }
}