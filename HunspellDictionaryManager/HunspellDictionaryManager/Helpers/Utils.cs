﻿using System;
using System.Linq;
using System.Windows;

namespace Sdl.Community.HunspellDictionaryManager.Helpers
{
	public static class Utils
	{
		public static readonly Log Log = Log.Instance;

		public static void InitializeWpfApplicationSettings()
		{
			if (Application.Current == null)
			{
				new Application();
			}
			if (Application.Current != null)
			{
				Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
				var controlsResources = new ResourceDictionary
				{
					Source = new Uri(Constants.ControlsStylePath, UriKind.Absolute)
				};
				var colorsResources = new ResourceDictionary
				{
					Source = new Uri(Constants.ColorsStylePath, UriKind.Absolute)
				};
				var fontsResources = new ResourceDictionary
				{
					Source = new Uri(Constants.FontsStylePath, UriKind.Absolute)
				};
				var greenResources = new ResourceDictionary
				{
					Source = new Uri(Constants.GreenAccentStylePath, UriKind.Absolute)
				};
				var baseLightResources = new ResourceDictionary
				{
					Source = new Uri(Constants.BaseLightAccentStylePath, UriKind.Absolute)
				};
				var flatButtonsResources = new ResourceDictionary
				{
					Source = new Uri(Constants.FlatButtonStylePath, UriKind.Absolute)
				};

				Application.Current.Resources.MergedDictionaries.Add(colorsResources);
				Application.Current.Resources.MergedDictionaries.Add(greenResources);
				Application.Current.Resources.MergedDictionaries.Add(baseLightResources);
				Application.Current.Resources.MergedDictionaries.Add(flatButtonsResources);
				Application.Current.Resources.MergedDictionaries.Add(controlsResources);
			}
		}

        /// <summary>
        /// Get installed version for Studio 2019
        /// </summary>
        /// <returns></returns>
        public static string GetInstalledStudioPath()
        {
			try
			{
				var studio = new Toolkit.Core.Studio().GetInstalledStudioVersion()?.Where(v => v.Version.Equals("Studio15")).FirstOrDefault();
				if (studio != null)
				{
					return studio.InstallPath;
				}
				else
				{
					MessageBox.Show(Constants.Studio2019ErrorMessage, Constants.InformativeMessage, MessageBoxButton.OK, MessageBoxImage.Error);					
				}
			}
			catch (Exception ex)
			{
				Log.Logger.Error($"{Constants.GetInstalledStudioPath}: {ex.Message}\n {ex.StackTrace}");
			}
			return string.Empty;
		}
	}
}