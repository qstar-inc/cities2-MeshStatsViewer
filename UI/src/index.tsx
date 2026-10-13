import { ModRegistrar } from "cs2/modding";
import mod from "mod.json";
import { EditorSelectedPanel } from "mods/EditorSelectedPanel";
import { SIPSystem } from "mods/SIPSection";
import { customizeTabSections } from "shared/CustomizeTabSections";
import { EditorWorkspaceAppend } from "shared/EditorWorkspaceAppend";

export const SIPSystem_NAME: string = `${mod.id}.Systems.SIPSystem`;

const register: ModRegistrar = moduleRegistry => {
  moduleRegistry.extend(
    "game-ui/game/components/selected-info-panel/selected-info-sections/selected-info-sections.tsx",
    "selectedInfoSectionComponents",
    SIPSystem,
  );

  moduleRegistry.extend(
    "game-ui/game/components/selected-info-panel/selected-info-sections/selected-info-sections.tsx",
    "CUSTOMIZE_TAB_SECTIONS",
    customizeTabSections(SIPSystem_NAME) as any,
  );

  moduleRegistry.extend(
    "game-ui/editor/components/editor-workspace.tsx",
    "EditorWorkspace",
    EditorWorkspaceAppend(EditorSelectedPanel),
  );
};

export default register;
