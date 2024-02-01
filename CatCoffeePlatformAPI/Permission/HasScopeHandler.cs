﻿using Microsoft.AspNetCore.Authorization;

namespace CatCoffeePlatformAPI.Permission
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private string? _scope;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var requiredScope = requirement.Scope;

            if (_scope != null)
            {
                if (_scope.Contains(requiredScope))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                else
                {
                    return Task.CompletedTask;
                }
            }

            var scopes = context.User.Claims.Where(claim => claim.Type == "scope" && claim.Issuer == requirement.Issuer);

            if (scopes == null)
                return Task.CompletedTask;

            foreach (var scope in scopes)
            {
                if (scope.Value.Equals(requiredScope))
                {
                    _scope = scope.Value;
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
