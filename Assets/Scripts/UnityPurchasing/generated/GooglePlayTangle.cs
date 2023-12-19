// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("FPvQNBCwpk9tSE8bFQd6cmyFbgnzoguBLd1pj3s+lUFSi0zDqIYSCv5Mz+z+w8jH5EiGSDnDz8/Py87Ndf7eSaxvsxaJPyMamDHk+1v+2jXqX/98OLor0sIHRuO98bxgLr6KG+FdeAWT4R99PcvB4A1R/r9JM5n+IRIe5sJ702mhE1BpA2BJXCCdMqpMz8HO/kzPxMxMz8/OV0Uvxdzm+Lf0wsKy8NJJXvkF3lxMGg6voySbWGbGl1RIb7A0kvckWHSz7iC6Bg+Iq+am9JY+6v9GPYvn4xfiQdboiR+Qew1j50JknAjn3lWRlIjNUYzLy66awlBJJlzM7lRQIUuv0vVDEwtQthxdZ9gBNgs7yBZGGVho3in0WBX2ZgQ+GiTR7czNz87P");
        private static int[] order = new int[] { 2,4,2,6,11,9,9,11,12,13,11,11,12,13,14 };
        private static int key = 206;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
