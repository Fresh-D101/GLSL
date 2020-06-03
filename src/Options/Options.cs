﻿using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace DMS.GLSL.Options
{
	public partial class Options : DialogPage
	{
		private string _userKeyWords;

		[Category("General")]
		[DisplayName("Arguments for the external compiler executable")]
		[Description("Command line arguments for the external compiler executable. Can contain environment variables, like %USERPROFILE% and also the Visual Studio variable $(SolutionDir).")]
		public string ExternalCompilerArguments { get; set; } = string.Empty;

		[Category("General")]
		[DisplayName("External compiler executable file path (without quotes)")]
		[Description("If non empty this file will be executed for each shader and the output parsed for error squiggles")]
		public string ExternalCompilerExeFilePath { get; set; } = string.Empty;

		[Category("General")]
		[DisplayName("Live compiling")]
		[Description("Compile the shader code in the background and show resulting errors")]
		public bool LiveCompiling { get; set; } = true;

		[Category("General")]
		[DisplayName("User key words")]
		[Description("Space separated list of user key words (used for coloring)")]
		public string UserKeyWords
		{
			get => _userKeyWords;
			set
			{
				_userKeyWords = value;
				GlslSpecification.SetUserKeyWords(value);
			}
		}

		[Category("General")]
		[DisplayName("Compile delay(ms)")]
		[Description("Minimal delay between two compiles.")]
		public int CompileDelay { get; set; } = 200;

		[Category("General")]
		[DisplayName("Print compilation result")]
		[Description("Print shader code compilation result to output window pane")]
		public bool PrintCompilationResult { get; set; } = true;
	}
}
