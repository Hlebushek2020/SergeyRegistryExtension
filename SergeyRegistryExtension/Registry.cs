using Microsoft.Win32;

namespace SergeyRegistryExtension
{
    public static class Registry
    {
        private const string prefix = "SG_";

        /// <summary>
        /// Ассоциация расширения файла с программой
        /// </summary>
        /// <param name="expansion">Расширение</param>
        /// <param name="productName">Имя продукта</param>
        /// <param name="pathApp">Путь к exe приложения</param>
        /// <param name="pathIcon">Путь к иконке файла (Добускается значение null или string.Empty)</param>
        public static void AssociateExtension(string expansion, string productName, string pathApp, string pathIcon)
        {
            RegistryKey reg = null;
            try
            {
                reg = Microsoft.Win32.Registry.CurrentUser;
                reg = reg.OpenSubKey("Software\\Classes", true);
                reg.CreateSubKey(expansion).SetValue("", $"{prefix}{productName}");
                reg = reg.CreateSubKey($"{prefix}{productName}");
                if (string.IsNullOrEmpty(pathIcon) == false)
                    reg.CreateSubKey("DefaultIcon").SetValue("", $"\"{pathIcon}\"");
                reg = reg.CreateSubKey("Shell");
                reg = reg.CreateSubKey("Open");
                reg.CreateSubKey("Command").SetValue("", $"\"{pathApp}\" \"%1\"");
            }
            finally
            {
                if (reg != null)
                    reg.Dispose();
            }
        }

        /// <summary>
        /// Удаление ассоциации расширения файла с программой
        /// </summary>
        /// <param name="expansion">Расширение</param>
        /// <param name="productName">Имя продукта</param>
        public static void RemoveAssociateExtension(string expansion, string productName)
        {
            RegistryKey reg = null;
            try
            {
                reg = Microsoft.Win32.Registry.CurrentUser;
                reg = reg.OpenSubKey("Software\\Classes", true);
                reg.OpenSubKey(expansion, true).DeleteValue($"{prefix}{productName}", false);
                reg.DeleteSubKeyTree($"{prefix}{productName}", false);
            }
            finally
            {
                if (reg != null)
                    reg.Dispose();
            }
        }
    }
}
