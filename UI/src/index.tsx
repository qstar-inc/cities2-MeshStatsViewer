import { ModRegistrar } from "cs2/modding";
import { customizeTabSections } from "shared/CustomizeTabSections";
import { SIP_MSV } from "mods/SIPSection";
import mod from "mod.json";

export const SIP_MSV_NAME: string = `${mod.id}.Systems.SIP_MSV`;

const register: ModRegistrar = moduleRegistry => {
  moduleRegistry.extend(
    "game-ui/game/components/selected-info-panel/selected-info-sections/selected-info-sections.tsx",
    "selectedInfoSectionComponents",
    SIP_MSV,
  );
  moduleRegistry.extend(
    "game-ui/game/components/selected-info-panel/selected-info-sections/selected-info-sections.tsx",
    "CUSTOMIZE_TAB_SECTIONS",
    customizeTabSections(SIP_MSV_NAME) as any,
  );
};

export default register;
