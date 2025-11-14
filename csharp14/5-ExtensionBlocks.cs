using System.Security.Claims;

internal class ExtensionBlocks
{
    public static void Execute()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "Admin")]));
        Console.WriteLine($"Is Admin: {user.IsAdmin}");
        Console.WriteLine("Is in Role 'User': " + user.IsInRole("User"));
    }
}

public static class UserExtension
{
    // Extension block
    extension(ClaimsPrincipal user) // extension members for IEnumerable<TSource>
    {
        // Extension property:
        public bool IsAdmin => user.HasClaim(ClaimTypes.Role, "Admin");

        // Extension method:
        public bool IsInRole(string role) => user.HasClaim(ClaimTypes.Role, role);
        public bool IsInRole2(string role) => user.HasClaim(ClaimTypes.Role, role);
        public bool IsInRole3(string role) => user.HasClaim(ClaimTypes.Role, role);
        public bool IsInRole4(string role) => user.HasClaim(ClaimTypes.Role, role);
    }

    // Extension block with generic type parameter
    extension<TUser>(TUser user) where TUser : ClaimsPrincipal
    {
        public bool IsInRole(string role) => user.HasClaim(ClaimTypes.Role, role);
    }
}
