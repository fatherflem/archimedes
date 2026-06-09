using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WhisperingCrashDM
{
    public class SceneSelectorPanel : MonoBehaviour
    {
        Transform content;
        void OnEnable() { AdventureEvents.CurrentSceneChanged += _ => Refresh(); }
        void OnDisable() { AdventureEvents.CurrentSceneChanged -= _ => Refresh(); }
        public void Build(AdventureManager manager)
        {
            content = UIFactoryHelpers.Scroll(transform, "Scene List").content;
            Refresh();
        }
        public void Refresh()
        {
            if (content == null || AdventureManager.Instance == null) return;
            UIFactoryHelpers.Clear(content);
            foreach (var group in AdventureManager.Instance.Scenes.GroupBy(s => s.category))
            {
                UIFactoryHelpers.Text(content, group.Key.ToString(), group.Key.ToString(), 20, FontStyle.Bold).color = new Color(.74f,1f,.75f);
                foreach (var scene in group)
                {
                    var selected = AdventureManager.Instance.CurrentScene == scene;
                    UIFactoryHelpers.Button(content, scene.displayName, () => AdventureManager.Instance.SetCurrentScene(scene.sceneId), selected ? new Color(.35f,.22f,.05f,.95f) : (Color?)null);
                }
            }
        }
    }
}
