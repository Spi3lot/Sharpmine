namespace Sharpmine.Domain.DataTypes;

public enum PlayerActionStatus
{

    /// <summary>
    /// Sent when the player starts digging a block.
    /// </summary>
    /// <remarks>
    /// If the block was instamined or the player is in creative mode,
    /// the client will ''not'' send Status = Finished digging,
    /// and will assume the server completed the destruction.
    /// To detect this, it is necessary to calculate the block destruction speed server-side.
    /// </remarks>
    StartedDigging = 0,

    /// <summary>
    /// Sent when the player lets go of the Mine Block key (default: left click).
    /// </summary>
    /// <remarks>
    /// Face is always set to -Y.
    /// </remarks>
    CancelledDigging = 1,

    /// <summary>
    /// Sent when the client thinks it is finished.
    /// </summary>
    FinishedDigging = 2,

    /// <summary>
    /// Triggered by using the Drop Item key (default: Q) with the modifier to drop
    /// the entire selected stack (default: Control or Command, depending on OS).
    /// </summary>
    /// <remarks>
    /// Location is always set to 0/0/0, Face is always set to -Y. Sequence is always set to 0.
    /// </remarks>
    DropItemStack = 3,

    /// <summary>
    /// Triggered by using the Drop Item key (default: Q).
    /// </summary>
    /// <remarks>
    /// Location is always set to 0/0/0, Face is always set to -Y. Sequence is always set to 0.
    /// </remarks>
    DropItem = 4,

    /// <summary>
    /// Indicates that the currently held item should have its state updated,
    /// such as eating food, pulling back bows, using buckets, etc.
    /// </summary>
    /// <remarks>
    /// Location is always set to 0/0/0, Face is always set to -Y. Sequence is always set to 0.
    /// </remarks>
    UpdateItemInHand = 5,

    /// <summary>
    /// Used to swap or assign an item to the second hand.
    /// </summary>
    /// <remarks>
    /// Location is always set to 0/0/0, Face is always set to -Y. Sequence is always set to 0.
    /// </remarks>
    SwapItemInHand = 6,

}
