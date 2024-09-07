using Reloaded.Mod.Interfaces;
using p5rpc.FixShadowSettings.Template;
using Reloaded.Memory.Sigscan;
using System.Diagnostics;
using Reloaded.Memory.Sources;

namespace p5rpc.FixShadowSettings;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{
    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private readonly IModLoader _modLoader;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Entry point into the mod, instance that created this class.
    /// </summary>
    private readonly IMod _owner;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _logger = context.Logger;
        _owner = context.Owner;

        // For more information about this template, please see
        // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

        // If you want to implement e.g. unload support in your mod,
        // and some other neat features, override the methods in ModBase.

        var mainModule = Process.GetCurrentProcess().MainModule;
        if( mainModule == null)
        {
            throw new Exception("[FixShadowSetting]Main Module is Null!");
        }
        var baseAddress = mainModule.BaseAddress;
        var exeSize = mainModule.ModuleMemorySize;
        unsafe
        {
            using var scanner = new Scanner((byte*)baseAddress, exeSize);
            
            // Pattern for Steam 1.04
            var result = scanner.FindPattern("89 05 ?? ?? ?? ?? 80 BB ?? ?? ?? ?? 00 74 05 83 C9 20");
            if (!result.Found)
            {
                _logger.WriteLine("[FixShadowSetting]Pattern not found for 1.04");

                // Pattern for Steam 1.03B
                result = scanner.FindPattern("89 05 ?? ?? ?? ?? 41 80 BA ?? ?? ?? ?? 00");
                if (!result.Found)
                {
                    throw new Exception("[FixShadowSetting]Pattern not found");
                }
            }

            _logger.WriteLine($"[FixShadowSetting] instruction offset {result.Offset:X}");

            // Nop the instruction that sets the medium shadow quality on every load screen...
            var ínstructionAddress = baseAddress + result.Offset;
            var instructionSize = 6;
            NopInstruction(ínstructionAddress, instructionSize);
            _logger.WriteLine($"[FixShadowSetting] Init ok");
        }
    }

    private void NopInstruction(nint ínstructionAddress, int instructionSize)
    {
        byte nopOpcodeX64 = 0x90;
        for (var i = 0; i < instructionSize; i++)
        {
            Memory.Instance.SafeWrite(ínstructionAddress + i, nopOpcodeX64);
        }
    }

    #region Standard Overrides
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}