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
    /// Retrieves a DWORD value for the specified Windows licensing property.
    /// </summary>
    /// <param name="pwszValueName">The name of the property to retrieve.</param>
    /// <param name="pdwValue">A pointer to receive the DWORD value.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [LibraryImport("slc.dll", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int SLGetWindowsInformationDWORD(
        string pwszValueName,
        out uint pdwValue);

    /// <summary>
    /// Retrieves information about the system's current usage of both physical and virtual memory.
    /// </summary>
    /// <param name="lpBuffer">A pointer to a MEMORYSTATUSEX structure.</param>
    /// <returns>True if the function succeeds, false otherwise.</returns>
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    /// <summary>
    /// Firmware table provider signature for SMBIOS ('RSMB').
    /// </summary>
    public const uint RSMB = 0x52534D42; // 'RSMB' in little-endian

    /// <summary>
    /// Retrieves the specified firmware table from the firmware table provider.
    /// </summary>
    /// <param name="FirmwareTableProviderSignature">The firmware table provider signature.</param>
    /// <param name="FirmwareTableID">The firmware table identifier.</param>
    /// <param name="pFirmwareTableBuffer">A pointer to a buffer to receive the firmware table data.</param>
    /// <param name="BufferSize">The size of the buffer.</param>
    /// <returns>The number of bytes written to the buffer, or the required buffer size if the buffer is too small.</returns>
    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial uint GetSystemFirmwareTable(
        uint FirmwareTableProviderSignature,
        uint FirmwareTableID,
        IntPtr pFirmwareTableBuffer,
        uint BufferSize);
}
