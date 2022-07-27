using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Forum.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Update,
        Delete,
        Read,
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation resourceOperation { get; set; }

        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            this.resourceOperation = resourceOperation;
        }
    }
}
