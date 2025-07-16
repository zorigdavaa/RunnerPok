// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Or9tb9EyPn9eF5J3iJqzIBElHULkulQJIzI/7k6xe1niSDgmfS+fBt0ft8miwWZsEtZrapc9zO8CPmyezORLocvz0jRNZw6IQvJPa9WXX4R6yEtoekdMQ2DMAsy9R0tLS09KSQDjcxwwzCYCcgZDK6+w0Ip7Vbkr4RqRo5zDL8OBRXWQX6KfI0PAR0O7ZwcdX3dcNySY3h1VOQXrG/nOOu/IoL7HDQ6eXjQYR4Y20l1FMyg8yEtFSnrIS0BIyEtLSo54PE6BUMgZos78SbQhIS2+RVgWGX0BOCEO/KYNpMNBzBdAJT3ixk1Y5oeG2T0cQVenMSk+SefScZLXzeAcU9eMKJFpvQEJX0J0+ndKG2dPiQR+j7B4RR1ushds3EBwY0hJS0pL");
        private static int[] order = new int[] { 6,10,12,12,6,13,8,11,9,10,11,11,12,13,14 };
        private static int key = 74;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
