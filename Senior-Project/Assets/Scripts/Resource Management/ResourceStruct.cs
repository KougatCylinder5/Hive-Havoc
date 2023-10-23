public struct ResourceStruct
{
    private static int _wood;
    public static int Wood { get { return _wood; } set { if (_wood + value > 0) { _wood = value; } } }
    private static int _coal;
    public static int Coal { get { return _coal; } set { if (_coal + value > 0) { _coal = value; } } }
    private static int _copperOre;
    public static int CopperOre { get { return _copperOre; } set { if (_copperOre + value > 0) { _copperOre = value; } } }
    private static int _copperIngot;
    public static int CopperIngot { get { return _copperIngot; } set { if (_copperIngot + value > 0) { _copperIngot = value; } } }
    private static int _ironOre;
    public static int IronOre { get { return _ironOre; } set { if (_ironOre + value > 0) { _ironOre = value; } } }
    private static int _ironIngot;
    public static int IronIngot { get { return _ironIngot; } set { if (_ironIngot + value > 0) { _ironIngot = value; } } }
    private static int _steel;
    public static int Steel { get { return _steel; } set { if (_steel + value > 0) { _steel = value; } } }
    private static int _stone;
    public static int Stone { get { return _stone; } set { if (_stone + value > 0) { _stone = value; } } }
    private const int _total = 8;
    public static int[] Total{get{return new int[_total] { Wood, Coal, CopperOre, CopperIngot, IronOre, IronIngot, Steel, Stone };}}
}
