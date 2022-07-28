using Forum.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Forum.Authorization
{
    public class TopicResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Topic>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Topic topic)
        {
            if (requirement.resourceOperation == ResourceOperation.Read ||
                requirement.resourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            int userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string role = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            if (topic.UserId == userId || role == "Admin")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

    }
}
