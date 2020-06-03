﻿namespace DMS.GLSL.Options
{
	using System;
	using System.CodeDom;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Threading.Tasks;
	using EnvDTE;
	using Microsoft.VisualStudio;
	using Microsoft.VisualStudio.Shell;
	using Microsoft.VisualStudio.Shell.Interop;

	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(PackageGuidString)]
	[ProvideOptionPage(typeof(Options), "GLSL language integration", "Configuration", 0, 0, true)]
	public sealed class OptionsPagePackage : AsyncPackage
	{
		public const string PackageGuidString = "fd8ee466-e18c-45fc-b1a1-ca0dc1ec67fb";

		//protected override System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken, IProgress<Microsoft.VisualStudio.Shell.ServiceProgressData> progress)
		//{
		//	//this.AddService(typeof(SMyTestService), CreateService, true);
		//	return System.Threading.Tasks.Task.FromResult<object>(null);
		//}

		public static Options Options
		{
			get
			{
				if (_options is null)
				{
					try { EnsurePackageLoaded(); } catch { }
					if (_options is null) return new Options();
				}
				return _options;
			}
		}

		public static async Task<string> ExpandEnvironmentVariablesAsync(string text)
		{
			const string solutionDirVar = "$(SolutionDir)";
			if (text.Contains(solutionDirVar))
			{
				var joinableTaskFactory = ThreadHelper.JoinableTaskFactory;
				await joinableTaskFactory.SwitchToMainThreadAsync();
				lock (_syncRoot)
				{
					DTE dTE = GetGlobalService(typeof(DTE)) as DTE;
					string solutiondir = File.Exists(dTE.Solution.FileName) ? Path.GetDirectoryName(dTE.Solution.FileName) : string.Empty; // the value of $(SolutionDir)
					text = text.Replace(solutionDirVar, solutiondir);
				}
			}
			return Environment.ExpandEnvironmentVariables(text);
		}

		public static string ExpandEnvironmentVariables(string text)
		{
			var joinableTaskFactory = ThreadHelper.JoinableTaskFactory;
			return joinableTaskFactory.Run(() => ExpandEnvironmentVariablesAsync(text));
		}

		private static Options _options;
		private static readonly object _syncRoot = new object();

		private static void EnsurePackageLoaded()
		{
			var joinableTaskFactory = ThreadHelper.JoinableTaskFactory;
			joinableTaskFactory.Run(async delegate
			{
				await joinableTaskFactory.SwitchToMainThreadAsync();
				lock (_syncRoot)
				{
					var shell = (IVsShell)GetGlobalService(typeof(SVsShell));
					var guid = new Guid(PackageGuidString);
					if (shell.IsPackageLoaded(ref guid, out IVsPackage package) != VSConstants.S_OK)
					{
						ErrorHandler.Succeeded(shell.LoadPackage(ref guid, out package));
						var myPack = package as OptionsPagePackage;
						_options = (Options)myPack.GetDialogPage(typeof(Options));
					}
				}
			});
		}
	}
}
