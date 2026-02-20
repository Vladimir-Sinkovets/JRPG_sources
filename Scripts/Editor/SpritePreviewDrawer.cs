#if UNITY_EDITOR
using TriInspector;
using TriInspector.Elements;
using TriInspector.Drawers;

[assembly: RegisterTriAttributeDrawer(typeof(SpritePreviewDrawer), TriDrawerOrder.Decorator)]

namespace TriInspector.Drawers
{
    public class SpritePreviewDrawer : TriAttributeDrawer<SpritePreviewAttribute>
    {
        public override TriElement CreateElement(TriProperty property, TriElement next)
        {
            if (!property.TryGetAttribute(out SpritePreviewAttribute attr))
                return next;

            var group = new TriElement();

            group.AddChild(next);

            group.AddChild(new SpritePreviewElement(
                property,
                attr.PreviewSize,
                attr.ShowBorder
            ));

            return group;
        }
    }
}
#endif