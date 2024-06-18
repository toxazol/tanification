using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Color characterColor;
    public AudioClip characterVoice;
    public Texture2D neutralSprite;

    public Texture2D GetSprite(string key)
    {
        switch (key)
        {
            case "neutral": return neutralSprite;
            default: return neutralSprite;
        }
    }

}
