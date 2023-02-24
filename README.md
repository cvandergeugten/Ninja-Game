# Ninja-Game

<p align="center">
  <img width="460" height="300" src="https://github.com/cvandergeugten/Ninja-Game/blob/main/ProjectImages/Ninja%20Game%20Preview.png">
</p>

<h1>Summary</h1>

After completing a Udemy course (RPG Core Combat Creator: Learn Intermediate Unity C# Coding) I decided to develop my own version of the game that was created in the course. This project includes programming/editor techniques that I learned from the course as well as assets purchased from the Unity Asset Store (https://assetstore.unity.com/). In this section I will briefly summarize the things that I learned from working on this project and the assets that I used. More detailed descriptions of the project can be found below the summary section.

<br>

I learned how to:
- Import Unity Assets from the Asset Store and configure them for my project
- Structure project folders and hierarchy in a clean and organized manner
- Implement various game design patterns (Observer, Singleton, State, etc.)
- Set up character animations for movement, fighting, and dancing
- Structure a code base and manage dependencies using C# namespaces
- Implement an action priority system
- Use scriptable objects to make the creation of in game items easy for designers (weapon system)
- Create and decorate level terrain from scratch
- Implement raycasting for movement and combat
- Configure a navmesh for player and enemy movement
- Program enemy AI behaviors such as attacking, patrolling, group aggro, etc.
- Develop a stats system that is configurable and easy to use for game designers (changes made in editor, not code!)
- Create different enemy types that each have unique weapons
- Create various UI components for player HUD (health bar, honor bar, weapon selection)
- Implement instantiation of projectiles for throwing weapons like the shuriken
- Create particle effects and trails for weapons/pickups
- Create pickups that restore the player's health
- Implement random drop chance for items after killing an enemy (health pickups)
- Implement random direction for weapon hit effects (blood spray)
- Implement background music playlist with random shuffle
- Implement a follow camera for the player using Cinemachine
- Include a skybox and adjust directional lighting to create desired level lighting
- Implement various cursors depending on what the player's mouse is hovering over
- Bind actions to certain keys (switching weapons/ dancing)
- Use Unity's profiler to diagnose performance issues
- Implement different C# features such as structs, lists, dictionaries, arrays, etc.

<br>

The following assets were obtained from the Unity Asset Store and used for this project:
- Books, Scrolls, and Other Things (Vergis)
- C.V.P - Japanese Garden (Alexander Elert)
- Dance MoCap 03 (Morro Motion)
- Death Animations (Polygonmaker)
- Fantasy Skybox FREE (Render Knight)
- Flat Shaded - Japan (Halberstram Art)
- Grass Flowers Pack Free (ALP8310)
- Human Vocalizations (Gamemaster Audio)
- LoFi Trip 2 Music (WAVmaniacs)
- Male Ninja Modular Pack Cute Series (Meshtint Studio)
- Pixel Cursors (ClayManStudio)
- Punch and Fighting Sounds (Cafofo)
- RPG Character Mecanim Animatiojn Pack FREE (Explosive)
- RPG Essentials Sound Effects - FREE! (leohpaz)
- Stylized RPG Cursors (Lid Games)
- Terrain Sample Asset Pack (ALP8310)
- Throwing Animations (Kevin Iglesias)
- Volumetric Blood Fluids (kripto289)
- Water Shaders V2.x (Nick Veselov)
- Wood & Tree Materials (Alessio Regalbuto)

<h1>Project Details</h1>

In this section I will go over each area of the project that I worked on and describe what I learned, how I implemented certain coding techniques, and things I might have done differently.

<h2>Movement</h2>

