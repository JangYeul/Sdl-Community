﻿using Sdl.Community.SignoffVerifySettings.Helpers;
using Sdl.Community.SignoffVerifySettings.Model;
using Sdl.Community.SignoffVerifySettings.Report;
using Sdl.Community.SignoffVerifySettings.Service;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.ProjectAutomation.AutomaticTasks;
using Sdl.ProjectAutomation.Core;

namespace Sdl.Community.SignoffVerifySettings
{
	[AutomaticTask("SignoffVerifySettingsID", "Signoff Verify Settings", "Signoff Verify Settings", GeneratedFileType = AutomaticTaskFileType.BilingualTarget)]
	[AutomaticTaskSupportedFileType(AutomaticTaskFileType.BilingualTarget)]
	[RequiresSettings(typeof(SignoffVerifySettings), typeof(SignoffVerifySettingsPage))]
	class SignoffVerifySettingsBatchTask : AbstractFileContentProcessingAutomaticTask
	{
		private ProjectInfoReportModel _projectInfoReportModel = new ProjectInfoReportModel();
		protected override void OnInitializeTask()
		{
			if (TaskFiles.Length > 0)
			{
				var projectService = new ProjectService();
				_projectInfoReportModel = projectService.GetCurrentProjectInformation(TaskFiles);
			}
		}
		protected override void ConfigureConverter(ProjectFile projectFile, IMultiFileConverter multiFileConverter)
		{
			var fileConverter = new FileConverter();
			multiFileConverter.AddBilingualProcessor(fileConverter);
		}

		public override void TaskComplete()
		{
			base.TaskComplete();

			var report = ReportGenerator.GenerateReport();
			CreateReport(Constants.ReportName, "Verification statistics", report);
		} 
	}
}