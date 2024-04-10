using UnityEngine;

[CreateAssetMenu(fileName = "newObjectList",menuName ="ObjectList")]
public class objectList : ScriptableObject
{
    public objectInfo[] objects;
}

[System.Serializable]
public class objectInfo
{
    public GameObject Object = null;
    public string key = "";
    public Vector3 PositionOffset = Vector3.zero;
    public bool needRotationOffset = false;
    public Vector3 RotationOffset = Vector3.zero;
}
