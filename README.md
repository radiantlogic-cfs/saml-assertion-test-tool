# SAML Client

## CFS Configuration
Create a Generic SAML application in CFS:

- add `https://myapp:44305/` as an `Audience`;
- set `https://myapp:44305/Auth/AssertionConsumerService` as the `Recipient`

## App Configuration (with an IDE)
Open your web application in Visual Studio and open the file `appsettings.json` at the root of the web application. Locate the `auth:saml` section.

- replace the value for `Tenant` with your _Tenant name_ from CFS;
- replace the value for `ApplicationID` with your _Application ID_ from CFS;
- replace the value for `IdPMetadata` with the _Metadata URL_ from CFS. (`https://cfs-master:44303/Trust/FederationMetadata?tenant={tenant}&application={appid}`);
- replace the value for `Issuer` with the _Application URL_ (`https://myapp:44305/`);
- replace the value for `AssertionConsumerServiceUrl` with the _Application Consumer Service URL_ (`https://myapp:44305/Saml2/Auth/AssertionConsumerService`).

![CFS OAuth application](SAML%20MVC/Docs/Resources/Images/cfs-saml-application.png)

![CFS OAuth Authority.png](SAML%20MVC/Docs/Resources/Images/cfs-saml-metadata.png)

## App Configuration (IIS deployment)
Go to the folder where the application source is located and go to the `SAML MVC` -> `Published` folder. Open the file `appsettings.json` with a text editor (_Notepad_). Locate the `auth:saml` section.

- replace the value for `Tenant` with your _Tenant name_ from CFS;
- replace the value for `ApplicationID` with your _Application ID_ from CFS;
- replace the value for `IdPMetadata` with the _Metadata URL_ from CFS. (`https://cfs-master:44303/Trust/FederationMetadata?tenant={tenant}&application={appid}`);
- replace the value for `Issuer` with the _Application URL_ (`https://myapp:44305/`);
- replace the value for `AssertionConsumerServiceUrl` with the _Application Consumer Service URL_ (`https://myapp:44305/Saml2/Auth/AssertionConsumerService`).

Open Internet Information Services (IIS) Manager. Right click on _Sites_ and select _Add Website_.

![IIS Sites](SAML%20MVC/Docs/Resources/Images/iis-sites.png)

Choose a site name then select the path in which the application is located. Choose a port and press _OK_. Now you can browse the website. If you encounter any errors, look into [Potential errors and fixes](#errors).

![IIS Add Website](SAML%20MVC/Docs/Resources/Images/iis-add-website.png)

## Preview

Sample SAML client application using .NET 8 & [ITFoxtec Identity SAML2](https://github.com/ITfoxtec/ITfoxtec.Identity.Saml2).

The homepage contains a link to the Dashboard labelled "Login" which will redirect to the OP for authorization first, since we are not logged in yet.

![Homepage](SAML%20MVC/Docs/Resources/Images/homepage.png)

After logging in to the OP and authorizing the application, we will be redirected back to the [Dashboard](Views/Home/Dashboard.cshtml), where we can see the user's claims.

![CFS authorization login prompt](SAML%20MVC/Docs/Resources/Images/cfs-authorization-login-prompt.png)

![Dashboard page](SAML%20MVC/Docs/Resources/Images/dashboard.png)

From the Dashboard we can Logout, which will end the session here at the RP, and then will redirect to the OP's `logout`
endpoint.

## Potential errors and fixes <a id='errors'></a>

- **HTTP Error 500.19: Invalid configuration data**

![Invalid Configuration Error](SAML%20MVC/Docs/Resources/Images/error1.png)

1. Give IIS permissions to application folder
    - Navigate to the application folder
    - Right click to the application folder
    - Select _Properties_
    - Go to the _Security_ tab
    - Verify the user `IIS_IUSRS` has the following permissions: Read & execute, List folder contents, Read
    - If the `IIS_IUSRS` user does not have the permissions, select it and click _Edit_, check the permissions and click _OK_
    - If the `IIS_IUSRS` user does not exist, create it by clicking _Edit_ then _Add..._ and entering `IIS_IUSRS` into the field, then pressing OK; make sure you check the required permissions

![IIS User Permissions](SAML%20MVC/Docs/Resources/Images/iis-user-permissions.png)

![IIS Add User](SAML%20MVC/Docs/Resources/Images/iis-add-user.png)

2. Install IIS Hosting Bundle for .NET 8
   - Navigate to [.NET 8 Download Page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
   - Download and install the _Hosting Bundle_
   - Restart IIS or the computer

![IIS Hosting Bundle](SAML%20MVC/Docs/Resources/Images/iis-hosting-bundle.png)
