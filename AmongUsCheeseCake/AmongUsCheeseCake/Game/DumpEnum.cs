using System;

 
[Flags]
public enum SpawnFlags : byte
{
   None = 0,
   IsClientCharacter = 1,
}

public enum SendOption : byte
{
    None = 0, //0x4000039
    Reliable = 1, //0x400003A
}
