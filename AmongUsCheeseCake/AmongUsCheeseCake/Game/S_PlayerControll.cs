using AmongUsCheeseCake.Cheat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AmongUsCheeseCake.Game
{

   


    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct S_PlayerControll
    {
 

        public static S_PlayerControll FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned); 
            var data = (S_PlayerControll)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(S_PlayerControll));
            gcHandle.Free();
            return data;
        }

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(S_PlayerControll)); 
            Console.WriteLine(size);
            return size;
        } 
  
        public Vector2 GetSyncPosition()
        {
            try
            { 
                int _offset_vec2_position = 60;
                int _offset_vec2_sizeOf = 8;
                var netTransform = ((int)NetTransform + _offset_vec2_position).ToString("X");  
                var vec2Data= CheatBase.Memory.ReadBytes($"{netTransform}",_offset_vec2_sizeOf); // 주소로부터 8바이트 읽는다   
                if (vec2Data != null && vec2Data.Length != 0 )
                { 
                    var vec2 = Vector2.FromBytes(vec2Data); 
                    return vec2;
                }
                else
                { 
                    return Vector2.Zero;
                }
            }


            catch(Exception e)
            { 
                Console.WriteLine(e);
                return Vector2.Zero;
            }
        }
        public Vector2 GetMyPosition()
        {
            try
            {
                int _offset_vec2_position = 80;
                int _offset_vec2_sizeOf = 8;
                var netTransform = ((int)NetTransform + _offset_vec2_position).ToString("X");
                var vec2Data= CheatBase.Memory.ReadBytes($"{netTransform}",_offset_vec2_sizeOf); // 주소로부터 8바이트 읽는다  
                if (vec2Data != null && vec2Data.Length != 0)
                {
                    var vec2 = Vector2.FromBytes(vec2Data); 
                    return vec2;
                }
                else
                { 
                    return Vector2.Zero;
                } 
            } 
            catch
            {
                return Vector2.Zero;
            }
        }


        public UIntPtr m_cachedPtr;
        public uint spawnId;
        public uint netId;
        public uint DirtyBits;
        public byte SpawnFlags;
        public bool sendMode;
        public uint OwnerId;
        public byte DespawnOnDestroy;
        public Int32 LastStartCounter;
        public byte PlayerId;
        public Single MaxReportDistance;
        public bool moveable;
        public byte inVent;
        public UIntPtr _cachedData;
        public UIntPtr FootSteps;
        public UIntPtr KillSfx;
        public UIntPtr KillAnimations;
        public Single killTimer;
        public Int32 RemainingEmergencies;
        public UIntPtr nameText;
        public UIntPtr LightPrefab;
        public UIntPtr myLight;
        public UIntPtr Collider;
        public UIntPtr MyPhysics;
        public UIntPtr NetTransform;
        public UIntPtr CurrentPet;
        public UIntPtr HatRenderer;
        public UIntPtr myRend;
        public UIntPtr hitBuffer;
        public UIntPtr myTasks;
        public UIntPtr ScannerAnims;
        public UIntPtr ScannersImages;
        public UIntPtr closest;
        public Boolean isNew;
        public UIntPtr cache;
        public UIntPtr itemsInRange;
        public UIntPtr newItemsInRange;
        public Byte scannerCount;
        public Boolean infectedSet;

    }
}