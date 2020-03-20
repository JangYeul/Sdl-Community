﻿using System.Collections.Generic;

namespace Sdl.Community.ReportExporter.Model
{
	public class ReportDetails
	{
		public string ProjectName { get; set; }
		public string ReportPath { get; set; }
		public Dictionary<LanguageDirection, bool> LanguagesForPoject { get; set; }
	}
}