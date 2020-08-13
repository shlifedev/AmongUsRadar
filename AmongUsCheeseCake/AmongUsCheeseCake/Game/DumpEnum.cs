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

public enum ColorID : byte
{
    red = 0,
    blue = 1,
    darkGreen = 2,
    pink = 3,
    orange = 4,
    yellow = 5,
    gray = 6,
    white = 7,
    purple = 8,
    brown = 9,
    cyan = 10,
    green = 11
}