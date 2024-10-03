using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPage : Page {
    private MonoBehaviour _monoBehaviour;

    public SettingsPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset) {
        _monoBehaviour = monoBehaviour;
    }

    public void Initialize() {
        var userNameLabel = Root.Q<Label>("UserName");
        var userEmailLabel = Root.Q<Label>("UserEmail");
        var adminProductList = Root.Q<VisualElement>("AdminProductToEdit");

        userNameLabel.text = $"Username: {UserManager.Instance.Username}";
        userEmailLabel.text = $"Email: {UserManager.Instance.Email}";

        if (UserManager.Instance.IsAdmin) {
            adminProductList.style.display = DisplayStyle.Flex;
            PopulateAdminProducts(adminProductList);
        } else {
            adminProductList.style.display = DisplayStyle.None;
        }

        var logOutButton = Root.Q<Button>("LogOutButton");
        if (logOutButton != null) {
            logOutButton.clicked += OnLogOutClicked;
        }
    }

    private void OnLogOutClicked() {
        UserManager.Instance.LogOut();
        Debug.Log("User logged out");
        ((PagesManager)_monoBehaviour).ShowPage("HomePage");
    }

    private void PopulateAdminProducts(VisualElement adminProductList) {
        // 填充管理员产品逻辑
    }
}