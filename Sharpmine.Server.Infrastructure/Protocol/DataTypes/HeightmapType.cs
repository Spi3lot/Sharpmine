namespace Sharpmine.Server.Infrastructure.Protocol.DataTypes;

public enum HeightmapType
{

    // All blocks other than air, cave air and void air.
    // To determine if a beacon beam is obstructed.
    WorldSurface = 1,

    // "Solid" blocks, except bamboo saplings and cactuses; fluids.
    // To determine where to display rain and snow.
    MotionBlocking = 4,

    MotionBlockingNoLeaves = 5, // Same as MOTION_BLOCKING, excluding leaf blocks. 

}
