import { MeshViewerOptions } from "types/MeshViewerOptions";
import { View } from "types/View";

export const GetDefault: MeshViewerOptions = {
  EnableVanilla: false,
  ViewChooser: View.Tabular,
  LOD1Threshold: 50,
  LOD2Threshold: 80,
  TrisThreshold: 5000,
  VolumeThreshold: 1,
  ShowWHD: false,
  CheekyMode: true,
};
