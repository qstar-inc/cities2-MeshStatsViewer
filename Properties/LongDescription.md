# Mesh Stats Viewer
View information about asset meshes in-game.

Ever wanted to check whether a 3D asset is poorly optimized, missing LODs or just standing there making your game cry?
With this mod, you can do at least two of those things.

- View the number of tris, vertices, volume of any 3D assets.
- In game, the data is viewable when selecting an asset with mesh, on the vanilla Selected Info Panel's Customization tab.
- In editor, the data is viewable when selecting any asset with mesh or selecting a Render Prefab on the Asset Browser, on the vanilla Selected Info Panel's Customization tab.
- Includes option for customizable toleration of LOD tris and volumes so that you can control exactly what should be considered "heavy" for you.
- Multiple views, options to show optional items.
- By default, data view is disabled for vanilla assets but can be enabled from the Options menu.
- Export mesh report for local assets or subscribed assets in three formats (JSON, TXT, CSV). JSON and TXT export creates one file per mod or ImportedData folder. CSV export is only available for subscribed assets and creates one CSV including all current subscriptions.

Originally created at the request of MayorCheeks.

## FAQ
### Why are the tri counts shown in Blender is different from the one shown in the mod/game?
There's two scenarios where the tri count can vary from what it is in the Blender models.
1. Your model has glass/grass/window submeshes. When an asset is imported in-game, all submesh geometry is merged with the main geometry which is what the game/mod reports on.
1. Your model has n-gons which are auto triangulated when exported, thus you don't see the final tris count.