using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGen))]
public class MapGenButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGen generator = (MapGen)target;
        if (GUILayout.Button("맵 생성"))
        {
            generator.DeleteMap();
            generator.BuildMap();
        }

        if (GUILayout.Button("맵 삭제"))
        {
            generator.DeleteMap();
        }
    }
}
