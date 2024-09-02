using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PageManager : MonoBehaviour
{
    // Static instance to hold the single instance of PageManager
    private static PageManager _instance;

    // Public static property to provide global access to the instance
    public static PageManager Instance
    {
        get
        {
            // If the instance is not found, find or create one
            if (_instance == null)
            {
                _instance = FindObjectOfType<PageManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(PageManager).ToString());
                    _instance = singleton.AddComponent<PageManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    // Reference to the UI Document
    public UIDocument uiDocument;

    // Dictionary to store visual tree assets by their page name
    private Dictionary<string, VisualTreeAsset> pages = new Dictionary<string, VisualTreeAsset>();

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure that only one instance exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Initialize method to load pages and show the initial page
    private void Initialize()
    {
        uiDocument = GetComponent<UIDocument>();
        LoadPages();
        ShowPage("HomePage");
    }

    // LoadPages method to load all visual tree assets from the specified resources folder
    private void LoadPages()
    {
        // Load all VisualTreeAsset resources from StoreUIDocuments folder
        VisualTreeAsset[] loadedPages = Resources.LoadAll<VisualTreeAsset>("StoreUIDocuments");

        // Add each loaded page to the pages dictionary
        foreach (var page in loadedPages)
        {
            Debug.Log($"Loaded page: {page.name}");
            pages[page.name] = page;
        }
    }

    // ShowPage method to display the specified page
    public void ShowPage(string pageName)
    {
        Debug.Log($"Attempting to show page: {pageName}");
        
        // Try to get the page asset from the dictionary
        if (pages.TryGetValue(pageName, out var pageAsset))
        {
            // Clone and display the visual tree asset
            var newPage = pageAsset.CloneTree();
            uiDocument.rootVisualElement.Clear(); // Clear existing UI elements
            uiDocument.rootVisualElement.Add(newPage); // Add new page UI elements
            SetupButtons(newPage); // Setup button actions
            Debug.Log($"Successfully showed page: {pageName}");
        }
        else
        {
            Debug.LogWarning($"Page not found: {pageName}"); // Log a warning if the page is not found
        }
    }

    // SetupButtons method to initialize button click actions on the page
    private void SetupButtons(VisualElement root)
    {
        // Setup button actions by calling SetupButton method
        SetupButton(root, "LaterButton", () => ShowPage("WelcomePage"));
        SetupButton(root, "SignInButton", () => ShowPage("SignPage"));
    }

    // SetupButton method to attach click actions to buttons
    private void SetupButton(VisualElement root, string buttonName, System.Action clickAction)
    {
        var button = root.Q<Button>(buttonName); // Query the button by its name
        if (button != null)
        {
            // Ensure the click event is not registered multiple times
            button.clicked -= clickAction; 
            button.clicked += clickAction;
        }
        else
        {
            Debug.LogWarning($"Button not found: {buttonName}"); // Log a warning if the button is not found
        }
    }
}