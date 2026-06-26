export enum View {
  Tabular,
  Detailed,
  Simplified,
}

export interface Options {
  ViewChooser: View;
  LOD1Threshold: number;
  LOD2Threshold: number;
  TrisThreshold: number;
  VolumeThreshold: number;
  CheekyMode: boolean;
}
