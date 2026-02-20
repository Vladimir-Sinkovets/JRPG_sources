using Assets.Game.Scripts.BattleSystem.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Configs
{
    [CreateAssetMenu(fileName = "Tokens_config", menuName = "Battle/Tokens_config")]
    [DeclareBoxGroup("Sounds", Title = "Sounds")]
    public class TokensConfig : ScriptableObject
    {
        [Button(ButtonSizes.Large)]
        private void UpdateData()
        {
            if (Data == null)
            {
                Data = new List<TokenData>();
            }

            var allTokens = Enum.GetValues(typeof(Token)).Cast<Token>().ToList();

            var existingData = new Dictionary<Token, TokenData>();
            foreach (var item in Data)
            {
                if (item != null && !existingData.ContainsKey(item.Token))
                {
                    existingData.Add(item.Token, item);
                }
            }

            Data.RemoveAll(item => item == null || !allTokens.Contains(item.Token));

            foreach (var token in allTokens)
            {
                if (!existingData.ContainsKey(token))
                {
                    Data.Add(new TokenData()
                    {
                        Token = token,
                        Name = ObjectNames.NicifyVariableName(token.ToString()),
                        Description = string.Empty
                    });
                }
            }

            Data = Data
                .OrderBy(d => allTokens.IndexOf(d.Token))
                .ToList();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        [Group("Sounds")]
        public AudioClip HighlightComboSfx;
        [Group("Sounds")]
        public AudioClip MergeComboSfx;

        [TableList(Draggable = true,
            HideAddButton = true,
            HideRemoveButton = true,
            AlwaysExpanded = true)]
        public List<TokenData> Data;
    }

    [Serializable]
    public class TokenData
    {
        [SpritePreview(32.0f, true)]
        public Sprite Icon;
        public Token Token;

        public string Name;
        [TextArea(3, 5)]
        public string Description;
    }
}
