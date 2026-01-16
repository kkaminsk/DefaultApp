using System.Runtime.InteropServices;

namespace DefaultApp.Services;

/// <summary>
/// Native method P/Invoke declarations.
/// </summary>
internal static partial class NativeMethods
{
    /// <summary>
    /// SL_GENUINE_STATE enumeration values returned by SLIsGenuineLocal.
    /// </summary>
    public enum SL_GENUINE_STATE : uint
    {
        SL_GEN_STATE_IS_GENUINE = 0,
        SL_GEN_STATE_INVALID_LICENSE = 1,
        SL_GEN_STATE_TAMPERED = 2,
        SL_GEN_STATE_OFFLINE = 3,
        SL_GEN_STATE_LAST = 4
    }

    /// <summary>
    /// MEMORYSTATUSEX structure for GlobalMemoryStatusEx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public static MEMORYSTATUSEX Create()
        {
            return new MEMORYSTATUSEX
            {
                dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>()
            };
        }
    }

    /// <summary>
    /// Windows Software Licensing GUID for Windows OS.
    /// </summary>
    public static readonly Guid WINDOWS_SLID = new("55c92734-d682-4d71-983e-d6ec3f16059f");

    /// <summary>
    /// Determines whether the installation is a genuine installation of Windows.
    /// </summary>
    /// <param name="pAppId">A pointer to a SLID structure that specifies the application ID.</param>
    /// <param name="pGenuineState">A pointer to a variable that receives the genuine state.</param>
    /// <param name="pUIOptions">Reserved. Must be null.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [LibraryImport("slc.dll")]
    public static partial int SLIsGenuineLocal(
        ref Guid pAppId,
        out SL_GENUINE_STATE pGenuineState,
        IntPtr pUIOptions);

    /// <summary>
    /// Retrieves information about the system's current usage of both physical and virtual memory.
    /// </summary>
    /// <param name="lpBuffer">A pointer to a MEMORYSTATUSEX structure.</param>
    /// <returns>True if the function succeeds, false otherwise.</returns>
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
}
