using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cortside.WebApiStarter.WebApi.Middleware {
    /// <summary>
    /// Middleware to handle impersonation
    /// </summary>
    public class ImpersonationMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ImpersonationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) {
            _next = next;
            logger = loggerFactory.CreateLogger<ImpersonationMiddleware>();
        }

        /// <summary>
        /// Pipeline method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context) {
            if (context.Request.Headers.ContainsKey("X-SubjectId")) {
                var impersonateSubjectId = context.Request.Headers["X-SubjectId"].First();
                var callerUser = context.User;
                var callerSubjectId = callerUser.Claims.First(x => x.Type == "sub").Value;
                logger.LogInformation($"Request from caller {callerSubjectId} contains impersonation header for subject {impersonateSubjectId}");
                var canImpersonate = callerUser.Claims.Any(x => x.Type == "impersonate");
                if (!canImpersonate) {
                    logger.LogError($"Caller {callerSubjectId} does not have impersonate claim");

                    // TODO: need to return here
                    //return HttpStatusCode.Forbidden;
                }

                // we are doing delegation

                // TODO: validate that the requested impersonation user exists

                // https://datatracker.ietf.org/doc/html/rfc8693

                // https://github.com/IdentityModel/IdentityModel/blob/main/src/JwtClaimTypes.cs#L138
                // act claim
                // may_act

                ///// <summary>The "act" (actor) claim provides a means within a JWT to express that delegation has occurred and identify the acting party to whom authority has been delegated.The "act" claim value is a JSON object and members in the JSON object are claims that identify the actor. The claims that make up the "act" claim identify and possibly provide additional information about the actor.</summary>
                //public const string Actor = "act";

                ///// <summary>The "may_act" claim makes a statement that one party is authorized to become the actor and act on behalf of another party. The claim value is a JSON object and members in the JSON object are claims that identify the party that is asserted as being eligible to act for the party identified by the JWT containing the claim.</summary>
                //public const string MayAct = "may_act";

                //"may_act":
                //{
                //    "sub":"admin@example.com"
                //}

                var claims = new List<Claim>();
                claims.Add(new Claim("sub", impersonateSubjectId));
                //claims.Add(new Claim("iss", "https://identityserver.k8s.enerbank.com"));
                //claims.Add(new Claim("aud", "fundingmanagement.api"));
                //claims.Add(new Claim("role", "admin"));
                //claims.Add(new Claim("scope", "fundingmangement.api"));
                ClaimsPrincipal impersonateUser = new GenericPrincipal(new ClaimsIdentity(claims, "AuthenticationTypes.Federation"), null);

                //Subject newSubject = await SetImpersonationUser(Request, passedUser);
                //var roles = ((ClaimsIdentity)passedUser.Identity).FindAll(ClaimTypes.Role).ToList();
                //var rolesString = roles.OfType<string>();
                //var rolesArray = rolesString.ToArray();
                //var claims = new List<Claim>();
                //claims.Add(new Claim("sub", newSubject.SubjectId));
                //claims.Add(new Claim("iss", "https://identityserver.k8s.enerbank.com"));
                //claims.Add(new Claim("aud", "fundingmanagement.api"));
                //claims.Add(new Claim("role", "admin"));
                //claims.Add(new Claim("scope", "fundingmangement.api"));
                //ClaimsPrincipal user = new GenericPrincipal(new ClaimsIdentity(claims, "AuthenticationTypes.Federation"), rolesArray);
                context.User = impersonateUser;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
