# Bounce Dash

## Game Overview
**Bounce Dash** is a hyper-casual 2D arcade-style game where the player controls a character that automatically bounces vertically, moving left or right to dodge obstacles (rotating blades, spikes) and collect coins to score points. The level scrolls upward as the player climbs, with the goal of achieving the highest score based on height reached

## Controls
- **Keyboard (PC):**
  - **Move Left**: `A` or Left Arrow key
  - **Move Right**: `D` or Right Arrow key
- **Touch (Mobile - Android/iOS):**
  - **Touch and swipe** on the screen and the character automatically follows the player's X position.
  - A **dead zone** around the character's position prevents floating-point precision issues.
- **Game Actions:**
  - **Jump**: Automatic on landing on platforms tagged "Ground."
  - **Pause**: Click the pause button in the HUD.
  - **Resume**: Click the resume button in the pause menu.
  - **Restart**: Click the restart button in the pause or game over menu.
  - **Exit**: Click the exit button to return to the main menu or quit.

## Known Issues
- **Mobile Input Responsiveness**: Touch controls may feel slightly delayed on low-end devices due to unoptimized `touchMovementSpeed`.
- **Landscape Support**: The UI is not currently setup for landscape orientation.
- **Obstacle Variety**: Limited to rotating blades and spikes; additional obstacle types could enhance gameplay variety.
- **Platform Spawning**: There is a very small chance of two vertical moving platforms spawning too far from each other and the player is unable to make any progress.

## Game Feel and Optimization
The game emphasizes a smooth, responsive feel through:
- **DOTween Animations**: Squash-and-stretch effects on the ball and pulsating UI text enhance visual feedback.
- **Smooth Camera**: `Vector3.SmoothDamp` ensures fluid camera movement, keeping the player in focus.
- **Persistence**: Coin count is saved using `PlayerPrefs`.
- **Singleton Pattern**: `GameManager`, `GameUI`, and `Spawner` use singletons for global state, UI, and spawning management.
- **Optimization**: Objects below the camera's view are destroyed to reduce performance overhead, but object pooling could further improve efficiency.
- **Animations**: DOTween manages platform movement, UI animations, and ball visual effects, with cleanup via `DOKill`.

## Potential Improvements
1. **Further Optimization**:
   - Reduce garbage by pooling the dynamically spawning objects such as platforms, obstacles and pickups.
   - Mobile movement currently works on `Update` loop. It can be moved to event-based logic.
2. **Player Feedback**:
   - Add particle effects for coin collection and death.
   - Include background music and sound effects.
   - Add haptic vibrations on death and purchase.
3. **Shop System**:
   - Implement the store feature in `MainMenuUI.cs` for purchasing skins (color/icon changes) and upgrades.
   - Upgrades data can be stored in scriptable objects for configuration.
   - Use `PlayerPrefs` to save purchased items.

4. **Daily Challenge Mode**:
   - Add a mode with objectives (e.g., "collect 50 coins") using ScriptableObjects for configuration.
   - Challenge progress can be tracked using events such as `OnCoinCollected` or `OnMaxHeightChanged` to keep the logic modular.
5. **Modularity**:
   - The current logic currently relies on singletons and direct references, making the code too tightly coupled. This can be mitigated using inheritance, overloading, interfaces, etc.
6. **Spawner Tuning**:
   - The singular `Spawner` class can be broken down into different classes such as `RotatingBladeSpawner`, `CoinSpawner` etc.
   - The currrent level generation logic is fairly limited. It can be expanded to make the levels more dynamic and prevent soft-lock scenarios.
7. **Visual Upgrades**:
   - Acquire a more complete set of assets to overhaul the aesthetic of the game.
   - Scrolling background with parallax effect to improve sense of vertical movement.
   - Add menu transition animations.
   - Implement high score leaderboards.

## Getting Started
1. **Prerequisites**: Unity 6, Input System, and DOTween packages.
2. **Setup**:
   - Clone the GitHub repository.
   - Open in Unity 6.
   - Add "MainMenu" and "GameScene" to build settings.
3. **Running the Game**:
   - Start in the "MainMenu" scene.
   - Click "Play" to load "GameScene".
   - Use the controls to navigate and collect coins.
