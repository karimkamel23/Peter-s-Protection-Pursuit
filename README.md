# Peter's Protection Pursuit

A Unity-based cybersecurity awareness game that educates players about online safety and digital protection through engaging gameplay and interactive challenges. The game follows the Model-View-Controller (MVC) pattern with additional Services layer for better code organization and maintainability.

## Server Repository

The backend server repository can be found at: [PPP-Server Repository](https://github.com/karimkamel23/PPP-Server)

## Project Structure

The project follows a well-organized architecture:

### Core Architecture
- **MVC Pattern**: Model-View-Controller separation for better code organization
- **Services Layer**: Additional layer for handling business logic and external communications
- **Scriptable Objects**: Used for configuration and data management

### Directory Structure
- **Assets/**
  - **Scripts/**
    - **Core/**: Core game systems and managers
    - **UI/**: User interface components and screens
    - **Player/**: Player character and related scripts
    - **Enemies/**: Enemy AI and behavior scripts
    - **Rooms/**: Room management and level design
    - **Health/**: Health system and damage handling
    - **Key/**: Key collection and door unlocking mechanics
    - **Door/**: Door mechanics and level progression
    - **Collectibles/**: Collectible items and power-ups
    - **Chest/**: Treasure chest mechanics
  - **Sprites/**
    - **Characters/**: Player and NPC sprites
    - **Icons and Items/**: Game items and UI icons
    - **UI/**: User interface elements
    - **Cutscenes/**: Story and transition scenes
    - **Enviroment/**: Background and level elements
    - **Projectiles/**: Attack and effect sprites
    - **Pixel Adventure 1/**: Asset pack content
  - **Levels/**: Game levels and scenes
  - **Prefabs/**: Reusable game objects
  - **Audio/**: Sound effects and music
  - **Animations/**: Character and object animations
  - **Fonts/**: Text and UI fonts
  - **RestClient/**: API communication utilities
  - **TextMesh Pro/**: Advanced text rendering
  - **Editor/**: Custom editor tools
  - **Presets/**: Unity configuration presets
  - **Adaptive Performance/**: Performance optimization settings

## Features

- Interactive cybersecurity challenges
- Educational content about online safety
- User authentication and progress tracking
- Real-time feedback and scoring system
- Multiple difficulty levels
- Achievement system
- Secure communication with backend server
- Responsive UI design
- Cross-platform compatibility

## Dependencies

- Unity 2022.3 LTS or higher
- Newtonsoft.Json for Unity
- Unity UI Extensions
- TextMeshPro
- Cinemachine
- Input System

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/karimkamel23/Peter-s-Protection-Pursuit.git
   ```

2. Open the project in Unity:
   - Launch Unity Hub
   - Add the project folder
   - Open with Unity 2022.3 LTS or higher

3. Set up the development environment:
   - Install required packages through Unity Package Manager
   - Configure the server connection in the Services settings
   - Set up the development build settings

## Development Setup

1. Ensure you have the PPP-Server running locally or have access to the development server
2. Configure the server connection settings in `Assets/Scripts/Core/NetworkService.cs`
3. Set up your development environment variables
4. Run the game in Unity Editor

## Building the Game

1. Open Build Settings (File > Build Settings)
2. Select target platform
3. Configure build settings
4. Click Build

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## Architecture Details

### MVC + Services Pattern

The game follows a modified MVC pattern with an additional Services layer:

- **Models**: Data structures and game state
  - Player data
  - Game progress
  - Level configurations
  - Achievement tracking

- **Views**: UI and visual elements
  - Menu screens
  - Game HUD
  - Level interfaces
  - Achievement displays

- **Controllers**: Game logic
  - Level management
  - Player input handling
  - Game state management
  - Achievement tracking

- **Services**: External communication
  - Network service for server communication
  - Authentication service
  - Progress synchronization
  - Analytics tracking

## Security Features

- Secure communication with backend
- Input validation
- Data encryption
- Secure storage of user credentials
- Protection against common game exploits

## License

ISC License
