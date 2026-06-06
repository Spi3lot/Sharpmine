namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public enum DemoEvent : byte
{

    ShowWelcomeToDemoScreen = 0,

    TellMovementControls = 101,

    TellJumpControl = 102,

    TellInventoryControl = 103,

    TellDemoOverAndPrintScreenshotTutorial = 104,

}
