using UnityEngine;
using UnityEngine.UI;

namespace WhisperingCrashDM
{
    public static class UIFactoryHelpers
    {
        static Font font;
        public static Font DefaultFont => font != null ? font : (font = Resources.GetBuiltinResource<Font>("Arial.ttf"));

        public static Text Text(Transform parent, string name, string value, int size = 22, FontStyle style = FontStyle.Normal)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
            var text = go.GetComponent<Text>(); text.font = DefaultFont; text.text = value; text.fontSize = size; text.fontStyle = style; text.color = new Color(0.92f, 0.96f, 1f); text.raycastTarget = false;
            return text;
        }

        public static Button Button(Transform parent, string label, System.Action onClick, Color? tint = null)
        {
            var go = new GameObject(label + " Button", typeof(RectTransform), typeof(Image), typeof(Button)); go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>(); image.color = tint ?? new Color(0.18f, 0.1f, 0.26f, 0.92f);
            var button = go.GetComponent<Button>(); button.onClick.AddListener(() => onClick?.Invoke());
            var layout = go.AddComponent<LayoutElement>(); layout.minHeight = 46;
            var text = Text(go.transform, "Label", label, 20, FontStyle.Bold); text.alignment = TextAnchor.MiddleCenter; text.color = Color.white;
            Stretch(text.rectTransform);
            return button;
        }

        public static VerticalLayoutGroup Vertical(Transform parent, string name, int spacing = 8)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter)); go.transform.SetParent(parent, false);
            var group = go.GetComponent<VerticalLayoutGroup>(); group.spacing = spacing; group.padding = new RectOffset(12, 12, 12, 12); group.childForceExpandWidth = true; group.childForceExpandHeight = false; group.childControlHeight = true; group.childControlWidth = true;
            var fitter = go.GetComponent<ContentSizeFitter>(); fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return group;
        }

        public static ScrollRect Scroll(Transform parent, string name)
        {
            var root = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(ScrollRect)); root.transform.SetParent(parent, false);
            Stretch(root.GetComponent<RectTransform>());
            root.GetComponent<Image>().color = new Color(0, 0, 0, 0.12f);
            var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask)); viewport.transform.SetParent(root.transform, false); viewport.GetComponent<Image>().color = new Color(0,0,0,0.01f); viewport.GetComponent<Mask>().showMaskGraphic = false; Stretch(viewport.GetComponent<RectTransform>());
            var content = Vertical(viewport.transform, "Content"); content.GetComponent<RectTransform>().anchorMin = new Vector2(0,1); content.GetComponent<RectTransform>().anchorMax = new Vector2(1,1); content.GetComponent<RectTransform>().pivot = new Vector2(0.5f,1); content.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
            var scroll = root.GetComponent<ScrollRect>(); scroll.viewport = viewport.GetComponent<RectTransform>(); scroll.content = content.GetComponent<RectTransform>(); scroll.horizontal = false;
            return scroll;
        }

        public static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        public static void Clear(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--) Object.Destroy(parent.GetChild(i).gameObject);
        }
    }
}
