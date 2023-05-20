using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSModules
{
    /// <summary>
    /// Класс <c>DynamicResourcesHelper</c> нужен для написания более чистого кода по работе <c>DynamicResources</c>
    /// </summary>
    public static class DynamicResourcesHelper
    {
        public static Application app;

        static DynamicResourcesHelper()
        {
            /// <remarks>
            /// В значение параметра <c>_app</c> нужно передать Application.Current
            /// </remarks>
            app = Application.Current;
        }

        public static int Create(string _name, double _value)
        {
            /// <remarks>
            /// Создает новый динамический ресурс
            /// </remarks>
            try
            {
                app.MainWindow.Resources.Add(_name, _value);
            }
            catch (Exception e)
            {
                if (app.MainWindow.Resources.Contains(_name)) {
                    return 1; // Element already created
                }
                return 2; // Unknown error
            }

            return 0;
        }

        public static int Update(string _name, double _value)
        {
            /// <remarks>
            /// Изменяет значение динамического ресурса
            /// </remarks>
            try
            {
                app.MainWindow.Resources[_name] = _value;
            }
            catch (Exception e) 
            {
                if (!(app.MainWindow.Resources.Contains(_name)))
                {
                    return 1; // Element does not exist
                }

                return 2; // Unknown error
            }

            return 0;
        }
    }
}
