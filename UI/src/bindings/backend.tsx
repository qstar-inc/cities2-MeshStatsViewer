import { bindValue, ValueBinding } from "cs2/api";
import mod from "mod.json";
import { MeshStats } from "types/MeshStats";
import { MeshViewerOptions } from "types/MeshViewerOptions";
import { GetDefault } from "types/OptionsDefaults";

export const prefabName$: ValueBinding<string> = bindValue(
  mod.id,
  "prefabNameBinding",
  "",
);
export const hasMesh$: ValueBinding<boolean> = bindValue(
  mod.id,
  "hasMeshBinding",
  false,
);
export const meshStats$: ValueBinding<MeshStats[]> = bindValue(
  mod.id,
  "meshStatsBinding",
  [],
);
export const meshViewerOptions$: ValueBinding<MeshViewerOptions> = bindValue(
  mod.id,
  "meshViewerOptionsBinding",
  GetDefault,
);
