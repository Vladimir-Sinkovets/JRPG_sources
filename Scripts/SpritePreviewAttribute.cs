using System;

namespace TriInspector
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpritePreviewAttribute : Attribute
    {
        public float PreviewSize { get; private set; } = 64f;
        public bool ShowBorder { get; private set; } = true;

        public SpritePreviewAttribute() { }

        public SpritePreviewAttribute(float previewSize)
        {
            PreviewSize = previewSize;
        }

        public SpritePreviewAttribute(bool showBorder)
        {
            ShowBorder = showBorder;
        }

        public SpritePreviewAttribute(float previewSize, bool showBorder)
        {
            PreviewSize = previewSize;
            ShowBorder = showBorder;
        }
    }
}