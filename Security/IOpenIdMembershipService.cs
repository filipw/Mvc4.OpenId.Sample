using DotNetOpenAuth.OpenId.RelyingParty;
using Mvc4.OpenId.Sample.Models;

namespace Mvc4.OpenId.Sample.Security
{
    internal interface IOpenIdMembershipService
    {
        IAuthenticationRequest ValidateAtOpenIdProvider(string openIdIdentifier);
        OpenIdUser GetUser();
    }
}