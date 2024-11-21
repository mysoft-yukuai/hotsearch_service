using System.Reflection;

namespace System
{
    public static class ObjectExtend
    {
        public static long ToLong(this object obj, long def = 0)
        {
            if (long.TryParse(obj.ToString(), out var val))
                return val;
            
            return def;
        }

        public static int ToInt(this object obj, int def = 0)
        {
            if (int.TryParse(obj.ToString(), out var val))
                return val;

            return def;
        }
    }
}
