using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gun))]
public class GunsEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        Gun gun = (Gun)target;
        base.OnInspectorGUI();
        switch (gun.type)
        {
            case Gun.GunType.Normal:
                gun.damage = EditorGUILayout.FloatField("Damage", gun.damage);
                gun.ammoWep = EditorGUILayout.Slider("Ammo Take Value", gun.ammoWep, .01f,1f);
                gun.range = EditorGUILayout.FloatField("Gun Range", gun.range);
                gun.particles = (GameObject)EditorGUILayout.ObjectField("Shoot particle", gun.particles, typeof(GameObject), true);
                break;
            case Gun.GunType.Bomb:
                gun.Spawn = (GameObject)EditorGUILayout.ObjectField("Object To Span On Land", gun.Spawn, typeof(GameObject), true);
                break;
            case Gun.GunType.Medicine:
                gun.animator = (Animator)EditorGUILayout.ObjectField("Animator",gun.animator,typeof(Animator),true);
                gun.Heal = EditorGUILayout.Knob(new Vector2(125, 75), gun.Heal, .01f, 1f, "Heals", Color.black, Color.blue, true);
                break;
            default:
                Debug.Log(gun.type + " was not defined in GunsEditor.cs");
                break;
        }
    }
}