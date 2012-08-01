using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Mvc4.OpenId.Sample.Models
{
    public class OpenIdIdentity : IIdentity
    {
        private readonly OpenIdUser _user;

        public OpenIdIdentity(OpenIdUser user)
        {
            _user = user;
        }

        public OpenIdUser OpenIdUser
        {
            get
            {
                return _user;
            }
        }

        public string AuthenticationType
        {
            get
            {
                return "OpenID Identity";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return _user.Nickname ?? string.Empty;
            }
        }
    }
}