using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SettingsPage : Page
{
    private MonoBehaviour _monoBehaviour;
    private int currentProductId;

    public SettingsPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void Initialize()
    {
        var userNameLabel = Root.Q<Label>("UserName");
        var userEmailLabel = Root.Q<Label>("UserEmail");
        var adminProductList = Root.Q<VisualElement>("AdminEditor");
        var inputNewPriceContainer = Root.Q<VisualElement>("InputNewPriceContainer");
        var submitButton = inputNewPriceContainer.Q<Button>("SubmitButton");
        var priceField = inputNewPriceContainer.Q<DoubleField>();

        inputNewPriceContainer.style.display = DisplayStyle.None; 

        userNameLabel.text = $"Username: {UserManager.Instance.Username}";
        userEmailLabel.text = $"Email: {UserManager.Instance.Email}";

        if (UserManager.Instance.IsAdmin)
        {
            adminProductList.style.display = DisplayStyle.Flex;
            PopulateAdminProducts(adminProductList);
        }
        else
        {
            adminProductList.style.display = DisplayStyle.None;
        }

        submitButton.clicked += () => OnPriceSubmit(priceField, inputNewPriceContainer);
        
        var logOutButton = Root.Q<Button>("LogOutButton");
        if (logOutButton != null)
        {
            logOutButton.clicked += OnLogOutClicked;
        }
    }

    private void OnLogOutClicked()
    {
        UserManager.Instance.LogOut();
        Debug.Log("User logged out");
        ((PagesManager)_monoBehaviour).ShowPage("HomePage");
    }

    private void PopulateAdminProducts(VisualElement adminProductList)
    {
        _monoBehaviour.StartCoroutine(FetchProducts(adminProductList));
    }

    private IEnumerator FetchProducts(VisualElement adminProductList)
    {
        string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php/products";
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(www.downloadHandler.text);
                DisplayProducts(adminProductList, products);
            }
            else
            {
                Debug.LogError("Failed to fetch products: " + www.error);
            }
        }
    }

    private void DisplayProducts(VisualElement adminProductList, List<Product> products)
    {
        adminProductList.Clear();
        foreach (var product in products)
        {
            var productElement = new VisualElement();
            productElement.AddToClassList("admin-product-to-edit");

            var idLabel = new Label($"ID: {product.product_id}");
            idLabel.AddToClassList("id-product");
            productElement.Add(idLabel);

            var nameLabel = new Label($"Product Name: {product.name}");
            nameLabel.AddToClassList("name-product");
            productElement.Add(nameLabel);

            var priceLabel = new Label($"Price: {product.price}");
            priceLabel.AddToClassList("price-product");
            productElement.Add(priceLabel);

            var editButton = new Button(() => EditProduct(product))
            {
                text = "Edit"
            };
            editButton.AddToClassList("button-to-edit");
            productElement.Add(editButton);

            adminProductList.Add(productElement);
        }
    }

    private void EditProduct(Product product)
    {
        var inputContainer = Root.Q<VisualElement>("InputNewPriceContainer");
        var priceField = inputContainer.Q<DoubleField>();
        priceField.value = product.price; 
        inputContainer.style.display = DisplayStyle.Flex; 
        currentProductId = product.product_id; 
    }

    private void OnPriceSubmit(DoubleField priceField, VisualElement inputContainer)
    {
        double newPrice = priceField.value;
        _monoBehaviour.StartCoroutine(UpdateProductPrice(currentProductId, (float)newPrice));
        inputContainer.style.display = DisplayStyle.None; 
    }

private IEnumerator UpdateProductPrice(int productId, float newPrice)
{
    string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php";

    var data = new
    {
        action = "modify_product",  
        userName = UserManager.Instance.Username,  
        product_id = productId,
        price = newPrice
    };

    var jsonData = JsonConvert.SerializeObject(data);
    var www = new UnityWebRequest(apiUrl, "POST");
    www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
    www.uploadHandler.contentType = "application/json";
    www.downloadHandler = new DownloadHandlerBuffer();

    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        var responseText = www.downloadHandler.text;
        Debug.Log("Server response: " + responseText);

        var responseJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
        if (responseJson.ContainsKey("message"))
        {
            Debug.Log(responseJson["message"]);
        }
        else if (responseJson.ContainsKey("error"))
        {
            Debug.LogError("Error from server: " + responseJson["error"]);
        }
    }
    else
    {
        Debug.LogError("Failed to update product price: " + www.error);
    }
}



}

public class Product
{
    public int product_id { get; set; }
    public string name { get; set; }
    public float price { get; set; }
}