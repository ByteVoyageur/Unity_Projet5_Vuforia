using UnityEngine.UIElements;

public class Page
{
    public VisualElement Root { get; private set; }

    public Page(VisualTreeAsset visualTreeAsset)
    {
        Root = visualTreeAsset.CloneTree();
    }

    public virtual void OnNavigatedTo(PagesManager pagesManager)
    {}
}