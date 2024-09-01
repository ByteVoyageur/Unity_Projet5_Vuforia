using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Carousel : MonoBehaviour
{
    private const string ussScrollView = "promotions-container";

    private ScrollView scrollView;

    private int currentIndex;
    private List<VisualElement> currentContent = new List<VisualElement>();

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Locate the carousel container
        scrollView = root.Q<ScrollView>("PromotionsContainer");

        // Ensure the ScrollView is set to horizontal scrolling
        scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

        // Initialize content from UXML
        currentContent = new List<VisualElement>(scrollView.Children());

        // Create navigation buttons (You will have to adjust the positions of these buttons in the UI)
        Button leftButton = new Button();
        leftButton.text = "<";
        Button rightButton = new Button();
        rightButton.text = ">";

        // Style and place buttons in UI (Optional styling, you can adjust positions as needed)
        leftButton.style.position = Position.Absolute;
        leftButton.style.left = 0;
        rightButton.style.position = Position.Absolute;
        rightButton.style.right = 0;

        root.Add(leftButton);
        root.Add(rightButton);

        leftButton.clicked += () => OnLeftClicked();
        rightButton.clicked += () => OnRightClicked();

        // Set initial index
        currentIndex = 0;
    }

    private void OnLeftClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            scrollView.ScrollTo(currentContent[currentIndex]);
        }
    }

    private void OnRightClicked()
    {
        if (currentIndex < currentContent.Count - 1)
        {
            currentIndex++;
            scrollView.ScrollTo(currentContent[currentIndex]);
        }
    }
}