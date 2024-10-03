using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPage : Page {
    private MonoBehaviour _monoBehaviour;

    public SettingsPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset) {}

    public void Initialize() {
        var userNameLabel = Root.Q<Label>("UserName");
        var userEmailLabel = Root.Q<Label>("UserEmail");
        var adminProductList = Root.Q<VisualElement>("AdminProductToEdit");

        UserManager userManager = UserManager.Instance;

        userNameLabel.text = $"{UserManager.Instance.Username}";   
        userEmailLabel.text = $"{UserManager.Instance.Email}";      

        if (userManager.IsAdmin) {
            adminProductList.style.display = DisplayStyle.Flex;
            PopulateAdminProducts(adminProductList);
        } else {
            adminProductList.style.display = DisplayStyle.None;
        }
    }
    
    private void PopulateAdminProducts(VisualElement adminProductList) {
        // 示例：添加管理员产品列表填充逻辑
        // 这里你可以使用实际的产品数据填充 adminProductList
    }
}