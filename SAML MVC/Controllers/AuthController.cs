using System.Security.Authentication;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAML_MVC.Identity;

namespace SAML_MVC.Controllers;

[AllowAnonymous]
[Route("Auth")]
public class AuthController : Controller
{
    private IConfiguration Configuration { get; }
    private const string RelayStateReturnUrl = "ReturnUrl";
    private readonly Saml2Configuration config;

    public AuthController(Saml2Configuration config, IConfiguration configuration)
    {
        this.config = config;
        Configuration = configuration;
    }

    [Route("Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var binding = new Saml2RedirectBinding();
        binding.SetRelayStateQuery(new Dictionary<string, string>
            { { RelayStateReturnUrl, returnUrl ?? Url.Content("~/") } });

        return binding.Bind(new Saml2AuthnRequest(config)
        {
            AssertionConsumerServiceUrl = new Uri(Configuration["auth:saml2:AssertionConsumerServiceUrl"] ?? string.Empty),
            Issuer = Configuration["auth:saml2:Issuer"],
            Subject = new Subject { NameID = new NameID { ID = "abcd" } },
            NameIdPolicy = new NameIdPolicy { AllowCreate = true, Format = Configuration["auth:saml2:NameIdFormat"] },
            RequestedAuthnContext = new RequestedAuthnContext
            {
                Comparison = AuthnContextComparisonTypes.Minimum,
                AuthnContextClassRef = [AuthnContextClassTypes.PasswordProtectedTransport.OriginalString],
            },
            Scoping = new Scoping
            {
                IDPList = new IDPList
                {
                    IDPEntry =
                    [
                        new IDPEntry
                        {
                            ProviderID = "https://qaz.org",
                            Name = "xxx",
                            Loc = "https://wsx.org"
                        }
                    ],
                    GetComplete = "xxx"
                },
                RequesterID = ["https://xyz.org"]
            }
        }).ToActionResult();
    }

    [Route("AssertionConsumerService")]
    public async Task<IActionResult> AssertionConsumerService()
    {
        var httpRequest = Request.ToGenericHttpRequest(validate: true);
        var saml2AuthnResponse = new Saml2AuthnResponse(config);

        httpRequest.Binding.ReadSamlResponse(httpRequest, saml2AuthnResponse);
        if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
        {
            throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
        }

        httpRequest.Binding.Unbind(httpRequest, saml2AuthnResponse);
        await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal));

        var relayStateQuery = httpRequest.Binding.GetRelayStateQuery();
        var returnUrl = relayStateQuery.ContainsKey(RelayStateReturnUrl) ? relayStateQuery[RelayStateReturnUrl] : Url.Content("~/");
        return Redirect(returnUrl);
    }
}