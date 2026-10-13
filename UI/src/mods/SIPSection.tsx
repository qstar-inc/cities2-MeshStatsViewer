import { SelectedInfoSectionBase } from "cs2/bindings";
import { FOCUS_AUTO } from "cs2/input";
import * as l10n from "cs2/l10n";
import { PanelFoldout, PanelSectionRow } from "cs2/ui";
import { SIPSystem_NAME } from "index";
import mod from "mod.json";
import { MSVPanel } from "mods/MSVPanel";
import { initTranslationService } from "shared/lang";
import { GetSectionOpen, SetSectionOpen } from "shared/section";
import { MeshStats } from "types/MeshStats";
import { MeshViewerOptions } from "types/MeshViewerOptions";

interface SIPSystem extends SelectedInfoSectionBase {
  prefabName: string;
  hasMesh: boolean;
  meshStatsInfo: MeshStats[];
  meshViewerOptions: MeshViewerOptions;
}

export const SIPSystem = (componentList: any): any => {
  componentList[SIPSystem_NAME] = (props: SIPSystem) => {
    const { translate } = l10n.useLocalization();
    initTranslationService(translate);

    const modNameText = mod.name;
    return (
      <PanelFoldout
        key={`msv-main`}
        initialExpanded={GetSectionOpen("msv-main", true)}
        onToggleExpanded={v => SetSectionOpen("msv-main", v)}
        expandFromContent={true}
        focusKey={FOCUS_AUTO}
        header={
          <PanelSectionRow
            uppercase={true}
            disableFocus={true}
            left={modNameText}
          />
        }
      >
        <MSVPanel
          hasMesh={props.hasMesh}
          meshStatsInfo={props.meshStatsInfo}
          options={props.meshViewerOptions}
          prefabName={props.prefabName}
        />
      </PanelFoldout>
    );
  };
  return componentList as any;
};
