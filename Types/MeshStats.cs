namespace MeshStatsViewer.Types
{
    public class MeshStats
    {
        public int LODCount { get; set; } = 0;
        public MeshStatData[] MeshStatData { get; set; } = new MeshStatData[0];
    }

    public class MeshStatData
    {
        public string Name { get; set; } = string.Empty;
        public float SurfaceArea { get; set; } = 0;

        //public int IndexCount { get; set; } = 0;
        public int VertexCount { get; set; } = 0;
        public int TrisCount { get; set; } = 0;
        public float TrisReduction { get; set; } = 0;
        public float TrisDensity { get; set; } = 0;

        //public float VertexDensity { get; set; } = 0;
        public float Volume { get; set; } = 0;
    }
}
