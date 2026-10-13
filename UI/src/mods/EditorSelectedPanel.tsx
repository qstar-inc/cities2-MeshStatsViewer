import {
  hasMesh$,
  meshStats$,
  meshViewerOptions$,
  prefabName$,
} from "bindings/backend";
import { useValue } from "cs2/api";
import * as l10n from "cs2/l10n";
import mod from "mod.json";
import { MSVPanel } from "mods/MSVPanel";
import { EditorWorkspaceBase } from "shared/EditorWorkspaceBase";
import { initTranslationService } from "shared/lang";

export const EditorSelectedPanel = (): JSX.Element => {
  const { translate } = l10n.useLocalization();
  initTranslationService(translate);
  let prefabName = useValue(prefabName$);
  let hasMesh = useValue(hasMesh$);
  let meshStats = useValue(meshStats$);
  let meshViewerOptions = useValue(meshViewerOptions$);

  return (
    <EditorWorkspaceBase
      content={
        <MSVPanel
          hasMesh={hasMesh}
          meshStatsInfo={meshStats}
          options={meshViewerOptions}
          prefabName={prefabName}
        />
      }
      id="msv-editor"
      title={mod.name}
      visible={hasMesh}
    />
  );
};
