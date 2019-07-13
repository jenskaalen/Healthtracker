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
    public class SupplementController : ControllerBase
    {
        private readonly ISupplementSuggestionsService supplementSuggestionsService;
        private string UserId => User.Identity.Name;

        public SupplementController(ISupplementSuggestionsService supplementSuggestionsService)
        {
            this.supplementSuggestionsService = supplementSuggestionsService;
        }

        [Route("suggestions")]
        public List<string> Suggestions()
        {
            int maxSuggestionCount = 10;
            return supplementSuggestionsService.GetSuggestions(UserId, maxSuggestionCount);
        }
    }
}