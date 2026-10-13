import { View } from "types/View";

export interface MeshViewerOptions {
  EnableVanilla: boolean;
  ViewChooser: View;
  LOD1Threshold: number;
  LOD2Threshold: number;
  TrisThreshold: number;
  VolumeThreshold: number;
  ShowWHD: boolean;
  CheekyMode: boolean;
}
