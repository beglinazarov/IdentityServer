using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
	public static class Configuration
	{
		public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource> {
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResource
				{
					Name = "begs.scope",
					UserClaims = 
					{
					"my.claim" 
					}

				}

			};

		public static IEnumerable<ApiResource> GetApis() =>
			new List<ApiResource>{
				new ApiResource("ApiOne", new string[] { "my.api.claim" }),
				new ApiResource("ApiTwo"),
			};
		public static IEnumerable<Client> GetClients() =>
			new List<Client> {
				new Client{
					ClientId ="client_id",
					ClientSecrets = {new Secret("client_secret".ToSha256()) },

					AllowedGrantTypes = { GrantType.ClientCredentials },
					AllowedScopes = { "ApiOne" }
				},
				new Client{
					ClientId ="client_id_mvc",
					ClientSecrets = {new Secret("client_secret_mvc".ToSha256()) },

					RedirectUris = {"https://localhost:44369/signin-oidc" },

					AllowedGrantTypes = GrantTypes.Code,
					AllowedScopes = {
						"ApiOne",
						"ApiTwo",
						IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
						IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
						"begs.scope"
					},

					// puts all the claims in the id token
					//AlwaysIncludeUserClaimsInIdToken = true,
					RequireConsent = false,
				},
			};
	}
}
