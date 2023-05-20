using System;
using System.Windows;

namespace PSModules;

/// <summary>
///     Класс <c>DynamicResourcesHelper</c> нужен для написания более чистого кода по работе <c>DynamicResources</c>
/// </summary>
public static class DynamicResourcesHelper
{
    public static readonly Application App;

    static DynamicResourcesHelper()
    {
        /// <remarks>
        /// В значение параметра <c>_app</c> нужно передать Application.Current
        /// </remarks>
        App = Application.Current;
    }

    public static int Create(string name, double value)
    {
        /// <remarks>
        /// Создает новый динамический ресурс
        /// </remarks>
        try
        {
            App.MainWindow.Resources.Add(name, value);
        }
        catch (Exception e)
        {
            if (App.MainWindow.Resources.Contains(name)) return 1; // Element already created
            return 2; // Unknown error
        }

        return 0;
    }

    public static int Update(string name, double value)
    {
        /// <remarks>
        /// Изменяет значение динамического ресурса
        /// </remarks>
        try
        {
            App.MainWindow.Resources[name] = value;
        }
        catch (Exception e)
        {
            if (!App.MainWindow.Resources.Contains(name)) return 1; // Element does not exist

            return 2; // Unknown error
        }

        return 0;
    }
}