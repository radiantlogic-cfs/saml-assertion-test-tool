using System.Security.Claims;

namespace SAML_MVC.Identity;

public static class ClaimsTransform
{
    public static ClaimsPrincipal Transform(ClaimsPrincipal incomingPrincipal)
    {
        if (!incomingPrincipal.Identity.IsAuthenticated)
        {
            return incomingPrincipal;
        }

        return CreateClaimsPrincipal(incomingPrincipal);
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(ClaimsPrincipal incomingPrincipal)
    {
        var claims = new List<Claim>();
        claims.AddRange(incomingPrincipal.Claims);

        return new ClaimsPrincipal(new ClaimsIdentity(claims, incomingPrincipal.Identity.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role)
        {
            BootstrapContext = ((ClaimsIdentity)incomingPrincipal.Identity).BootstrapContext
        });
    }
}