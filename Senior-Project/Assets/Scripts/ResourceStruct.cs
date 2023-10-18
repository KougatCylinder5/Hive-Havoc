using System.Diagnostics;

public struct ResourceStruct
{
    private static int _wood;
    public static int Wood 
    {  
        get 
        {
            return _wood; 
        }
        set 
        {
            if(_wood + value > 0)
            {
                _wood = value;
            }
            else
            {
                Debug.Print("You can't afford that. Make something happen here!");
            }
        } 
    }

}
