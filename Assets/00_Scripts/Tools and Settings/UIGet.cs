using UnityEngine;

public static class UIGet 
{
    private static GameObject canvas = null;

    public static GameObject FindUIGameObject(string uiName)
    {
        if (canvas == null)
        {
            canvas = ObjGet.FindGameObject("Canvas");
        }
        if(canvas == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There is no Canvas in your scene");
#endif
            return null;
        }

        return ObjGet.FindChildGameObject(canvas, uiName);
    }

    public static T GetUIComponent<T>(GameObject container, string uiName) where T: Component
    {
        GameObject ChildObj = ObjGet.FindChildGameObject(container, uiName);
        if(ChildObj == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There is no [" + uiName + "] object in your scene!");
#endif
            return null;
        }

        T tempObj = ChildObj.GetComponent<T>();
        if(tempObj == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There is no [" + typeof(T) + "] component on " + uiName);
#endif
            return null;
        }

        return tempObj;
    }


}
