# Ninja-Game

<p align="center">
  <img width="460" height="300" src="https://github.com/cvandergeugten/Ninja-Game/blob/main/ProjectImages/Ninja_Game_Heading.gif">
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

This project features a point and click movement system similar to Diablo and other MOBA games. In order to get the movement system working, I had to setup a couple different components. One of the first steps was to set up a NavMeshAgent in the Movement.cs script. The NavMeshAgent is a Unity component that is used to interact with the environment's Navigation Mesh. For each level, the Navigation Mesh has to be baked based on parameters determined by the designer which stipulate where the player (or any object with the movement script) can move to in the world. All of the decorative objects and props had to be marked with the "static" property in order for the Nav Mesh to be baked around them (so that the player and enemies can't walk through buildings). I also adjusted the parameters for the Nav Mesh to prevent the player from walking up steep inclines.

Raycasting is used to allow the player to click on areas of the map and initiate movement to that location. The Movement.cs script utilizes a few important functions to allow for smooth movement behavior. The CanMoveTo() method checks if the destination provided by the player’s raycast input is a valid location on the Nav Mesh for the player to move. The method also calculates the length of the path to the target destination and will return false if the length of the path is too long (to prevent the player from clicking on a part of the map that initiates an incredibly long movement sequence and running through the whole map with one click). The MoveTo() function simply identifies the target destination to move to, adjusts the object’s movement speed, and moves the object to that location. This function is referenced by other scripts (for example in the Fighter.cs scipt when an enemy has to move to the player before initiating a melee attack). The last function worth mentioning is the UpdateAnimator() function which interacts with the Animator Component in Unity to adjust the speed and blend of the character’s movement animation based on the speed at which they are moving (walking for slower movement blends into running as the character speeds up). This function allows for a very smooth look to the way a character walks and runs in the game. An enemy that is patrolling at a walking pace can transition seamlessly into a run animation as they are alerted by the player and go in for an attack. Functionality for movement is also supported by the PlayerController.cs script (for the player) and the AIController.cs script (for enemies).

The PlayerController script contains functions for initialzing raycasting when the player clicks down on the mouse and allows for continuous raycasting if the mouse button is held down (so the player doesn’t have to keep clicking constantly while playing). Since enemy movement is not dependent on raycasting, we can just issue the exact destination for enemy characters within the AiController script.

<h2>Combat</h2>

The most crucial component to the game is the combat system. Like Diablo or other MOBA games, this combat system is based on the player clicking the targets that they want to attack (making use of the raycasting we have been using for movement). The way that the combat system is structured also allows for enemy AI to behave in similar ways in combat without needing a raycast input.

The main script responsible for handling the combat is the Fighter.cs script. The Fighter script is dependent on the Core, Movement, Attributes, and Stats namespaces for information regarding player and enemy positions, health, and damage output. This script initializes and keeps track of what weapons are being used through the help of the Weapon.cs and WeaponConfig.cs scripts which will be discussed later. The Fighter script is also responsible for checking if a player/enemy is close enough to the target based on their weapon range to initiate an attack. If the target is out of range, there is a call to the movement script which will move the player/enemy within range and then initiate an attack. Yet another responsibility of the fighter script is to update all of the animations for the characters based on the attacks they are making and the type of weapon they are using. This is accomplished by creating triggers within the Unity animator window and then setting and resetting these triggers in code when certain events happen. The last responsibility of the Fighter script is to delegate damage to the target through the Hit() event which is specific to each animation within the Unity animation tab. This Hit() event happens at the exact moment in the animation where we would expect to see effects and take damage (which is exactly what happens in the code under the Hit() event within the Fighter script). The script interacts with the Health component of the target in order to delegate the proper amount of damage based on what weapon the character is using. When instantiating a blood effect for each weapon hit, I found that the repeated placement for the blood looked a bit inorganic. To improve the presentation for the effect, I implemented some code to randomly change the angle of the blood spray effect within a cone so that repeated hits spray blood in different directions, making combat feel more authentic.

To supplement the functionality of the fighter script, I had to create a combat target script which is a simple way for our system to detect if the object hit by the raycast is an attackable target. Each character in the game has to have the capsule collider component attached in order to be a valid target. This script also ensures that the player cannot target themselves for an attack (Yes, this was happening!).

<h2>Weapons and Projectiles</h2>

The weapon system in this game project relies on Unity’s scriptable object system. I implemented the WeaponConfig.cs script to define the scriptable object and specify the characteristics of each weapon. Functions within this script define what prefab model to use for each weapon type, adjusting the animator override for each weapon, and determining whether or not the weapon is projectile based. I also added a GetRandomHItEffect() function so that I could have a list of blood effects for each weapon to call upon so that the visuals weren’t too repetitive.

The way that this weapon system is set up with scriptable objects makes it very easy for designers to add new weapons to the game. Within the Unity editor, a designer can create a new weapon through the editor menu and add in the details such as prefab model, weapon stats (range, damage, etc.), and the animation to be used for each weapon.

As a supplement to the WeaponConfig script, I have included the Projectile.cs script to handle the projectiles associated with ranged weapons! This script deals with obtain the target’s location, launching the projectile in the direction of the target, and handling collision with the target (or what to do if the projectile has missed the target). This script also gives designers the ability to choose if the projectiles are homing (they follow the target vs. go in a straight line).
The characters (player and enemies) contain an “Default Weapon” component on them which allows designers to easily dictate which weapons each enemy has. I created many different enemies for this game that have a variety of different weapons: katana, kunai, shuriken, and unarmed.

Initially I was building this game with a weapon pickup system for the player, but decided that it would be better for the player to have access to all the weapons in the game at all times. This would give the player creative freedom for how they would like to approach taking down enemies. To achieve this, I used a list to hold all of the weapon scriptable objects and then created a system that cycles through the list based on which hotkey is pressed. This weapon swapping system takes advantage of the EquipWeapon() method which automatically destroys the weapon the player is currently using and replaces it with the target weapon. I made some variants of the weapon prefabs specifically for the player which add visual effects (particle effects and trails) so that the player’s weapons feel more unique than the ones that the enemies are using. To complete the weapon swapping system, I included a simple HUD UI element which indicates which weapon the player currently has selected and what hotkeys are associated with each weapon.

<h2>Stats</h2>

The stats system for this game includes a couple different components that allow for designers to tune the game in a way that fits their vision for the intended playstyle.

Initially I was building this game to have a leveling system similar to common RPG games like World of Warcraft, but had decided later on that I would just use the stats system as a way to easily modify the difficulty of the game for the player. Instead of having enemies provide experience points for being defeated, I modified the system to award “honor” points which contribute to the goal of the game which is to defeat enough enemies to fill up the honor bar in the HUD. The whole system revolves around a progression scriptable object which contains all the details for the characters in the game. With the progression object designers are able to specify the health, damage, honor to victory (player only) , and honor rewarded (enemies only). The progression system utilizes and lookup table that can return the appropriate stat values based on a characters defined class.

There are two enum files that are used for the definition of character classes and each stat. The classes that are currently available to designers are Player, BasicNinja, ShurikenNinja, KunaiNinja, and KatanNinja. The stats that are currently available to designers are Health, HonorReward, HonorToLevelUp, and Damage. More stats can be added and created within the code base and then modified and tuned within the Unity editor using the progression scriptable object.

<h2>Enemy AI</h2>

The AiController.cs script is responsible for handling the behaviors associated with our enemy characters.

The main behavior for our enemies is the ability to attack the player. The script checks if the enemy is within range of the player based on the enemy’s current weapon. If the player is within the attack range, the script will call functions from the Fighter.cs script to initiate combat. The OnDrawGizmosSelected() method is called from the UnityEngine to show designers the attack range of each enemy within the scene view of the project. This made is easy to adjust each enemy’s attack range.

The script also controls the aggro mechanic of the game, which will alert and aggravate enemies nearby an enemy that is attacking the player. Before this mechanic was implemented, you would be able to fight two guards that were standing next to each other separately, since engaging in combat with one wouldn’t aggravate the other. The aggro distance for each enemy is a value that designers can adjust to their liking. With the aggro mechanic in place, the player has to be a little more careful about engaging in combat because if too many enemies are standing near each other they will all be alerted.

Enemies also have a Patrol Behavior that is controlled by the script. Enemies can be given patrol paths created in the Unity Editor which are a list of waypoints that are then cycled through by the enemy. The enemy’s patrolling speed can be adjusted, as well as the number of waypoints in the patrol path. If an enemy is not assigned a patrol path they will stand still in the idle animation until they are aggravated and engage in combat. If the player runs away from enemies that they have aggravated and exceeds a specified distance considered too far, the enemy will return to its patrol path or where it had originally been standing before it was aggravated (aggro reset). 

There are many variables for designers to adjust such as the speed at which an enemy patrols, how long they dwell at each waypoint in a patrol path before they move to the next one, and how far they will chase a player before returning to their post.

<h2>Action Scheduler</h2>

The ActionScheduler.cs script is implemented in this project to help create a nice flow to game’s behaviors. Although the script is simple, it plays a vital role in maintaining smooth transitions between different character states such as moving, fighting, and dancing. This script is coupled with the IAction.cs interface script which allows the functionality of the action scheduler to be implemented in the appropriate scripts.

<h2>Health & Health Pickups</h2>

After developing the basic mechanics for the game, I had to come up with a way for the player to restore the health that they lost from fighting enemies. I initially thought of placing health pickups in pre-determined location on the map, but then decided against this idea as it would make the player’s pathing through each replay to predictable. Instead I decided to give the enemies a random chance of dropping a health pickup so that the player could choose any path in the game and still enjoy the benefits of restoring their health! Each enemy type has a different drop frequency for this health pickup (the more difficult enemies have a much higher chance of dropping a health pickup than easier enemies).

The ItemPickup.cs script provides general functionality for item pickups; in the current state of the project the only pickup that I have is the health pickup. This script handles the behavior of the pickup once the player moves over it (collides with the pickup). The Pickup() method uses the variable healthToRestore to determine if the item is in fact a health pickup. Once the item is determined to be a health pickup, the Heal() method is called from the Attributes namespace in our codebase (in Health.cs) and then the player’s health is adjusted accordingly. 

I added a health bar UI to the player’s HUD so that the player can see how much health they have. The health bar is updated based upon receiving damage or running over a health pickup. Running over multiple health pickups will not heal beyond their maximum health pool.

<h2>Sound Effects & Music</h2>

For the weapon sound effects I utilized the Hit() event attached to each weapon’s animation and implemented the sound.Play() method whenever the Hit() event was triggered. This was super easy to set up in the Unity editor and allowed the game to feature different sound effects for each unique weapon in the game (including unarmed).

I also chose to include some sounds effects for the characters whenever they were taking damage. For this I created a list that I could drop sound clips into and then randomly draw from anytime a character in the game took damage. Have a large list of grunting and pain sounds to draw from gave more of authentic feel when playing the game; otherwise the same repeated sound over and over would drive the player absolutely insane.

I did add some small details sounds to the game as well like a sound for the health pickup and a background soundtrack. The sound effect for the pickup goes off every time the character runs over and pickups up the item (as expected). The soundtrack is made up of a list of a couple licensed songs that are on a random shuffle. The music is a little upbeat and sets a faster tone for the game so when the player feels a bit of a rush while running around and defeating as many enemies as possible.

I initially thought about adding sounds for the footsteps of each character, but I realized that I would have way too many sounds competing with each other and it would sound like an audible mess.



