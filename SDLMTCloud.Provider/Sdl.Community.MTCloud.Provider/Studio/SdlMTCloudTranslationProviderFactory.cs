﻿using System;
using Sdl.Community.MTCloud.Provider.Service;
using Sdl.LanguageCloud.IdentityApi;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace Sdl.Community.MTCloud.Provider.Studio
{
	[TranslationProviderFactory(Id = "SDLMachineTranslationCloudProviderFactory",
		Name = "SDLMachineTranslationCloudProviderFactory",
		Description = "SDL Machine Translation Cloud Provider")]
	public class SdlMTCloudTranslationProviderFactory : ITranslationProviderFactory
	{		
		public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState,
			ITranslationProviderCredentialStore credentialStore)
		{			
			var connectionService = new ConnectionService(StudioInstance.GetActiveForm(), new VersionService(), LanguageCloudIdentityApi.Instance);
						
			var credential = connectionService.GetCredential(credentialStore);
			var connectionResult = connectionService.EnsureSignedIn(credential);

			if (!connectionResult.Item1)
			{
				throw new TranslationProviderAuthenticationException(PluginResources.Message_Invalid_credentials);
			}

			connectionService.SaveCredential(credentialStore);

			var languageMappingsService = new LanguageMappingsService();
			var translationService = new TranslationService(connectionService, languageMappingsService);
			var editorController = SdlTradosStudio.Application?.GetController<EditorController>();
			
			var provider = new SdlMTCloudTranslationProvider(translationProviderUri, translationService, translationProviderState, editorController);			
			return provider;		
		}		

		public bool SupportsTranslationProviderUri(Uri translationProviderUri)
		{
			if (translationProviderUri == null)
			{
				throw new ArgumentNullException(nameof(translationProviderUri));
			}

			var supportsProvider = string.Equals(translationProviderUri.Scheme, Constants.MTCloudUriScheme, StringComparison.OrdinalIgnoreCase);
			return supportsProvider;
		}

		public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
		{
			var info = new TranslationProviderInfo
			{
				TranslationMethod = TranslationMethod.MachineTranslation,
				Name = PluginResources.Plugin_NiceName
			};

			return info;
		}			
	}
}