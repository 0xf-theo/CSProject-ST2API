<p align="center">
  <img height="50%" width="50%" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/ed4cc8b3-bc79-4f4e-b63b-f6905f429b73">
</p>

# Financial Chat App EFREI Paris

- VILLANO Théo
- CANTRELLE Noa

## Installation and Launch Instructions

### Prerequisites

- .NET 8.0 SDK or later
- A code editor such as Visual Studio or Visual Studio Code

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/yourusername/chat-application.git
   cd chat-application
   ```
2. **Navigate to the Solution Directory:**

   ```bash
   cd Project.Chat.Client
   ```
3. **Restore Dependencies:**

   ```bash
   dotnet restore
   ```
### Build and Run

1. **Build the Solution:**

Build the solution using the .NET CLI:

   ```bash
   git clone https://github.com/yourusername/chat-application.git
   cd chat-application
   ```
2. **Run the Server:**

Navigate to the server project directory and run the server application:

   ```bash
  cd Project.Chat.Server
  dotnet run
   ```
3. **Run the Client:**

Open a new terminal window, navigate to the client project directory, and run the client application:

   ```bash
  cd ../Project.Chat
  dotnet run
   ```
3. **Log In or Register:**

- Open the client application.
- Register a new account or log in with existing credentials.
- Start chatting in an existing topic or create a new topic.

## 1. Functions Implemented and Possible Improvements

### Functions Implemented

This project is a chat application that facilitates real-time communication between users. Below are the detailed functionalities of the project:

- **User Authentication:**
  - **Login and Registration:** The application provides a secure system for users to create accounts and log in. Users register by providing a username and password. The passwords are validated using a custom `PasswordChecker` utility, which ensures that they meet specific security requirements. This utility uses the `bcrypt` algorithm to hash and store passwords securely, preventing unauthorized access.
  - **Password Validation:** The `PasswordChecker` class verifies the strength of passwords by enforcing rules such as minimum length, inclusion of special characters, and preventing common passwords. This is done to ensure that user passwords are robust and not easily guessable. When users log in, their input passwords are hashed using `bcrypt` and compared against the stored hashed passwords to verify their identity.

- **Real-Time Messaging:**
  - **Packet-Based Communication:** The application uses a packet-based system for sending and receiving messages. Packets are data structures that encapsulate the information needed for various actions such as sending a message, creating a topic, or registering a user. The main packet classes include:
    - `SendMessagePacket`: Used to encapsulate the details of a message sent by a user.
    - `ReceiveMessagePacket`: Used by the server to send messages to clients.
    - `RegisterAuthPacket`: Handles user registration and authentication details.
    - `TopicHistoryPacket`: Used to retrieve and send the message history of a topic.
  - **Message Handling:** When a user sends a message, a `SendMessagePacket` is created and sent to the server. The server processes this packet, stores the message, and broadcasts the message to all users in the same topic by sending them `ReceiveMessagePacket` instances. This ensures that all users in a topic receive messages in real-time.

- **Topic Management:**
  - **Creating and Managing Topics:** Users can create new chat topics or channels, join existing ones, and view message histories. The user interface for topic creation is managed by the `CreateTopicModal` class. This allows users to specify the name and description of a new topic.
  - **Server-Side Topic Management:** The server handles the creation and management of topics through packet classes such as `ShowTopicPacket` and `TopicsListPacket`. These classes ensure that topics are properly created, stored, and synchronized across all clients.
  - **Viewing Topic History:** The `TopicHistoryPacket` class retrieves the message history for a given topic, allowing users to see past conversations when they join a topic. This is particularly useful for new users joining an ongoing discussion.

- **Data Persistence:**
  - **File-Based Repositories:** The application uses file-based repositories to store user data, messages, and topics persistently. This ensures that data is not lost between server restarts. The repositories are implemented in classes such as:
    - `FileBasedUserRepository`: Manages user data.
    - `FileBasedMessageRepository`: Stores and retrieves chat messages.
    - `FileBasedTopicRepository`: Handles the creation and retrieval of chat topics.
  - **Data Management:** These repositories implement interfaces like `IUserRepository`, `IMessageRepository`, and `ITopicRepository` to standardize data operations. Data is serialized and stored in files, ensuring that it can be reloaded when the server restarts, providing a persistent chat history and user information.

- **Utilities:**
  - **PasswordChecker:** This utility class is responsible for validating passwords during user registration and login. It enforces password strength requirements and uses `bcrypt` to hash passwords securely. This ensures that passwords are stored in a secure manner and are not easily compromised.
  - **Store Class:** The `Store` class is a singleton that maintains the application state. It holds references to user data, topics, and messages, allowing easy access and management of this data throughout the application. The `Store` class ensures that there is a single source of truth for the application's state, making state management more predictable and easier to debug.

### Functions Not Implemented

- **Private Messaging:**
  - The current implementation does not include a private messaging system that allows two users to send direct messages to each other without using a public topic or channel. This feature could be a significant enhancement to support private conversations between users. Implementing private messaging would involve creating new packet types for direct communication and updating the server logic to handle these packets appropriately.

### Possible Improvements

1. **Enhanced Error Handling:**
   - Currently, the error handling is basic and can be improved to cover more edge cases. Implementing a robust error-handling mechanism will make the application more reliable. For example, detailed error messages can be displayed to users, and logs can be maintained for debugging purposes. Exception handling can be standardized across the application to ensure consistent behavior in error scenarios.

2. **Unit Testing:**
   - Introducing comprehensive unit tests for all major functions will ensure code reliability. Automated tests can help catch issues early in the development process and facilitate easier debugging and maintenance. Tests can be written for user authentication, message handling, topic management, and data persistence to ensure that all critical functionalities work as expected.

3. **Code Documentation:**
   - Adding detailed documentation for each class and function will improve code readability and maintainability. This includes inline comments and a comprehensive API reference. Well-documented code is easier for new developers to understand and contribute to, and it also helps in maintaining the codebase over time.

4. **Optimization:**
   - There are opportunities to optimize the code for better performance, particularly in the areas of UI rendering and data processing. Efficient algorithms and data structures can be employed to enhance the application's responsiveness. Profiling tools can be used to identify performance bottlenecks and optimize them.

5. **User Experience Improvements:**
   - Refining the user interface and enhancing the overall user experience based on feedback can make the application more intuitive. This includes improving navigation, reducing clutter, and ensuring consistent design elements. Usability testing can be conducted to gather feedback and make iterative improvements to the UI.

6. **Scalability:**
   - Enhancing the architecture to support scalability will allow the application to handle a larger number of concurrent users efficiently. This could involve transitioning from file-based storage to a database system and optimizing network communication protocols. Load testing can be performed to identify and address scalability issues.

## 2. Design and Implementation

### Design

The project is structured into three main components: the client application, the server application, and a common library. This modular design ensures separation of concerns and facilitates independent development and testing of each component.

- **Client Application:**
  - **Windows Presentation Foundation (WPF):** The client application is built using WPF for the graphical user interface, providing a rich and responsive user experience. WPF allows for a highly customizable and visually appealing UI, enhancing the user experience.
  - **User Interface:** The client application includes screens for user login, registration, and chat functionalities. It manages user interactions and displays real-time updates from the server. The UI components are designed to be intuitive and easy to navigate.
  - **State Management:** The `Store` class is a singleton that holds the application state, including user data, current topics, and messages. This central repository allows easy access and manipulation of data throughout the client application. The `Store` class ensures that there is a single source of truth for the application's state, making state management more predictable and easier to debug.

- **Server Application:**
  - **Connection Management:** The server handles client connections, authenticates users, and processes messages. It ensures that data is synchronized across all connected clients. The server is designed to handle multiple client connections simultaneously and efficiently manage data exchange between them.
  - **Data Repositories:** The server uses file-based repositories to store user information, messages, and topics. These repositories implement interfaces that define standard operations for data management. The file-based approach ensures that data is persistently stored and can be easily accessed and managed.
  - **Packet Processing:** The server processes various types of packets to perform actions such as user registration, message broadcasting, and topic management. This modular approach allows easy extension and modification of server functionalities. The packet processing system ensures that the server can efficiently handle different types of requests and perform the necessary actions.

- **Common Library:**
  - **Packet Structures:** The common library defines packet structures used for communication between the client and server. These packets encapsulate data and instructions for various actions, ensuring consistent data exchange. The packet-based communication system allows for a flexible and extensible messaging protocol.
  - **Shared Code:** The library contains shared code and utilities used by both the client and server applications, ensuring consistency and reducing code duplication. The shared code includes common data structures, utility functions, and constants that are used across the application.

### Implementation

1. **User Authentication:**
   - **Login and Registration:** The login and registration functionalities are implemented in the `LoginRegisterWindow` class and related files. Users can create new accounts or log in to existing accounts. Passwords are checked for complexity using the `PasswordChecker` utility, which leverages `bcrypt` for secure hashing. The authentication system ensures that only authorized users can access the application.
   - **Session Management:** Upon successful login, the application creates a user session, storing relevant user information in the `Store` class. This session management system ensures that user data is available throughout the application.

2. **Real-Time Messaging:**
   - **Packet-Based Communication:** The messaging system uses packet-based communication to send and receive messages. Each message is encapsulated in a `SendMessagePacket` and sent to the server, which then broadcasts it to other users. The server sends `ReceiveMessagePacket` instances to clients to deliver messages. This packet-based approach allows for flexible and efficient communication between clients and the server.

3. **Topic Management:**
   - **Creating and Managing Topics:** The `CreateTopicModal` class provides the user interface for creating new chat topics. Users can join existing topics and view their message histories. The server handles topic management and ensures that all clients are updated with the latest topic information. The topic management system allows users to organize their conversations and participate in different discussion threads.
   - **Viewing Topic History:** The `TopicHistoryPacket` class retrieves and displays the message history for a selected topic, allowing users to catch up on past conversations. This feature ensures that users can access historical data and continue ongoing discussions seamlessly.

4. **Data Persistence:**
   - **File-Based Repositories:** Data persistence is implemented using file-based repositories. The `FileBasedUserRepository`, `FileBasedMessageRepository`, and `FileBasedTopicRepository` classes handle the storage and retrieval of user data, messages, and topics. These classes implement interfaces such as `IUserRepository`, `IMessageRepository`, and `ITopicRepository` to standardize data operations. The file-based repositories ensure that data is stored persistently and can be easily accessed and managed.
   - **Serialization:** Data is serialized and stored in files, ensuring that all user interactions are saved and can be reloaded when the server restarts. The serialization process ensures that data is stored in a structured format, making it easy to retrieve and manage.

5. **Utilities:**
   - **PasswordChecker:** The `PasswordChecker` class is responsible for validating passwords. It enforces password strength requirements and uses `bcrypt` to hash passwords securely. This utility ensures that user passwords are stored securely and are not easily compromised.
   - **Store Class:** The `Store` class is a singleton that maintains the application state. It holds references to user data, topics, messages, and other relevant data, allowing easy access and management of this data throughout the application. This central state management approach ensures consistency across different parts of the application and makes state management more predictable and easier to debug.

This design and implementation approach ensures a clear separation of concerns, making the codebase easier to maintain and extend. Each component can be developed and tested independently, promoting modularity and reusability. This structured design also facilitates future enhancements and scalability improvements.

## 3. Screenshots from the App

<img width="312" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/bd038253-728b-4d12-a0df-b03402d6dc3f">
<img width="312" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/87cd89e0-d180-41e9-8d7f-f2a86563e7e3">
<img width="628" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/9f6ec7a2-6c5a-4c76-a50f-a0ba0c5d3e95">
<img width="628" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/6a907a50-02fc-4933-be96-8b03beeeb091">
<img width="628" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/38fc96cf-8bad-4bd2-958f-577bd5f837bf">
<img width="631" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/5684aefb-2a8e-47a1-891a-8799598b9dd2">
<img width="631" alt="image" src="https://github.com/0xf-theo/CSProject-ST2API/assets/25564401/660f4eb1-ce9a-47b7-a6a6-e08943d152cd">




