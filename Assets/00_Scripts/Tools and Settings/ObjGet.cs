using UnityEngine;

public static class ObjGet 
{
    public static GameObject FindGameObject(string name)
    {
        GameObject TempObj = GameObject.Find(name);
        if(TempObj == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Can't find GameObject [" + name + "]!");
#endif
            return null;
        }
        return TempObj;
    }

    public static GameObject FindChildGameObject(GameObject parent, string objName)
    {
        if(parent == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Parent is null!");
#endif
            return null;
        }

        Transform FindingObj = null;

        if(parent.name == objName)
        {
            FindingObj = parent.transform;
        }
        else
        {
            Transform[] allChildren = parent.transform.GetComponentsInChildren<Transform>();
            foreach(Transform child in allChildren)
            {
                if(child.name == objName)
                {
                    if(FindingObj == null)
                    {
                        FindingObj = child;
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogWarning(parent.name + " has mutiple child objects with same name");
#endif
                    }
                }
            }
        }

        if(FindingObj == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Parent " + parent.name + " has no child named " + objName);
#endif
            return null;
        }
        return FindingObj.gameObject;
    }
}
