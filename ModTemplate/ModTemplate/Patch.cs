using HarmonyLib;

namespace ModTemplate
{
    //패치 할때는 무조건 static
    public static class Patch
    {
        
        //[HarmonyPatch(typeof(Type_Name),"Method_Name")] //해당 메소드가 시작 전 또는 끝난 후 실행할 코드 설정
        //타일 이동될때 판정 가져오기
        [HarmonyPatch(typeof(scrPlanet), "MoveToNextFloor")]
        //클래스 이름은 아무렇게나 지어도 됨
        public static class MoveToNextFloor
        {
            //__파람미터 이용
            //Prefix는 시작 전, Postfix는 끝난 후
            public static void Prefix(HitMargin hitMargin)
            {
                //가끔 모드가 꺼지지 않는것을 방지 하기 위해  return
                if (!Main.IsEnabled) return;
                Main.testGUI.text = hitMargin.ToString();
            }
        }
        
    }
}