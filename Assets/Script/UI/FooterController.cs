using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

    public static class FooterController
    {
        // Initialize all buttons fo Footer
        public static void InitializeFooter (VisualElement root, PagesManager pagesManager)
        {
            var homeFooter = root.Q<VisualElement>("HomeFooter");
            if(homeFooter != null)
            {
                homeFooter.RegisterCallback<ClickEvent>(evt =>
                {pagesManager.ShowPage("HomePage");}
                );
            }
            else
            {
                Debug.LogError("HomeFooter element not found.");
            }

            // function of Setting button Footer
            // Function of AR button footer
            // function of wish list button footer
        }
    }
