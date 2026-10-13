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

## License Notes
This project is licensed under GPLv3 (see LICENSE). A quick summary of what that means in practice:

- You're free to use, modify, and redistribute this code.
- Any modified version you distribute must also be licensed under GPLv3 (copyleft). This means you can't use this code in a closed-source project.
- Please retain attribution to the original author when redistributing or forking.

### Forks and Redistribution
This mod is distributed exclusively via Paradox Mods, and I actively maintain it there.

If you'd like to contribute, please consider submitting a PR or reaching out instead of publishing a separate copy or fork.

I kindly ask that you do **not**:
- Upload this mod, or any fork of it, to Nexus Mods or any platform other than Paradox Mods, under any circumstances.
- Publish it as a separate listing on Paradox Mods, unless the original mod is abandoned and I'm unresponsive to contact for an extended period.

If you do publish a fork under those circumstances, please:
- Clearly mark it as a fork/unofficial version (not the original),
- Link back to this repository and credit the original work,
- Follow GPLv3's requirements for source availability and licensing.

This is a request from the maintainer, not an added legal restriction beyond what GPLv3 already requires. It does not modify or limit any rights granted under the GPLv3 license, which remains the sole binding license for this Software.

Reposting an actively maintained mod under a new listing, without need, fragments the community and support for users.