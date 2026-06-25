export interface MeshStats {
  LODCount: number;
  MeshStatData: MeshStatData[];
}

export interface MeshStatData {
  Name: string;
  SurfaceArea: number;
  IndexCount: number;
  VertexCount: number;
  TrisCount: number;
  TrisDensity: number;
  VertexDensity: number;
  Width: number;
  Height: number;
  Depth: number;
  Volume: number;
}
