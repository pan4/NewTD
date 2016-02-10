
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerController), true)]
public class TowerControllerEditor : Editor
{
    private bool _damages;
    private bool _attackSpeedes;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TowerController towerController = target as TowerController;

        _damages = GUILayout.Toggle(_damages, "Damage");
        if (_damages)
        {
            for (int towerLevel = 0; towerLevel <= TowerController.MAX_TOWER_LEVEL; towerLevel++)
                towerController.Damage[(int)towerLevel] = EditorGUILayout.FloatField(towerLevel.ToString(), towerController.Damage[(int)towerLevel]);
        }

        _attackSpeedes = GUILayout.Toggle(_attackSpeedes, "Attack Speed");
        if (_attackSpeedes)
        {
            for (int towerLevel = 0; towerLevel <= TowerController.MAX_TOWER_LEVEL; towerLevel++)
                towerController.AttackSpeed[(int)towerLevel] = EditorGUILayout.FloatField(towerLevel.ToString(), towerController.AttackSpeed[(int)towerLevel]);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(towerController);
    }
}