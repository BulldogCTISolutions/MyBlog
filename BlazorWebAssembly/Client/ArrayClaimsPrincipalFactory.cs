using System.Security.Claims;
using System.Text.Json;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace BlazorWebAssembly.Client;

public class ArrayClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount> where TAccount : RemoteUserAccount
{
    public ArrayClaimsPrincipalFactory( IAccessTokenProviderAccessor accessor )
    : base( accessor )
    { }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync( TAccount account, RemoteAuthenticationUserOptions options )
    {
        ClaimsPrincipal? user = await base.CreateUserAsync( account, options ).ConfigureAwait( false );
        ClaimsIdentity? claimsIdentity = user.Identity as ClaimsIdentity;

        if( account is not null )
        {
            foreach( KeyValuePair<string, object> kvp in account.AdditionalProperties )
            {
                string? key = kvp.Key;
                object? value = kvp.Value;
                if( (value is JsonElement element) && (element.ValueKind == JsonValueKind.Array) )
                {
                    claimsIdentity?.RemoveClaim( claimsIdentity.FindFirst( key ) );

                    IEnumerable<Claim>? claims = element.EnumerateArray()
                        .Select( x => new Claim( key, x.ToString() ) );

                    claimsIdentity?.AddClaims( claims );
                }
            }
        }

        return user;
    }
}
