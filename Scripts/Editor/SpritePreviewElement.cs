#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TriInspector.Elements
{
    public class SpritePreviewElement : TriElement
    {
        private readonly TriProperty property;
        private readonly float previewSize;
        private readonly bool showBorder;

        public SpritePreviewElement(TriProperty property, float previewSize, bool showBorder)
        {
            this.property = property;
            this.previewSize = previewSize;
            this.showBorder = showBorder;
        }

        public override float GetHeight(float width)
        {
            var sprite = GetSpriteValue();
            if (sprite == null || sprite.texture == null) return 0;

            var ratio = (float)sprite.texture.width / sprite.texture.height;
            var height = ratio > 1 ? previewSize / ratio : previewSize;
            return height + 8f;
        }

        public override void OnGUI(Rect position)
        {
            var sprite = GetSpriteValue();
            if (sprite == null || sprite.texture == null) return;

            var ratio = (float)sprite.texture.width / sprite.texture.height;
            var width = ratio > 1 ? previewSize : previewSize * ratio;
            var height = ratio > 1 ? previewSize / ratio : previewSize;

            var spriteRect = new Rect(
                x: position.x + (position.width - width) * 0.5f,
                y: position.y + 4f,
                width: width,
                height: height
            );

            if (showBorder)
            {
                EditorGUI.DrawRect(new Rect(
                    spriteRect.x - 2,
                    spriteRect.y - 2,
                    spriteRect.width + 4,
                    spriteRect.height + 4),
                    new Color(0.2f, 0.2f, 0.2f, 0.5f));
            }

            GUI.DrawTexture(spriteRect, sprite.texture, ScaleMode.ScaleToFit);
        }

        private Sprite GetSpriteValue()
        {
            return property.Value as Sprite;
        }
    }
}
#endif