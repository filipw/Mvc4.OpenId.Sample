using System;
using System.Web.Security;
using System.Web;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using Mvc4.OpenId.Sample.Models;

namespace Mvc4.OpenId.Sample.Security
{
    public class OpenIdMembershipService : IOpenIdMembershipService
    {
        private readonly OpenIdRelyingParty openId;

        public OpenIdMembershipService()
        {
            openId = new OpenIdRelyingParty();
        }

        public IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier)
        {
            IAuthenticationRequest openIdRequest = openId.CreateRequest(Identifier.Parse(openIdIdentifier));

            var fields = new ClaimsRequest()
            {
                Email = DemandLevel.Require,
                FullName = DemandLevel.Require,
                Nickname = DemandLevel.Require
            };
            openIdRequest.AddExtension(fields);

            return openIdRequest;
        }

        public OpenIdUser GetUser()
        {
            OpenIdUser user = null;
            IAuthenticationResponse openIdResponse = openId.GetResponse();

            if (openIdResponse.IsSuccessful())
            {
                user = ResponseIntoUser(openIdResponse);
            }

            return user;
        }

        private OpenIdUser ResponseIntoUser(IAuthenticationResponse response)
        {
            OpenIdUser user = null;
            var claimResponseUntrusted = response.GetUntrustedExtension<ClaimsResponse>();
            var claimResponse = response.GetExtension<ClaimsResponse>();

            if (claimResponse != null)
            {
                user = new OpenIdUser(claimResponse, response.ClaimedIdentifier);
            }
            else if (claimResponseUntrusted != null)
            {
                user = new OpenIdUser(claimResponseUntrusted, response.ClaimedIdentifier);
            }

            return user;
        }

        public HttpCookie CreateFormsAuthenticationCookie(OpenIdUser user)
        {
            var ticket = new FormsAuthenticationTicket(1, user.Nickname, DateTime.Now, DateTime.Now.AddDays(7), true, user.ToString());
            var encrypted = FormsAuthentication.Encrypt(ticket).ToString();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

            return cookie;
        }
    }

    public static class Extensions
    {
        public static bool IsSuccessful(this IAuthenticationResponse response)
        {
            return response != null && response.Status == AuthenticationStatus.Authenticated;
        }
    }
}