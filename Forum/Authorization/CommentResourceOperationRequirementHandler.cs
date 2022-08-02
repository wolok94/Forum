using Forum.Entities;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Forum.Authorization
{
    public class CommentResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Comment>
    {
        private readonly IUserContextService userService;

        public CommentResourceOperationRequirementHandler(IUserContextService userService)
        {
            this.userService = userService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Comment comment)
        {
            if (requirement.resourceOperation == ResourceOperation.Read ||
                requirement.resourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            User user = (User)context.User.Identity;
            string role = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            if (comment.User == user || role == "Admin")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

    }
}
