/*
 * This file and other files in the `Template` folder are intended to be left unedited (if possible),
 * to make it easier to upgrade to newer versions of the template.
*/

using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

namespace p5rpc.FixShadowSettings.Template;

public class Startup : IMod
{
    /// <summary>
    /// Used for writing text to the Reloaded log.
    /// </summary>
    private ILogger _logger = null!;

    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private IModLoader _modLoader = null!;

    /// <summary>
    /// Configuration of the current mod.
    /// </summary>
    private IModConfig _modConfig = null!;

    /// <summary>
    /// Encapsulates your mod logic.
    /// </summary>
    private ModBase _mod = new Mod();

    /// <summary>
    /// Entry point for your mod.
    /// </summary>
    public void StartEx(IModLoaderV1 loaderApi, IModConfigV1 modConfig)
    {
        _modLoader = (IModLoader)loaderApi;
        _modConfig = (IModConfig)modConfig;
        _logger = (ILogger)_modLoader.GetLogger();

        // Please put your mod code in the class below,
        // use this class for only interfacing with mod loader.
        _mod = new Mod(new ModContext()
        {
            Logger = _logger,
            ModLoader = _modLoader,
            Owner = this,
        });
    }

    private void OnConfigurationUpdated(IConfigurable obj)
    {

    }

    /* Mod loader actions. */
    public void Suspend() => _mod.Suspend();
    public void Resume() => _mod.Resume();
    public void Unload() => _mod.Unload();

    /*  If CanSuspend == false, suspend and resume button are disabled in Launcher and Suspend()/Resume() will never be called.
        If CanUnload == false, unload button is disabled in Launcher and Unload() will never be called.
    */
    public bool CanUnload() => _mod.CanUnload();
    public bool CanSuspend() => _mod.CanSuspend();

    /* Automatically called by the mod loader when the mod is about to be unloaded. */
    public Action Disposing => () => _mod.Disposing();
}