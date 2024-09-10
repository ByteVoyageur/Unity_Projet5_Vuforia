using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PagesManager : MonoBehaviour
{
    public UIDocument uiDocument;
    public VisualTreeAsset itemTemplate;
    public Dictionary<string, VisualTreeAsset> pageAssets = new Dictionary<string, VisualTreeAsset>();
    private Dictionary<string, Page> pagePool = new Dictionary<string, Page>();
    private CategorySO currentCategory;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        // Load VisualTreeAssets
        LoadPageAsset("HomePage");
        LoadPageAsset("SignInPage");
        LoadPageAsset("WelcomePage");
        LoadPageAsset("ItemDetailPage");
        LoadPageAsset("CategoryPage");
        LoadPageAsset("WishListPage");
        LoadPageAsset("WishListCartTemplate");
        LoadPageAsset("CategoryCartTemplate");
        LoadPageAsset("ItemCartTemplate");

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

    public void ShowPage(string pageName, object data = null)
    {
        foreach (var existingPage in pagePool.Values)
        {
            existingPage.Root.style.display = DisplayStyle.None;
        }

        Page page = null;

        if (pageName == "ItemDetailPage" && data is FurnitureSO furnitureData)
        {
            if (pageAssets.TryGetValue(pageName, out var visualTreeAsset))
            {
                page = new ItemDetailPage(visualTreeAsset);
                ((ItemDetailPage)page).Initialize(furnitureData, this);
            }
            else
            {
                Debug.LogError($"No VisualTreeAsset found for {pageName}");
                return;
            }
        }
        else
        {
            if (!pagePool.TryGetValue(pageName, out page))
            {
                if (pageAssets.TryGetValue(pageName, out var visualTreeAsset))
                {
                    page = CreatePageInstance(pageName, visualTreeAsset, data);
                    pagePool[pageName] = page;
                }
                else
                {
                    Debug.LogError($"No VisualTreeAsset found for {pageName}");
                    return;
                }
            }
        }

        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(page.Root);
        page.Root.style.display = DisplayStyle.Flex;
        Debug.Log($"Displayed page: {pageName}");
    }

    private Page CreatePageInstance(string pageName, VisualTreeAsset visualTreeAsset, object data = null)
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
                ((SignInPage)page).Initialize(this);
                break;
            case "WelcomePage":
                page = new WelcomePage(visualTreeAsset);
                ((WelcomePage)page).Initialize(this);
                AddButtonActions(page.Root, new Dictionary<string, System.Action>
                {
                    { "ARModeFooter", () => ShowCategoryPage("DefaultCategory") }  // default example
                });
                break;
            case "ItemDetailPage":
                page = new ItemDetailPage(visualTreeAsset);
                if (data is FurnitureSO furnitureData)
                {
                    ((ItemDetailPage)page).Initialize(furnitureData, this);
                }
                AddButtonActions(page.Root, new Dictionary<string, System.Action>
                {
                    { "ViewInARButton", ShowInAR }
                });
                break;
            case "CategoryPage":
                page = new CategoryPage(visualTreeAsset);
                if (data is CategorySO categoryData)
                {
                    ((CategoryPage)page).Initialize(this, categoryData);
                }
                else
                {
                    Debug.LogError("Category data is null for CategoryPage initialization.");
                }
                break;
            case "WishListPage":
                page = new WishListPage(visualTreeAsset);
                ((WishListPage)page).Initialize(this);
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
                    button.clicked -= kvp.Value;
                    button.clicked += kvp.Value;

                    button.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        Debug.Log($"Button {buttonName} PointerUpEvent captured");
                        kvp.Value.Invoke();
                    });

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

    public void ShowCategoryPage(string categoryName)
    {
        Debug.Log($"ShowCategoryPage called for {categoryName}");

        var categoryData = Resources.Load<CategorySO>($"Data/Category/{categoryName}");
    if (categoryData == null)
    {
        Debug.LogError($"Category data for {categoryName} not found.");
        return;
    }

        if (!pageAssets.TryGetValue("CategoryPage", out var visualTreeAsset))
    {
        Debug.LogError("VisualTreeAsset for CategoryPage could not be found.");
        return;
    }

        var categoryPage = CategoryPage.CreateInstance(visualTreeAsset);
        categoryPage.Initialize(this, categoryData);

        currentCategory = categoryData;

        uiDocument.rootVisualElement.Clear();
        uiDocument.rootVisualElement.Add(categoryPage.Root);
        Debug.Log("Created and displayed a new CategoryPage instance.");
    }

    public void ShowItemDetailPage(FurnitureSO itemData)
    {
        ShowPage("ItemDetailPage", itemData);
    }

    public CategorySO GetCurrentCategory()
    {
        return currentCategory;
    }

    private void ShowInAR()
    {
        Debug.Log("Footer AR button clicked.");
    }
}