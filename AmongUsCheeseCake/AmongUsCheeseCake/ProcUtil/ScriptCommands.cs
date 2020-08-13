using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region Definitions (from scripts.dll)
/*
typedef void (*_Debug_Message) (const char *Message,...);
typedef void (*_Action_Reset) (GameObject *Obj,float MaxResetPriority);
typedef void (*_Action_Goto) (GameObject *Obj,const void *Params);
typedef void (*_Action_Attack) (GameObject *Obj,const void *Params);
typedef void (*_Action_Play_Animation) (GameObject *Obj,const void *Params);
typedef void (*_Action_Enter_Exit) (GameObject *Obj,const void *Params);
typedef void (*_Action_Face_Location) (GameObject *Obj,const void *Params);
typedef void (*_Action_Dock) (GameObject *Obj,const void *Params);
typedef void (*_Action_Follow_Input) (GameObject *Obj,const void *Params);
typedef void (*_Modify_Action) (GameObject *Obj,int ActionID,const void *Params,bool ModifyUnk1,bool ModifyUnk2);
typedef int (*_Get_Action_ID) (GameObject *Obj);
typedef void (*_Get_Action_Params) (GameObject *Obj,void *Params);
typedef bool (*_Is_Performing_Pathfind_Action) (GameObject *Obj);
typedef void (*_Set_Position) (GameObject *Obj,const Vector3 &Position);
typedef Vector3 (*_Get_Position) (GameObject *Obj);
typedef Vector3 (*_Get_Bone_Position) (GameObject *Obj,const char *Bone);
typedef float (*_Get_Facing) (GameObject *Obj);
typedef void (*_Set_Facing) (GameObject *Obj,float Facing);
typedef void (*_Disable_All_Collisions) (GameObject *Obj);
typedef void (*_Disable_Physical_Collisions) (GameObject *Obj);
typedef void (*_Enable_Collisions) (GameObject *Obj);
typedef void (*_Destroy_Object) (GameObject *Obj);
typedef GameObject *(*_Find_Object) (int ObjectID);
typedef GameObject *(*_Create_Object) (const char *Preset,const Vector3 &Position);
typedef GameObject *(*_Create_Object_At_Bone) (GameObject *Obj,const char *Preset,const char *Bone);
typedef int (*_Get_ID) (GameObject *Obj);
typedef int (*_Get_Preset_ID) (GameObject *Obj);
typedef const char *(*_Get_Preset_Name) (GameObject *Obj);
typedef void (*_Attach_Script) (GameObject *Obj,const char *Script,const char *Parameters);
typedef void (*_Add_To_Dirty_Cull_List) (GameObject *Obj);
typedef void (*_Start_Timer) ();
typedef void (*_Trigger_Weapon) (GameObject *Obj,bool TriggerWeapon,const Vector3 &Location,bool Primary);
typedef void (*_Select_Weapon) (GameObject *Obj,const char *Weapon);
typedef void (*_Send_Custom_Event) (GameObject *Sender,GameObject *Reciever,int Message,int Param,float Delay);
typedef void (*_Send_Damaged_Event) (GameObject *Obj,GameObject *Damager);
typedef float (*_Get_Random) (float Min,float Max);
typedef int (*_Get_Random_Int) (int Min,int Max);
typedef void (*_Find_Random_Simple_Object) (const char *Preset);
typedef void (*_Set_Model) (GameObject *Obj,const char *Model);
typedef void (*_Set_Animation) (GameObject *Obj,const char *Animation,bool Loop,const char *SubObject,float FirstFrame,float LastFrame,bool Blended);
typedef void (*_Set_Animation_Frame) (GameObject *Obj,const char *Animation,int Frame);
typedef int (*_Create_Sound) (const char *Sound,const Vector3 &Position,GameObject *Obj);
typedef int (*_Create_2D_Sound) (const char *Sound);
typedef int (*_Create_2D_WAV_Sound) (const char *Sound);
typedef int (*_Create_3D_WAV_Sound_At_Bone) (const char *Sound,GameObject *Obj,const char *Bone);
typedef int (*_Create_3D_Sound_At_Bone) (const char *Sound,GameObject *Obj,const char *Bone);
typedef int (*_Create_Logical_Sound) (GameObject *Obj,int SoundTypeMask,const Vector3 &Position,float DropoffRadius);
typedef void (*_Start_Sound) (int SoundID);
typedef void (*_Stop_Sound) (int SoundID,bool RemoveFromScene);
typedef void (*_Monitor_Sound) (GameObject *Obj,int SoundID);
typedef void (*_Set_Background_Music) (const char *Music);
typedef void (*_Fade_Background_Music) (const char *Music,int FadeOut,int FadeIn);
typedef void (*_Stop_Background_Music) ();
typedef float (*_Get_Health) (GameObject *Obj);
typedef float (*_Get_Max_Health) (GameObject *Obj);
typedef void (*_Set_Health) (GameObject *Obj,float Health);
typedef float (*_Get_Shield_Strength) (GameObject *Obj);
typedef float (*_Get_Max_Shield_Strength) (GameObject *Obj);
typedef void (*_Set_Shield_Strength) (GameObject *Obj,float Armor);
typedef void (*_Set_Shield_Type) (GameObject *Obj,const char *ArmourType);
typedef int (*_Get_Player_Type) (GameObject *Obj);
typedef void (*_Set_Player_Type) (GameObject *Obj,int PlayerType);
typedef float (*_Get_Distance) (const Vector3 &Pos1,const Vector3 &Pos2);
typedef void (*_Set_Camera_Host) (GameObject *Obj);
typedef void (*_Force_Camera_Look) (const Vector3 &Look);
typedef GameObject *(*_Get_The_Star) ();
typedef GameObject *(*_Get_A_Star) (const Vector3 &Pos);
typedef GameObject *(*_Find_Closest_Soldier) (const Vector3 &Pos,float StartDistance,float EndDistance,bool HumanControled);
typedef bool (*_Is_A_Star) (GameObject *Obj);
typedef void (*_Control_Enable) (GameObject *Obj,bool Enable);
typedef const char *(*_Get_Damage_Bone_Name) ();
typedef float (*_Get_Damage_Bone_Direction) ();
typedef bool (*_Is_Object_Visible) (GameObject *Obj,GameObject *Visible);
typedef void (*_Enable_Enemy_Seen) (GameObject *Obj,bool Enable);
typedef void (*_Set_Display_Color) (unsigned char Red,unsigned char Green,unsigned char Blue);
typedef void (*_Display_Text) (int StringID);
typedef void (*_Display_Float) (float Number,const char *Message);
typedef void (*_Display_Int) (int Number,const char *Message);
typedef void (*_Save_Data) ();
typedef void (*_Save_Pointer) ();
typedef char (*_Load_Begin) ();
typedef void (*_Load_Data) ();
typedef void (*_Load_Pointer) ();
typedef void (*_Load_End) ();
typedef void (*_Begin_Chunk) ();
typedef void (*_End_Chunk) ();
typedef char (*_Open_Chunk) ();
typedef void (*_Close_Chunk) ();
typedef void (*_Clear_Radar_Markers) ();
typedef void (*_Clear_Radar_Marker) (int Marker);
typedef void (*_Add_Radar_Marker) (int Marker,const Vector3 &Pos,int Shape,int Color);
typedef void (*_Set_Obj_Radar_Blip_Shape) (GameObject *Obj,int Shape);
typedef void (*_Set_Obj_Radar_Blip_Color) (GameObject *Obj,int Color);
typedef void (*_Enable_Radar) (bool Enable);
typedef void (*_Clear_Map_Cell) (int X,int Y);
typedef void (*_Clear_Map_Cell_By_Pos) (const Vector3 &Pos);
typedef void (*_Clear_Map_Cell_By_Pixel_Pos) (int X,int Y);
typedef void (*_Clear_Map_Region_By_Pos) (const Vector3 &Pos,int Reigon);
typedef void (*_Reveal_Map) ();
typedef void (*_Shroud_Map) ();
typedef void (*_Show_Player_Map_Marker) (bool Show);
typedef float (*_Get_Safe_Flight_Height) (float X,float Y);
typedef void (*_Create_Explosion) (const char *Explosion,const Vector3 &Pos,GameObject *Damager);
typedef void (*_Create_Explosion_At_Bone) (const char *Explosion,GameObject *Obj,const char *Bone,GameObject *Damager);
typedef void (*_Enable_HUD) (bool Enable);
typedef void (*_Mission_Complete) (bool Success);
typedef void (*_Give_Powerup) (GameObject *Obj,const char *Powerup,bool ShowOnHud);
typedef void (*_Innate_Disable) (GameObject *Obj);
typedef void (*_Innate_Enable) (GameObject *Obj);
typedef void (*_Innate_Soldier_Enable_Enemy_Seen) (GameObject *Obj,bool Enable);
typedef void (*_Innate_Soldier_Enable_Gunshot_Heard) (GameObject *Obj,bool Enable);
typedef void (*_Innate_Soldier_Enable_Footsteps_Heard) (GameObject *Obj,bool Enable);
typedef void (*_Innate_Soldier_Enable_Bullet_Heard) (GameObject *Obj,bool Enable);
typedef void (*_Innate_Soldier_Enable_Actions) (GameObject *Obj,bool Enable);
typedef void (*_Set_Innate_Soldier_Home_Location) (GameObject *Obj,const Vector3 &Pos,float Facing);
typedef void (*_Set_Innate_Aggressiveness) (GameObject *Obj,float Aggressiveness);
typedef void (*_Set_Innate_Take_Cover_Probability) (GameObject *Obj,float Probobility);
typedef void (*_Set_Innate_Is_Stationary) (GameObject *Obj,bool IsStationary);
typedef void (*_Innate_Force_State_Bullet_Heard) (GameObject *Obj,const Vector3 &Pos);
typedef void (*_Innate_Force_State_Footsteps_Heard) (GameObject *Obj,const Vector3 &Pos);
typedef void (*_Innate_Force_State_Gunshots_Heard) (GameObject *Obj,const Vector3 &Pos);
typedef void (*_Innate_Force_State_Enemy_Seen) (GameObject *Obj,GameObject *Enemy);
typedef void (*_Static_Anim_Phys_Goto_Frame) (int ObjectID,float Frame,const char *Anim);
typedef void (*_Static_Anim_Phys_Goto_Last_Frame) (int ObjectID,const char *Anim);
typedef int (*_Get_Sync_Time) ();
typedef void (*_Add_Objective) (int ObjectiveID,int Type,int ObjectiveUnk,int LongDescID,const char *Sound,int ShortDescID);
typedef void (*_Remove_Objective) (int ObjectiveID);
typedef void (*_Set_Objective_Status) (int ObjectiveID,int Status);
typedef void (*_Change_Objective_Type) (int ObjectiveID,int Type);
typedef void (*_Set_Objective_Radar_Blip) (int ObjectiveID,const Vector3 &Pos);
typedef void (*_Set_Objective_Radar_Blip_Object) (int ObjectiveID,GameObject *Obj);
typedef void (*_Set_Objective_HUD_Info) (int ObjectiveID,float Priority,const char *PogTexture,int PogTitleId);
typedef void (*_Set_Objective_HUD_Info_Position) (int ObjectiveID,float Priority,const char *PogTexture,int PogTitleId,const Vector3 &Pos);
typedef void (*_Shake_Camera) (const Vector3 &Pos,float Radius,float Intensity,float Time);
typedef void (*_Enable_Spawner) (int SpawnerID,bool Enable);
typedef void (*_Trigger_Spawner) (int SpawnerID);
typedef void (*_Enable_Engine) (GameObject *Obj,bool Enable);
typedef int (*_Get_Difficulty_Level) ();
typedef void (*_Grant_Key) (GameObject *Obj,int Key,bool Grant);
typedef bool (*_Has_Key) (GameObject *obj,int Key);
typedef void (*_Enable_Hibernation) (GameObject *Obj,bool Enable);
typedef void (*_Attach_To_Object_Bone) (GameObject *AttachObj,GameObject *BoneObj,const char *Bone);
typedef int (*_Create_Conversation) (const char *Conversation,int Priority,float MaxDist,bool IsInterruptable);
typedef void (*_Join_Conversation) (GameObject *Obj,int ConversationID,bool OratorFlag1,bool OratorFlag2,bool OratorFlag3);
typedef void (*_Join_Conversation_Facing) (GameObject *Obj,int ConversationID,int Facing);
typedef void (*_Start_Conversation) (int ConversationID,int ActionID);
typedef void (*_Monitor_Conversation) (GameObject *Obj,int ConversationID);
typedef void (*_Start_Random_Conversation) (GameObject *Obj);
typedef void (*_Stop_Conversation) (int ConversationID);
typedef void (*_Stop_All_Conversations) ();
typedef void (*_Lock_Soldier_Facing) (GameObject *Obj,GameObject *LockObj,bool Lock);
typedef void (*_Unlock_Soldier_Facing) (GameObject *Obj);
typedef void (*_Apply_Damage) (GameObject *Obj,float Damage,const char *Warhead,GameObject *Damager);
typedef void (*_Set_Loiters_Allowed) (GameObject *Obj,bool Allowed);
typedef void (*_Set_Is_Visible) (GameObject *Obj,bool Visible);
typedef void (*_Set_Is_Rendered) (GameObject *Obj,bool Rendered);
typedef float (*_Get_Points) (GameObject *Obj);
typedef void (*_Give_Points) (GameObject *Obj,float Points,bool EntireTeam);
typedef float (*_Get_Money) (GameObject *Obj);
typedef void (*_Give_Money) (GameObject *Obj,float Money,bool EntireTeam);
typedef bool (*_Get_Building_Power) (GameObject *Obj);
typedef void (*_Set_Building_Power) (GameObject *Obj,bool Powered);
typedef void (*_Play_Building_Announcement) (GameObject *Obj,int AnnouncementID);
typedef GameObject *(*_Find_Nearest_Building) (GameObject *Obj,const char *Building);
typedef GameObject *(*_Find_Nearest_Building_To_Pos) (const Vector3 &Pos,const char *Building);
typedef int (*_Team_Members_In_Zone) (GameObject *Zone,int Team);
typedef void (*_Set_Clouds) (float Cover,float Gloominess,float Transition);
typedef void (*_Set_Lightning) (float Intensity,float StartDistance,float EndDistance,float Heading,float Distribution,float Transition);
typedef void (*_Set_War_Blitz) (float Intensity,float StartDistance,float EndDistance,float Heading,float Distribution,float Transition);
typedef void (*_Set_Wind) (float Heading,float Speed,float Variability,float Transition);
typedef void (*_Set_Rain) (float Density,float Transition,bool Unused);
typedef void (*_Set_Snow) (float Density,float Transition,bool Unused);
typedef void (*_Set_Ash) (float Density,float Transition,bool Unused);
typedef void (*_Set_Fog_Enable) (bool Enable);
typedef void (*_Set_Fog_Range) (float StartDistance,float EndDistance,float Transition);
typedef void (*_Enable_Stealth) (GameObject *Obj,bool Stealth);
typedef void (*_Cinematic_Sniper_Control) (bool Enable,float Zoom);
typedef int (*_Text_File_Open) (const char *Filename);
typedef bool (*_Text_File_Get_String) (int Handle,char *Data,int Size);
typedef void (*_Text_File_Close) (int Handle);
typedef void (*_Enable_Vehicle_Transitions) (GameObject *Obj,bool Enable);
typedef void (*_Display_GDI_Player_Terminal) ();
typedef void (*_Display_NOD_Player_Terminal) ();
typedef void (*_Display_Mutant_Player_Terminal) ();
typedef bool (*_Reveal_Encyclopedia_Character) (int EncyclopediaID);
typedef bool (*_Reveal_Encyclopedia_Weapon) (int EncyclopediaID);
typedef bool (*_Reveal_Encyclopedia_Vehicle) (int EncyclopediaID);
typedef bool (*_Reveal_Encyclopedia_Building) (int EncyclopediaID);
typedef void (*_Display_Encyclopedia_Event_UI) ();
typedef void (*_Scale_AI_Awareness) (float GlobalSightRangeScale,float Unused);
typedef void (*_Enable_Cinematic_Freeze) (GameObject *Obj,bool Freeze);
typedef void (*_Expire_Powerup) (GameObject *Obj);
typedef void (*_Set_HUD_Help_Text) (int StringID,const Vector3 &Color);
typedef void (*_Enable_HUD_Pokable_Indicator) (GameObject *Obj,bool IsPokeable);
typedef void (*_Enable_Innate_Conversations) (GameObject *Obj,bool Enable);
typedef void (*_Display_Health_Bar) (GameObject *Obj,bool Display);
typedef void (*_Enable_Shadow) (GameObject *Obj,bool Enable);
typedef void (*_Clear_Weapons) (GameObject *Obj);
typedef void (*_Set_Num_Tertiary_Objectives) (int Num);
typedef void (*_Enable_Letterbox) (bool Enable,float Transition);
typedef void (*_Set_Screen_Fade_Color) (float Red,float Green,float Blue,float Transition);
typedef void (*_Set_Screen_Fade_Opacity) (float Opacity,float Transition);

//note: "tested" after a script command means that someone has actually used it in a script and we have verified that it does what we think it does and that we know how to use it.
//also "doesnt work in MP" means that it will not work for machines other than the host in standard renegade. However,some of them can be made to work in various ways via bhs.dll
struct ScriptCommands {
0   unsigned int version1;
1unsigned int version2;
2_Debug_Message Debug_Message; //doesnt work in the public builds of renegade,one can always use your own calls to fopen and fwrite and etc
3_Action_Reset Action_Reset; //tested
4_Action_Goto Action_Goto; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
5_Action_Attack Action_Attack; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
	_Action_Play_Animation Action_Play_Animation; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do),may not work in MP
	_Action_Enter_Exit Action_Enter_Exit;
	_Action_Face_Location Action_Face_Location; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
	_Action_Dock Action_Dock; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
10_Action_Follow_Input Action_Follow_Input; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
	_Modify_Action Modify_Action; //partially tested (command works but there is a lot more testing to figure out all the fields in the ActionParamsStruct to do)
	_Get_Action_ID Get_Action_ID;
	_Get_Action_Params Get_Action_Params;
	_Is_Performing_Pathfind_Action Is_Performing_Pathfind_Action;
15	_Set_Position Set_Position; //tested
	_Get_Position Get_Position; //tested
	_Get_Bone_Position Get_Bone_Position; //tested
	_Get_Facing Get_Facing; //tested
	_Set_Facing Set_Facing; //tested
20	_Disable_All_Collisions Disable_All_Collisions; //tested
	_Disable_Physical_Collisions Disable_Physical_Collisions; //tested
	_Enable_Collisions Enable_Collisions; //tested
	_Destroy_Object Destroy_Object; //tested
	_Find_Object Find_Object; //tested
25	_Create_Object Create_Object; //tested. Dont use on objects that arent PhysicalGameObjs
	_Create_Object_At_Bone Create_Object_At_Bone; //tested
	_Get_ID Get_ID; //tested
	_Get_Preset_ID Get_Preset_ID; //tested
	_Get_Preset_Name Get_Preset_Name; //tested
30	_Attach_Script Attach_Script; //tested
	_Add_To_Dirty_Cull_List Add_To_Dirty_Cull_List; //tested
	_Start_Timer Start_Timer; //tested
	_Trigger_Weapon Trigger_Weapon;
	_Select_Weapon Select_Weapon; //tested, doesnt work in MP without bhs.dll
35	_Send_Custom_Event Send_Custom_Event; //tested
	_Send_Damaged_Event Send_Damaged_Event;
	_Get_Random Get_Random; //tested
	_Get_Random_Int Get_Random_Int; //tested
	_Find_Random_Simple_Object Find_Random_Simple_Object;
40	_Set_Model Set_Model; //tested, doesnt work in MP for vehicles or infantry without bhs.dll
	_Set_Animation Set_Animation; //tested,doesnt work in MP unless you have bhs.dll
	_Set_Animation_Frame Set_Animation_Frame; //tested,doesnt work in MP unless you have bhs.dll
	_Create_Sound Create_Sound; //tested,this one takes a preset,doesnt work in MP unless you have bhs.dll
	_Create_2D_Sound Create_2D_Sound; //tested,this one takes a preset,doesnt work in MP unless you have bhs.dll
45	_Create_2D_WAV_Sound Create_2D_WAV_Sound; //tested,doesnt work in MP unless you have bhs.dll
	_Create_3D_WAV_Sound_At_Bone Create_3D_WAV_Sound_At_Bone; //tested,doesnt work in MP unless you have bhs.dll
	_Create_3D_Sound_At_Bone Create_3D_Sound_At_Bone; //tested,doesnt work in MP unless you have bhs.dll
	_Create_Logical_Sound Create_Logical_Sound; //tested,this one takes a "logical sound",doesnt work in MP
	_Start_Sound Start_Sound; //tested,doesnt work in MP
50	_Stop_Sound Stop_Sound; //tested,doesnt work in MP
	_Monitor_Sound Monitor_Sound; //tested,doesnt work in MP
	_Set_Background_Music Set_Background_Music; //tested,doesnt work in MP unless you have bhs.dll
	_Fade_Background_Music Fade_Background_Music; //tested,doesnt work in MP unless you have bhs.dll
	_Stop_Background_Music Stop_Background_Music; //tested,doesnt work in MP unless you have bhs.dll
55	_Get_Health Get_Health; //tested
	_Get_Max_Health Get_Max_Health; //tested
	_Set_Health Set_Health; //tested
	_Get_Shield_Strength Get_Shield_Strength; //tested
	_Get_Max_Shield_Strength Get_Max_Shield_Strength; //tested
60	_Set_Shield_Strength Set_Shield_Strength; //tested
	_Set_Shield_Type Set_Shield_Type; //tested
	_Get_Player_Type Get_Player_Type; //tested,doesnt work on Buildings or Script Zones
	_Set_Player_Type Set_Player_Type; //tested,doesnt work on Buildings or Script Zones, doesnt work 100% in MP without bhs.dll (I think)
	_Get_Distance Get_Distance; //tested
65	_Set_Camera_Host Set_Camera_Host; //tested,doesnt work in MP
	_Force_Camera_Look Force_Camera_Look; //tested,doesnt work in MP unless you have bhs.dll
	_Get_The_Star Get_The_Star; //tested,not for MP
	_Get_A_Star Get_A_Star; //tested
	_Find_Closest_Soldier Find_Closest_Soldier; //tested
70	_Is_A_Star Is_A_Star; //tested
	_Control_Enable Control_Enable; //tested
	_Get_Damage_Bone_Name Get_Damage_Bone_Name; //tested
	_Get_Damage_Bone_Direction Get_Damage_Bone_Direction;
	_Is_Object_Visible Is_Object_Visible;
75	_Enable_Enemy_Seen Enable_Enemy_Seen; //tested
	_Set_Display_Color Set_Display_Color; //doesnt work in MP without bhs.dll
	_Display_Text Display_Text; //tested,doesnt work in MP without bhs.dll
	_Display_Float Display_Float; //tested,doesnt work in MP without bhs.dll
	_Display_Int Display_Int; //tested,doesnt work in MP without bhs.dll
80	_Save_Data Save_Data; //tested,doesnt work in MP
	_Save_Pointer Save_Pointer; //doesnt work in MP
	_Load_Begin Load_Begin; //tested,doesnt work in MP
	_Load_Data Load_Data; //tested,doesnt work in MP
	_Load_Pointer Load_Pointer; //doesnt work in MP
85	_Load_End Load_End; //tested,doesnt work in MP
	_Begin_Chunk Begin_Chunk; //tested,doesnt work in MP
	_End_Chunk End_Chunk; //tested,doesnt work in MP
	_Open_Chunk Open_Chunk; //tested,doesnt work in MP
	_Close_Chunk Close_Chunk; //tested,doesnt work in MP
90	_Clear_Radar_Markers Clear_Radar_Markers; //doesnt work in MP
	_Clear_Radar_Marker Clear_Radar_Marker; //tested,doesnt work in MP
	_Add_Radar_Marker Add_Radar_Marker; //tested,doesnt work in MP
	_Set_Obj_Radar_Blip_Shape Set_Obj_Radar_Blip_Shape; //tested,doesnt work in MP
	_Set_Obj_Radar_Blip_Color Set_Obj_Radar_Blip_Color; //tested,doesnt work in MP
95	_Enable_Radar Enable_Radar; //tested,doesnt work in MP unless you have bhs.dll
	_Clear_Map_Cell Clear_Map_Cell; //tested,doesnt work in MP
	_Clear_Map_Cell_By_Pos Clear_Map_Cell_By_Pos; //doesnt work in MP
	_Clear_Map_Cell_By_Pixel_Pos Clear_Map_Cell_By_Pixel_Pos; //doesnt work in MP
	_Clear_Map_Region_By_Pos Clear_Map_Region_By_Pos; //doesnt work in MP
100	_Reveal_Map Reveal_Map; //doesnt work in MP
	_Shroud_Map Shroud_Map; //doesnt work in MP
	_Show_Player_Map_Marker Show_Player_Map_Marker; //doesnt work in MP
	_Get_Safe_Flight_Height Get_Safe_Flight_Height; //tested
	_Create_Explosion Create_Explosion; //tested,doesnt work in MP unless you have bhs.dll
105	_Create_Explosion_At_Bone Create_Explosion_At_Bone; //tested,doesnt work in MP unless you have bhs.dll
	_Enable_HUD Enable_HUD; //tested,doesnt work in MP unless you have bhs.dll
	_Mission_Complete Mission_Complete; //tested,doesnt work in MP
	_Give_Powerup Give_Powerup; //tested
	_Innate_Disable Innate_Disable; //tested
110	_Innate_Enable Innate_Enable; //tested
	_Innate_Soldier_Enable_Enemy_Seen Innate_Soldier_Enable_Enemy_Seen; //tested
	_Innate_Soldier_Enable_Gunshot_Heard Innate_Soldier_Enable_Gunshot_Heard; //tested
	_Innate_Soldier_Enable_Footsteps_Heard Innate_Soldier_Enable_Footsteps_Heard; //tested
	_Innate_Soldier_Enable_Bullet_Heard Innate_Soldier_Enable_Bullet_Heard; //tested
115	_Innate_Soldier_Enable_Actions Innate_Soldier_Enable_Actions; //tested
	_Set_Innate_Soldier_Home_Location Set_Innate_Soldier_Home_Location;
	_Set_Innate_Aggressiveness Set_Innate_Aggressiveness; //tested
	_Set_Innate_Take_Cover_Probability Set_Innate_Take_Cover_Probability;
	_Set_Innate_Is_Stationary Set_Innate_Is_Stationary;
120	_Innate_Force_State_Bullet_Heard Innate_Force_State_Bullet_Heard;
	_Innate_Force_State_Footsteps_Heard Innate_Force_State_Footsteps_Heard;
	_Innate_Force_State_Gunshots_Heard Innate_Force_State_Gunshots_Heard;
	_Innate_Force_State_Enemy_Seen Innate_Force_State_Enemy_Seen;
	_Static_Anim_Phys_Goto_Frame Static_Anim_Phys_Goto_Frame; //tested,doesnt work in MP
125	_Static_Anim_Phys_Goto_Last_Frame Static_Anim_Phys_Goto_Last_Frame; //tested,doesnt work in MP
	_Get_Sync_Time Get_Sync_Time; //tested
	_Add_Objective Add_Objective; //tested,doesnt work in MP
	_Remove_Objective Remove_Objective; //tested,doesnt work in MP
	_Set_Objective_Status Set_Objective_Status; //tested,doesnt work in MP
130	_Change_Objective_Type Change_Objective_Type; //tested,doesnt work in MP
	_Set_Objective_Radar_Blip Set_Objective_Radar_Blip; //tested,doesnt work in MP
	_Set_Objective_Radar_Blip_Object Set_Objective_Radar_Blip_Object; //tested,doesnt work in MP
	_Set_Objective_HUD_Info Set_Objective_HUD_Info; //tested,doesnt work in MP
	_Set_Objective_HUD_Info_Position Set_Objective_HUD_Info_Position; //tested,doesnt work in MP
135	_Shake_Camera Shake_Camera; //tested, doesnt work in MP without bhs.dll
	_Enable_Spawner Enable_Spawner; //tested
	_Trigger_Spawner Trigger_Spawner;
	_Enable_Engine Enable_Engine; //tested,does something to vehicle sounds and possibly something else
	_Get_Difficulty_Level Get_Difficulty_Level; //tested
140	_Grant_Key Grant_Key; //tested
	_Has_Key Has_Key; //tested
	_Enable_Hibernation Enable_Hibernation; //tested
	_Attach_To_Object_Bone Attach_To_Object_Bone; //tested
	_Create_Conversation Create_Conversation; // doesnt work in MP
145	_Join_Conversation Join_Conversation; // doesnt work in MP
	_Join_Conversation_Facing Join_Conversation_Facing; // doesnt work in MP
	_Start_Conversation Start_Conversation; // doesnt work in MP
	_Monitor_Conversation Monitor_Conversation; // doesnt work in MP
	_Start_Random_Conversation Start_Random_Conversation; // doesnt work in MP
150	_Stop_Conversation Stop_Conversation; // doesnt work in MP
	_Stop_All_Conversations Stop_All_Conversations; // doesnt work in MP
	_Lock_Soldier_Facing Lock_Soldier_Facing; //tested
	_Unlock_Soldier_Facing Unlock_Soldier_Facing; //tested
	_Apply_Damage Apply_Damage; //tested,the "damager" is the object that should be treated as doing the damage (for the purposes of the Damaged event and probobly also points)
155	_Set_Loiters_Allowed Set_Loiters_Allowed; //tested
	_Set_Is_Visible Set_Is_Visible; //tested (only affects the Is_Object_Visible command and also I think Enemy_Seen logic)
	_Set_Is_Rendered Set_Is_Rendered; //tested
	_Get_Points Get_Points; //tested
	_Give_Points Give_Points; //tested,negative numbers work
160	_Get_Money Get_Money; //tested
	_Give_Money Give_Money; //tested,negative numbers work
	_Get_Building_Power Get_Building_Power; //tested
	_Set_Building_Power Set_Building_Power; //tested (only the base defenses seem to actually power down)
	_Play_Building_Announcement Play_Building_Announcement; //doesnt work in mp without bhs.dll
165	_Find_Nearest_Building Find_Nearest_Building;
	_Find_Nearest_Building_To_Pos Find_Nearest_Building_To_Pos;
	_Team_Members_In_Zone Team_Members_In_Zone;
	_Set_Clouds Set_Clouds; //tested
	_Set_Lightning Set_Lightning; //tested
170	_Set_War_Blitz Set_War_Blitz; //tested,doesnt work in mp unless you have bhs.dll
	_Set_Wind Set_Wind; //tested
	_Set_Rain Set_Rain; //tested
	_Set_Snow Set_Snow; //tested
	_Set_Ash Set_Ash; //tested
175	_Set_Fog_Enable Set_Fog_Enable; //tested,doesnt work in MP unless you have bhs.dll
	_Set_Fog_Range Set_Fog_Range; //tested,doesnt work in MP unless you have bhs.dll
	_Enable_Stealth Enable_Stealth; //tested,doesnt work in MP unless you have bhs.dll
	_Cinematic_Sniper_Control Cinematic_Sniper_Control; //tested (the sniper effect probobly only happens if you call other commands first or something,more work needed),doesnt work in MP
	_Text_File_Open Text_File_Open; //tested
180	_Text_File_Get_String Text_File_Get_String; //tested
	_Text_File_Close Text_File_Close; //tested
	_Enable_Vehicle_Transitions Enable_Vehicle_Transitions; //tested,doesnt stop you from getting out,only in (I think), doesnt work 100% in MP without bhs.dll
	_Display_GDI_Player_Terminal Display_GDI_Player_Terminal; //tested,doesnt work in MP unless you have bhs.dll
	_Display_NOD_Player_Terminal Display_NOD_Player_Terminal; //tested,doesnt work in MP unless you have bhs.dll
185	_Display_Mutant_Player_Terminal Display_Mutant_Player_Terminal; //crashes renegade if you use it
	_Reveal_Encyclopedia_Character Reveal_Encyclopedia_Character; //tested,doesnt work in MP
	_Reveal_Encyclopedia_Weapon Reveal_Encyclopedia_Weapon; //tested,doesnt work in MP
	_Reveal_Encyclopedia_Vehicle Reveal_Encyclopedia_Vehicle; //tested,doesnt work in MP
	_Reveal_Encyclopedia_Building Reveal_Encyclopedia_Building; //tested,doesnt work in MP
190	_Display_Encyclopedia_Event_UI Display_Encyclopedia_Event_UI; //tested,doesnt work in MP
	_Scale_AI_Awareness Scale_AI_Awareness;
	_Enable_Cinematic_Freeze Enable_Cinematic_Freeze;
	_Expire_Powerup Expire_Powerup; //tested
	_Set_HUD_Help_Text Set_HUD_Help_Text; //tested,doesnt work in MP
195	_Enable_HUD_Pokable_Indicator Enable_HUD_Pokable_Indicator; //tested
	_Enable_Innate_Conversations Enable_Innate_Conversations; //tested
	_Display_Health_Bar Display_Health_Bar; //tested
	_Enable_Shadow Enable_Shadow; //tested
	_Clear_Weapons Clear_Weapons; //tested, doesnt work in MP for vehicles without bhs.dll
200	_Set_Num_Tertiary_Objectives Set_Num_Tertiary_Objectives; //tested,doesnt work in MP
	_Enable_Letterbox Enable_Letterbox; //tested,doesnt work in MP
	_Set_Screen_Fade_Color Set_Screen_Fade_Color; //tested,doesnt work in MP unless you have bhs.dll
	_Set_Screen_Fade_Opacity Set_Screen_Fade_Opacity; //tested,doesnt work in MP unless you have bhs.dll
};
*/
#endregion

namespace RGsHarp
{
    /// <summary>
    /// implementation of the ScriptCommands class
    /// </summary>
    public class ScriptCommands
    {
        // pointer to the scriptscommand struct as of Renegade 1.037
        protected static IntPtr pScriptCommands = (IntPtr) 0x85F490;

        protected static IntPtr pGet_Health;
        protected static IntPtr pGet_Player_Type;
        protected static IntPtr pDisplay_GDI_Player_Terminal;
        protected static IntPtr pDisplay_NOD_Player_Terminal;

        // ProcessMemory instance of the opened renegade process
        protected ProcessUtil.ProcessMemory m_RenegadeProcessMemory;

        public ScriptCommands(ProcessUtil.ProcessMemory renegadeProcessMemory)
        {
            this.m_RenegadeProcessMemory = renegadeProcessMemory;

            // initialize the other script-pointer from pScriptCommands-base.
            // to add more, have a look at the table above, you have to look up the number of the desired command.
            // Get_Health is the 55th command for example, so the offset is 55 * 4 (4byte pointer / 32bit)
            pGet_Health = this.m_RenegadeProcessMemory.ReadPointer(ScriptCommands.pScriptCommands, 55 * 4);
            pGet_Player_Type = this.m_RenegadeProcessMemory.ReadPointer(ScriptCommands.pScriptCommands, 62 * 4);
            pDisplay_GDI_Player_Terminal = this.m_RenegadeProcessMemory.ReadPointer(ScriptCommands.pScriptCommands, 183 * 4);
            pDisplay_NOD_Player_Terminal = this.m_RenegadeProcessMemory.ReadPointer(ScriptCommands.pScriptCommands, 184 * 4);
        }

        /// <summary>
        /// Gets the health of a specific GameObject
        /// </summary>
        /// <param name="GameObject">GameObject to get health for (e.g. Player[x].GameObject)</param>
        public float Get_Health(IntPtr GameObject)
        {
            return this.m_RenegadeProcessMemory.CallFunctionFloat(ScriptCommands.pGet_Health, GameObject);
        }

        /// <summary>
        /// Gets the type (team) for a given GameObject
        /// </summary>
        /// <param name="GameObject">GameObject to get type/team for (e.g. Player[x].GameObject)</param>
        public int Get_Player_Type(IntPtr GameObject)
        {
            return this.m_RenegadeProcessMemory.SecureCallFunction(ScriptCommands.pGet_Player_Type, GameObject);
        }

        public void Display_GDI_Player_Terminal()
        {
            this.m_RenegadeProcessMemory.CallFunction(ScriptCommands.pDisplay_GDI_Player_Terminal, IntPtr.Zero);
        }
        public void Display_NOD_Player_Terminal()
        {
            this.m_RenegadeProcessMemory.CallFunction(ScriptCommands.pDisplay_NOD_Player_Terminal, IntPtr.Zero);
        }

    }
}
