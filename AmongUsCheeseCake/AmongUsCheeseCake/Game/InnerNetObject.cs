using System;
using System.Runtime.InteropServices;

namespace AmongUsCheeseCake.Game
{
	[System.Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class InnerNetObject
	{
		public int Instance_Dummy, Instance2_Dummy;
		public UIntPtr m_cachedPtr;
		public uint SpawnId;
		public uint NetId;
		public uint DirtyBits;
		public byte SpawnFlags;
		public byte sendMode; 
		public int OwnerId;
		protected bool DespawnOnDestroy;

	}
}