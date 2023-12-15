using UnityEditor.ShaderGraph.Internal;
using static ResourceCollectBuilding;
public class ResourceConvertBuilding : BuildingClass
{
    private int reset = 10;
    public int[] costPerConversion;
    public GatheringType[] convertType;
    public int makePerConversion;
    public GatheringType makeType;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(ConvertResources), 0, reset);
    }

    public void ConvertResources()
    {
        bool flag = true;
        for(int i = 0; i < costPerConversion.Length; i++)
        {
            if (ResourceStruct.Total[i] < costPerConversion[i])
            {
                flag = false;
                break;
            }
        }
        if(flag)
        {
            foreach (GatheringType type in convertType)
            {
                switch (type)
                {
                    case GatheringType.Wood:
                        ResourceStruct.Wood -= costPerConversion[(int)type];
                        break;
                    case GatheringType.Stone:
                        ResourceStruct.Stone -= costPerConversion[(int)type];
                        break;
                    case GatheringType.Coal:
                        ResourceStruct.Coal -= costPerConversion[(int)type];
                        break;
                    case GatheringType.CopperOre:
                        ResourceStruct.CopperOre -= costPerConversion[(int)type];
                        break;
                    case GatheringType.CopperIngot:
                        ResourceStruct.CopperIngot -= costPerConversion[(int)type];
                        break;
                }

            }

            switch (makeType)
            {
                case GatheringType.Wood:
                    ResourceStruct.Wood += makePerConversion;
                    break;
                case GatheringType.Stone:
                    ResourceStruct.Stone += makePerConversion;
                    break;
                case GatheringType.Coal:
                    ResourceStruct.Coal += makePerConversion;
                    break;
                case GatheringType.CopperOre:
                    ResourceStruct.CopperOre += makePerConversion;
                    break;
                case GatheringType.CopperIngot:
                    ResourceStruct.CopperIngot += makePerConversion;
                    break;
            }
        }
        
    }
}