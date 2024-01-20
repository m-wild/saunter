﻿using System.Collections.Generic;

namespace AsyncApi.Net.Generator;

/// <summary>
/// Defines a security scheme that can be used by the operations.
/// </summary>
public class SecuritySchemeObject
{
    /// <summary>
    /// Gets or sets the type of the security scheme.
    /// Valid values are "userPassword", "apiKey", "X509", "symmetricEncryption",
    /// "asymmetricEncryption", "httpApiKey", "http", "oauth2", "openIdConnect",
    /// "plain", "scramSha256", "scramSha512", and "gssapi".
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The type of the security scheme.
    /// </remarks>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets a short description for the security scheme.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the header, query, or cookie parameter to be used.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The name of the header, query, or cookie parameter to be used.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the location of the API key.
    /// Valid values are "user" and "password" for "apiKey",
    /// and "query", "header", or "cookie" for "httpApiKey".
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The location of the API key.
    /// </remarks>
    public string In { get; set; }

    /// <summary>
    /// Gets or sets the name of the HTTP Authorization scheme to be used.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The name of the HTTP Authorization scheme to be used
    /// in the Authorization header as defined in RFC7235.
    /// </remarks>
    public string Scheme { get; set; }

    /// <summary>
    /// Gets or sets a hint to the client to identify how the bearer token is formatted.
    /// Bearer tokens are usually generated by an authorization server, so this information is primarily for documentation purposes.
    /// </summary>
    public string BearerFormat { get; set; }

    /// <summary>
    /// Gets or sets an object containing configuration information for the flow types supported.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** An object containing configuration information for the flow types supported.
    /// </remarks>
    public OAuthFlowsObject Flows { get; set; }

    /// <summary>
    /// Gets or sets OpenId Connect URL to discover OAuth2 configuration values.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** OpenId Connect URL to discover OAuth2 configuration values.
    /// This MUST be in the form of an absolute URL.
    /// </remarks>
    public string OpenIdConnectUrl { get; set; }

    /// <summary>
    /// Gets or sets a list of the needed scope names.
    /// An empty array means no scopes are needed.
    /// </summary>
    public List<string> Scopes { get; set; }
}

