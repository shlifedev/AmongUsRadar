using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AmongUsCheeseCake.Game
{ 
    [System.Serializable]
    public class S_PlayerControll
    {
        public UIntPtr m_cachedPtr;
        public uint spawnId;
        public uint netId;
        public uint DirtyBits;
        public uint SpawnFlags; 
        public uint sendMode;
        public uint OwnerId;
        public byte DespawnOnDestroy;
        public Int32 LastStartCounter;
        public byte PlayerId;
        public Single MaxReportDistance;
        public Boolean moveable;
        public Boolean inVent;
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