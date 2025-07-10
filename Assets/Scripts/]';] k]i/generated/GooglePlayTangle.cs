// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NvRcIkkqjYf5PYCBfNYnBOnVh3XySSUXol/KysZVrrP98pbq08rlF9FUhoQ62dWUtfx5nGNxWMv6zvapUIzs9rSct9zPczX2vtLuAPASJdGqvEzawtWiDDmaeTwmC/e4PGfDek3mTyiqJ/yrztYJLaazDWxtMtb3CvF6SHcoxChqrp57tEl0yKgrrKiRI6CDkaynqIsn6SdWrKCgoKShooJW6uK0qZ8RnKHwjKRi75VkW5OuBCNLVSzm5XW13/Osbd05tq7Yw9cPUb/iyNnUBaVakLIJo9PNlsR07ScPoEogGDnfpozlY6kZpIA+fLRv6wiY99snzemZ7ajARFs7YZC+UsAjoK6hkSOgq6MjoKChZZPXpWq7I/aFWfyHN6ubiKOioKGg");
        private static int[] order = new int[] { 12,7,6,11,11,11,8,12,11,9,10,11,13,13,14 };
        private static int key = 161;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
