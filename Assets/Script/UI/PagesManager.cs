using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PagesManager : MonoBehaviour
{
    public UIDocument uiDocument;
    private Dictionary<string, VisualTreeAsset> pageAssets = new Dictionary<string, VisualTreeAsset>();
    private Dictionary<string, Page> pagePool = new Dictionary<string, Page>();

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        // Load VisualTreeAssets
        LoadPageAsset("HomePage");
        LoadPageAsset("SignInPage");
        LoadPageAsset("WelcomePage");
        LoadPageAsset("ItemDetailPage");
        LoadPageAsset("CategoryPage");

        // Show initial page
        ShowPage("HomePage");
    }

    private void LoadPageAsset(string pageName)
    {
        VisualTreeAsset vta = Resources.Load<VisualTreeAsset>($"StoreUIDocuments/{pageName}");
        if (vta != null)
        {
            pageAssets[pageName] = vta;
            Debug.Log($"Loaded VisualTreeAsset for {pageName}");
        }
        else
        {
            Debug.LogError($"Failed to load VisualTreeAsset for {pageName}");
        }
    }

    public void ShowPage(string pageName)
    {
        // Deactivate all pages in the pool
        foreach (var existingPage in pagePool.Values)
        {
            existingPage.Root.style.display = DisplayStyle.None;
        }

        // Try to get the page from the pool
        if (!pagePool.TryGetValue(pageName, out var page))
        {
            // If page not in pool, create a new instance and add to pool
            if (pageAssets.TryGetValue(pageName, out var visualTreeAsset))
            {
                page = CreatePageInstance(pageName, visualTreeAsset);
                pagePool[pageName] = page;
            }
            else
            {
                Debug.LogError($"No VisualTreeAsset found for {pageName}");
                return;
            }
        }

        // Remove all existing children
        uiDocument.rootVisualElement.Clear();

        // Add the page to the root element and activate it
        uiDocument.rootVisualElement.Add(page.Root);
        page.Root.style.display = DisplayStyle.Flex;
        Debug.Log($"Displayed page: {pageName}");
    }

private Page CreatePageInstance(string pageName, VisualTreeAsset visualTreeAsset)
{
    Page page = null;

    switch (pageName)
    {
        case "HomePage":
            page = new HomePage(visualTreeAsset);
            AddButtonActions(page.Root, new Dictionary<string, System.Action>
            {
                { "SignInButton", () => ShowPage("SignInPage") },
                { "LaterButton", () => ShowPage("WelcomePage") }
            });
            break;

        case "SignInPage":
            page = new SignInPage(visualTreeAsset);
            ((SignInPage)page).Initialize(this);  // Initialize SignInPage with PagesManager reference
            break;

        case "WelcomePage":
            page = new WelcomePage(visualTreeAsset);
            ((WelcomePage)page).Initialize(this);  // Initialize WelcomePage with PagesManager reference
            AddButtonActions(page.Root, new Dictionary<string, System.Action>
            {
                { "ARModeFooter", () => ShowPage("CategoryPage") } // Add action for ARModeFooter
            });
            break;

        case "ItemDetailPage":
            page = new ItemDetailPage(visualTreeAsset);
            ((ItemDetailPage)page).Initialize();  // Initialize ItemDetailPage
            AddButtonActions(page.Root, new Dictionary<string, System.Action>
            {
                { "ViewInARButton", ShowInAR }
            });
            break;

        case "CategoryPage": // Add support for CategoryPage
            page = new CategoryPage(visualTreeAsset);
            ((CategoryPage)page).Initialize(this);  // Initialize CategoryPage with PagesManager reference
            break;

        default:
            Debug.LogError($"Unknown page name: {pageName}");
            break;
    }

    Debug.Log($"Created page instance for {pageName}");
    return page;
}

    private void AddButtonActions(VisualElement root, Dictionary<string, System.Action> buttonActions)
    {
        Debug.Log("Adding button actions");

        bool actionsAdded = false;

        root.RegisterCallback<GeometryChangedEvent>(evt =>
        {
            if (actionsAdded) return;

            foreach (var kvp in buttonActions)
            {
                var buttonName = kvp.Key;
                var button = root.Q<Button>(buttonName);

                if (button != null)
                {
                    Debug.Log($"Button {buttonName} found in {root.name}, position: {button.worldBound}");

                    // Print basic status
                    Debug.Log($"Button {buttonName} style display: {button.resolvedStyle.display}, enabled: {button.enabledSelf}");

                    // Ensure events are correctly registered
                    button.clicked -= kvp.Value;
                    button.clicked += kvp.Value;

                    // Add PointerUp event callback
                    button.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        Debug.Log($"Button {buttonName} PointerUpEvent captured");
                        kvp.Value.Invoke();
                    });

                    // Test event propagation
                    button.RegisterCallback<ClickEvent>(evt =>
                    {
                        Debug.Log($"ClickEvent registered on {buttonName}");
                    });
                }
                else
                {
                    Debug.LogWarning($"Button {buttonName} not found in {root.name}");
                }
            }

            actionsAdded = true;
        });
    }

    private void ShowInAR()
    {
        Debug.Log("View in AR button clicked.");
    }
}