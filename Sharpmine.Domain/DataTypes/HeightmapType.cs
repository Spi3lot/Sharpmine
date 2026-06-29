namespace Sharpmine.Domain.DataTypes;

// https://minecraft.wiki/w/Java_Edition_protocol/Chunk_format#Heightmap_structure
// https://gist.github.com/ByteZ1337/31f10b0052f44acfc177f40a0f0fe9cd
public enum HeightmapType
{

    /// <summary>
    /// Tracks the highest block that is not air (including water/lava).
    /// Used exclusively during initial chunk generation to map the raw topography of the terrain before decorations (like trees) are added.
    /// </summary>
    /// <remarks>
    /// This heightmap is never sent to the client and is discarded from active memory once WorldGen is complete.
    /// </remarks>
    WorldSurfaceWg = 0,

    /// <summary>
    /// Tracks the highest block that is not air (including water/lava, trees, and buildings).
    /// </summary>
    /// <remarks>
    /// Used by the server to determine if a Beacon's vertical beam has clear access to the sky, 
    /// and by the client for general rendering culling.
    /// </remarks>
    WorldSurface = 1,

    /// <summary>
    /// Tracks the highest "Solid" block, actively ignoring all fluids (water/lava) and non-solid blocks (tall grass, flowers).
    /// Used exclusively during initial chunk generation to determine the physical sea-bed/river-bed before water is filled in.
    /// </summary>
    OceanFloorWg = 2,

    /// <summary>
    /// Tracks the highest "Solid" block, actively ignoring all fluids (water/lava).
    /// </summary>
    /// <remarks>
    /// Used by the server for spawning mechanics (e.g., determining where a Drowned can spawn underwater).
    /// </remarks>
    OceanFloor = 3,

    /// <summary>
    /// Tracks the highest "Solid" block OR fluid. Ignores non-solid blocks (tall grass, saplings).
    /// </summary>
    /// <remarks>
    /// Used by both the server and client to calculate exactly where rain and snow particles should collide with the terrain.
    /// </remarks>
    MotionBlocking = 4,

    /// <summary>
    /// Same as <see cref="MotionBlocking"/>, but explicitly ignores Leaves.
    /// </summary>
    /// <remarks>
    /// Used primarily by the server's mob spawning algorithm. By ignoring leaves, the server ensures 
    /// that mobs spawn on the floor *under* the forest canopy, rather than on top of the trees.
    /// </remarks>
    MotionBlockingNoLeaves = 5,

}
