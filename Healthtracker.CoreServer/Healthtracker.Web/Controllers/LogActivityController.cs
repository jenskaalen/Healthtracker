using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Healthtracker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Healthtracker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogActivityController : ControllerBase
    {
        private readonly IActivitySuggestionsService activitySuggestionsService;
        private string UserId => User.Identity.Name;

        public LogActivityController(IActivitySuggestionsService activitySuggestionsService)
        {
            this.activitySuggestionsService = activitySuggestionsService;
        }

        [Route("suggestions")]
        public List<string> Suggestions()
        {
            int maxSuggestionCount = 10;
            return activitySuggestionsService.GetSuggestions(UserId, maxSuggestionCount);
        }
    }
}