using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpriteDrawer : PropertyDrawer
{

    private static GUIStyle s_TempStyle = new GUIStyle();
    public float maxSize = 128;
    public bool hasImage;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var ident = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect spriteRect;

        //create object field for the sprite
        spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.objectReferenceValue = EditorGUI.ObjectField(spriteRect, property.name, property.objectReferenceValue, typeof(Sprite), false);

        //if this is not a repain or the property is null exit now
        if (Event.current.type != EventType.Repaint || property.objectReferenceValue == null)
            return;

        //draw a sprite
        Sprite sp = property.objectReferenceValue as Sprite;
        if (sp != null)
        {
            spriteRect.x += 155;
            spriteRect.y += EditorGUIUtility.singleLineHeight + 4;
            if (sp.rect.width > sp.rect.height)
            {
                spriteRect.width = maxSize;
                spriteRect.height = maxSize / sp.rect.width * sp.rect.height;
            }
            else
            {
                spriteRect.height = maxSize;
                spriteRect.width = maxSize / sp.rect.height * sp.rect.width;
            }

            s_TempStyle.normal.background = sp.texture;
            s_TempStyle.Draw(spriteRect, GUIContent.none, false, false, false, false);
            EditorGUI.indentLevel = ident;
            hasImage = true;
        }
        else
        {
            hasImage = false;
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return hasImage ? base.GetPropertyHeight(property, label) + maxSize + 6 : 18;
    }
}
