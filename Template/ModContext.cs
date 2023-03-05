using Reloaded.Mod.Interfaces;

namespace p5rpc.FixShadowSettings.Template;

/// <summary>
/// Represents information passed in from the mod loader template to the implementing mod.
/// </summary>
public class ModContext
{
    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    public IModLoader ModLoader { get; set; } = null!;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    public ILogger Logger { get; set; } = null!;

    /// <summary>
    /// Instance of the IMod interface that created this mod instance.
    /// </summary>
    public IMod Owner { get; set; } = null!;
}