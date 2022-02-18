using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TypeColor
{
    private static Dictionary<Type, string> typecolors = new Dictionary<Type, string>()
    {
        {Type.Common, "#FFE6A5" },
        {Type.Element, "#05d500" },
        {Type.Legendary, "#c600d2" }
    };

    public static Dictionary<Type, string> MyTypeColors { get => typecolors;  }
}