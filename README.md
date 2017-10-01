# Mana
This is a framework that provides a lot of boilerplate code for a simple game. The intention is for it to be used for small mobile game projects (like those in a game jam). It currently isn't in a usable state.

<b>Overview</b>
- Utilises Contexts to logically separate gameplay areas. Each context has ContextObjects which may be accessed through a singular interface (they can be thought of as Singletons). Multiple contexts can be used to logically separate areas of gameplay. There is a single GlobalContext to handle the lifecyle of the game and creation of critical systems. Most small games can just use the single GlobalContext. - Each Context handles: initialisation, game data loading, save data saving/loading, updating, shutdown. Initialisation order and execution order is controlled through a Contexts which allows you to avoid errors caused by inconsistent Unity execution. 
- A ContextObject (just a Component) can implement IInitialisationDelegate, IDataLoadDelegate, ISaveDataDelegate, and/or IUpdateDelegate in order to hook into any of the Contexts' methods. 
- Will have a SceneManager that will provide functionality commonly found in games - stacking scenes, stacking pop-ups and transitions. Also auto populate the scene list.
- Will eventually allow you to drop the App prefab into any scene and allow the game to execute from anywhere!

<b>Usage</b>
- Drag App into an empty scene. It is already configured with a GlobalContext.
- The App will create the GlobalContext which will then initialise your app and open your main game scene.

<b>TODO</b>
- Add access to different contexts.
- Decide on convention for loading scenes (perhaps code generation for scene enum).
- Decide on convention for data format and data loading.
- Decide on convention for save data format, saving and loading.
- Add options for loading remote data.
- Add options for connecting to a server.
- Add a better input system than Unity's default touch.
