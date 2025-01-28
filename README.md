# SAML Client

## CFS Configuration
Create a Generic SAML application in CFS:

- add `https://localhost:44305/` as an `Audience`;
- set `https://localhost:44305/Auth/AssertionConsumerService` as the `Recipient`

## App Configuration (with an IDE)
Open your web application in Visual Studio and open the file `appsettings.json` at the root of the web application. Locate the `auth:saml` section.

- replace the value for `Tenant` with your _Tenant name_ from CFS;
- replace the value for `ApplicationID` with your _Application ID_ from CFS;
- replace the value for `IdPMetadata` with the _Metadata URL_ from CFS. (`https://localhost:44303/Trust/FederationMetadata?tenant={tenant}&application={appid}`).

![CFS OAuth application](SAML%20MVC/Docs/Resources/Images/cfs-saml-application.png)

![CFS OAuth Authority.png](SAML%20MVC/Docs/Resources/Images/cfs-saml-metadata.png)

## App Configuration (already compiled)
Go to the folder where the application source is located and go to the `SAML MVC` -> `Published` folder. Open the file `appsettings.json` with a text editor (_Notepad_). Locate the `auth:saml` section.

- replace the value for `Tenant` with your _Tenant name_ from CFS;
- replace the value for `ApplicationID` with your _Application ID_ from CFS;
- replace the value for `IdPMetadata` with the _Metadata URL_ from CFS. (`https://localhost:44303/Trust/FederationMetadata?tenant={tenant}&application={appid}`).

Run the `SAML MVC.exe` executable.

![CFS OAuth Authority.png](SAML%20MVC/Docs/Resources/Images/console-running.png)

## Preview

Sample SAML client application using .NET 8 & [ITFoxtec Identity SAML2](https://github.com/ITfoxtec/ITfoxtec.Identity.Saml2).

The homepage contains a link to the Dashboard labelled "Login" which will redirect to the OP for authorization first, since we are not logged in yet.

![Homepage](SAML%20MVC/Docs/Resources/Images/homepage.png)

After logging in to the OP and authorizing the application, we will be redirected back to the [Dashboard](Views/Home/Dashboard.cshtml), where we can see the user's claims.

![CFS authorization login prompt](SAML%20MVC/Docs/Resources/Images/cfs-authorization-login-prompt.png)

![Dashboard page](SAML%20MVC/Docs/Resources/Images/dashboard.png)

From the Dashboard we can Logout, which will end the session here at the RP, and then will redirect to the OP's `logout`
endpoint.
