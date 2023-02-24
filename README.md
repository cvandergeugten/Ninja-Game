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

This project features a point and click movement system similar to Diablo and other MOBA games. In order to get the movement system working, I had to setup a couple different components. One of the first steps was to set up a NavMeshAgent in the Movement.cs script. The NavMeshAgent is a Unity component that is used to interact with the environment's Navigation Mesh. For each level, the Navigation Mesh has to be baked based on parameters determined by the designer which stipulate where the player (or any object with the movement script) can move to in the world. All of the decorative objects and props had to be marked with the "static" property in order for the Nav Mesh to be baked around them (so that the player and enemies can't walk through buildings). I also adjusted the parameters for the Nav Mesh to prevent the player from walking up steep inclines. Raycasting is used to allow the player to click on areas of the map and initiate movement to that location. The Movement.cs script utilizes a few important functions to allow for smooth movement behavior. The CanMoveTo() method checks if the destination provided by the player’s raycast input is a valid location on the Nav Mesh for the player to move. The method also calculates the length of the path to the target destination and will return false if the length of the path is too long (to prevent the player from clicking on a part of the map that initiates an incredibly long movement sequence and running through the whole map with one click). The MoveTo() function simply identifies the target destination to move to, adjusts the object’s movement speed, and moves the object to that location. This function is referenced by other scripts (for example in the Fighter.cs scipt when an enemy has to move to the player before initiating a melee attack). The last function worth mentioning is the UpdateAnimator() function which interacts with the Animator Component in Unity to adjust the speed and blend of the character’s movement animation based on the speed at which they are moving (walking for slower movement blends into running as the character speeds up). This function allows for a very smooth look to the way a character walks and runs in the game. An enemy that is patrolling at a walking pace can transition seamlessly into a run animation as they are alerted by the player and go in for an attack. Functionality for movement is also supported by the PlayerController.cs script (for the player) and the AIController.cs script (for enemies). The PlayerController script contains functions for initialzing raycasting when the player clicks down on the mouse and allows for continuous raycasting if the mouse button is held down (so the player doesn’t have to keep clicking constantly while playing). Since enemy movement is not dependent on raycasting, we can just issue the exact destination for enemy characters within the AiController script.


