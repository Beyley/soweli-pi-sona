using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace sowelipisona.Native; 

public class Linux {
	/// <summary>
	///     A list of flags available to load a library with
	/// </summary>
	[Flags]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public enum LoadFlags {
		RTLD_LAZY         = 0x00001,
		RTLD_NOW          = 0x00002,
		RTLD_BINDING_MASK = 0x00003,
		RTLD_NOLOAD       = 0x00004,
		RTLD_DEEPBIND     = 0x00008,
		RTLD_GLOBAL       = 0x00100,
		RTLD_LOCAL        = 0x00000,
		RTLD_NODELETE     = 0x01000
	}

	[DllImport("libdl.so", EntryPoint = "dlopen", CharSet = CharSet.Unicode)]
	private static extern IntPtr dlopen(string library, LoadFlags flags);

	/// <summary>
	///     Loads a native Linux library
	/// </summary>
	/// <param name="filename">The filename of the library to load</param>
	/// <param name="flags">The flags of the load</param>
	public static void Load(string filename, LoadFlags flags) {
		dlopen(filename, flags);
	}
}
