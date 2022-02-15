/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

using UnityEngine.Serialization;

namespace Opsive.UltimateInventorySystem.UI.Audio
{
    using UnityEngine;

    /// <summary>
    /// A scriptable object used to map categories to Item Actions.
    /// </summary>
    [CreateAssetMenu(fileName = "ButtonAudio", menuName = "Ultimate Inventory System/UI/Button Audio", order = 51)]
    public class ButtonAudio : ScriptableObject
    {
        [FormerlySerializedAs("m_Click")]
        [Tooltip("The audio of clicking a button.")]
        [SerializeField] protected AudioClip m_ClickClip;
        [FormerlySerializedAs("m_Select")]
        [Tooltip("The audio of selecting a button.")]
        [SerializeField] protected AudioClip m_SelectClip;

        public AudioClip ClickClipClip => m_ClickClip;
        public AudioClip SelectClipClip => m_SelectClip;
    }
}