using UnityEngine.UIElements;

public class Page
{
    public VisualElement Root { get; private set; }

    public Page(VisualTreeAsset visualTreeAsset)
    {
        Root = visualTreeAsset.CloneTree();
    }
}