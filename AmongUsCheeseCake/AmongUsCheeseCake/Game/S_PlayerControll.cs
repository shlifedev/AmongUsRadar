﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AmongUsCheeseCake.Game
{ 
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class S_PlayerControll
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
            var size = Marshal.SizeOf(typeof(S_PlayerControll));;
            return size;
        }
        public static UIntPtr GameOptions;
        public static UIntPtr LocalPlayer;
        public static UIntPtr AllPlayerControls;

        public UIntPtr m_cachedPtr;
        public uint spawnId;
        public uint netId;
        public uint DirtyBits;
        public byte SpawnFlags; 
        public byte sendMode;
        public int OwnerId;
        public bool DespawnOnDestroy;
        public int LastStartCounter;
        public byte PlayerId;
        public float MaxReportDistance;
        public bool moveable;
        public byte inVent;
        public UIntPtr _cachedData;
        public UIntPtr FootSteps; 
        public UIntPtr KillSfx;  
        public UIntPtr KillAnimations;
        public float killTimer;
        public int RemainingEmergencies;
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
        public bool isNew;
        public UIntPtr cache;
        public UIntPtr itemsInRange;
        public UIntPtr newItemsInRange;
        public Byte scannerCount;
        public Boolean infectedSet; 

    }
} 