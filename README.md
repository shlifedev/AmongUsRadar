# AmongUsCheat

  c# based amongus memory radar.
  Reflection Class List Use by Cheatengine mono dissector.
 
 

# 사용방법
 1. 치트엔진의 모노 디섹터를 이용하여 **PlayerControl** 의 Static Offset을 찾습니다.
 2. 이후, **PlayerControl** 의 모든 인스턴스를 AOB Scan하기위해 아무런 인스턴스의 주소값을 메모리뷰에서 찾아야합니다.
 3. 메모리뷰에서 표시되는 첫 4바이트를 복사한후 코드내의 변수에 정의하고 (00 00 00 00 ?? ?? ?? ??) 게임 내에서 치트를 실행하면 됩니다.
 
 
 # Feature
  - 2D Radar
  - 2D Radar Player Color
  - 2D Radar Imposter
  - 2D Radar Latest Died Position
 
 
  - (Command) Set Imposter
  - (Command) Reset Dead Condition
 
 # Soon
  - 2D ESP
  - 2D Radar Map(Image)
  - Rage Mode
  - Filter & Show Dead Player
  - Filter & Show Imposter
  - Method Hook
  
  
 
