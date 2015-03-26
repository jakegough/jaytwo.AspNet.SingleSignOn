using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.ProviderAuthenticationPolicy;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using jaytwo.AspNet.SingleSignOn;
using jaytwo.AspNet.SingleSignOn.Security;
using jaytwo.AspNet.SingleSignOn.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace jaytwo.AspNet.MvcDemo.Security
{
    public class OpenIdAppHost : SsoAppHost
    {
        public OpenIdAppHost(string discoveryUrl)
            : base(authenticationProviderEndpoint: discoveryUrl)
        {
        }

        public override string GetRedirectToAuthenticationProviderUrl(HttpRequestBase request, string returnHandlerUrl)
        {
            var realm = AspNetUtility.GetRootApplicationUri(request);
            var reutrnUri = new Uri(returnHandlerUrl);
            var openIdRequest = new OpenIdRelyingParty().CreateRequest(AuthenticationEndpointUrl, realm, reutrnUri);
            openIdRequest.Mode = AuthenticationRequestMode.Setup;
            openIdRequest.AddExtension(new ClaimsRequest { Email = DemandLevel.Require });
            openIdRequest.AddExtension(new PolicyRequest { MaximumAuthenticationAge = TimeSpan.Zero });

            return openIdRequest.RedirectingResponse.Headers["Location"];
        }

        public override AuthenticationProviderUserInfo GetUserInformationFromAuthenticationProviderReturn(HttpRequestBase authenticationProviderReturnRequest)
        {
            var relyingPartyResponse = new OpenIdRelyingParty().GetResponse(authenticationProviderReturnRequest);

            if (relyingPartyResponse != null)
            {
                switch (relyingPartyResponse.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var result = new AuthenticationProviderUserInfo();
                        result.Data = new Dictionary<string, object>();

                        var fetch = relyingPartyResponse.GetExtension<FetchResponse>();

                        string email;
                        if (fetch != null)
                        {
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                        }
                        else
                        {
                            var claimsResponse = relyingPartyResponse.GetExtension<ClaimsResponse>();
                            email = claimsResponse.Email;
                        }

                        result.UserName = email;

                        return result;

                    case AuthenticationStatus.Canceled:
                        throw new Exception("Login was cancelled at the provider");

                    case AuthenticationStatus.Failed:
                        throw new Exception("Login failed using the provided OpenId identifier : " + relyingPartyResponse.Exception.Message);

                    default:
                        throw new Exception("Unknown OpenId status : " + relyingPartyResponse.Status);
                }
            }

            throw new Exception("No OpenId Response.");
        }

        private static void AddIfNotNullOrWhitSpace(IDictionary<string, object> dictionary, string key, object value)
        {
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                var valueAsString = value as string;

                if (valueAsString == null || !string.IsNullOrWhiteSpace(valueAsString))
                {
                    dictionary.Add(key, value);
                }
            }
        }
    }
}