![Projet5_03](https://github.com/user-attachments/assets/87c91be9-8c73-4b31-8225-fa49461ed8d5)![Projet5_06](https://github.com/user-attachments/assets/bf7fc6e3-26b1-4560-97f8-cf9697322537)

DecoMaison is an augmented reality (AR)-based furniture display and shopping application. This README file aims to introduce the technical implementation and key functional modules of the project to developers.

### Project Upgrade

Previously, product data was retrieved using Scriptable Object. After the upgrade, data is provided by a PHP/SQL backend and accessed via API, including product ID, name, description, and price. User login and registration features are also managed through the backend server, which is set up by the author using Ubuntu, Apache, SSL, and other technologies. The API documentation is available [here](https://xiaosong.fr/decomaison/api/swagger-ui-master/index.html).

### Feature Overview

The main features of DecoMaison include:

- User registration and login
- Product category browsing
- Product detail viewing
- Wishlist management
- AR (Augmented Reality) functionality for displaying and placing furniture

### Technical Implementation Overview

### Page Management

The project uses a page management system to handle the various pages within the application. Each page is constructed using Unity's UI Toolkit, and the page management system is responsible for:

- Loading and initializing page resources
- Showing and hiding pages
- Switching pages based on user interactions

The page management system is designed with a component-based architecture, treating each page as an independent module. It provides a unified interface for page transitions and data passing.

### User Interface (UI)

The user interface consists of multiple independent pages, including the home page, welcome page, product detail page, category page, wishlist page, and shopping cart page. Each page is dynamically loaded and displayed via the resource management system, allowing for flexible content adaptation. The interaction behavior of UI elements (such as button clicks) is handled and feedbacks are provided through an event system.

### Data Management

The application's data mainly includes product data, user data, category data, and wishlist data. Data is stored and managed using ScriptableObjects, offering good scalability and usability. User-specific data like wishlists and shopping carts are managed through a singleton pattern to ensure data consistency and real-time updates.

### Wishlist Functionality

The wishlist management feature allows users to add items of interest to a wishlist for later viewing and operation. The wishlist system supports adding, removing, and viewing items and is tightly integrated with the page management system to reflect wishlist changes in the UI in real time.

### AR Functionality

AR functionality is a core feature of DecoMaison, allowing users to place virtual furniture in their real environments. Key functionalities include:

- **Object Placement**: Recognizing planes to locate furniture positions, allowing users to place furniture in their real environment.
- **Object Selection and Preview**: Users can select furniture from the wishlist for preview and placement.
- **Double-Tap to Cancel Placement**: Users can double-tap placed objects to cancel placement.
- **Object Dragging and Rotating**: Users can drag and rotate virtual furniture to find the best position.
- **Information Panel Display**: Long-pressing items shows an information panel, providing more details about the furniture.

### Key Components

- **ObjectManager**: Manages object selection and switching in AR mode. Generates buttons for users to select items from the wishlist and implements item preview and placement.
- **ObjectPlacer**: Handles placing and moving virtual furniture. Uses raycasting to detect planes and determine furniture placement positions, enabling dragging and rotating of furniture.
- **DoubleTapHandler**: Handles double-tap events to cancel furniture placement. By listening for double-tap events, users can easily cancel unsatisfactory placements.
- **ClickableObject**: Handles long-press events to display a detailed information panel for the furniture. Users can long-press furniture to view detailed information and engage in further interactions.
