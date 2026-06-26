export interface MeshStats {
  LODCount: number;
  MeshStatData: MeshStatData[];
}

export interface MeshStatData {
  Name: string;
  TrisCount: number;
  VertexCount: number;
  TrisReduction: number;
  TrisDensity: number;
  SurfaceArea: number;
  Volume: number;
}
