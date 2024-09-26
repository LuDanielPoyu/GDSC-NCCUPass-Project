using System;
using Microsoft.AspNetCore.Http;

namespace NCCUPass.Application.Features.Report.Commands.ReportBug
{
	public class ReportBugReq : IRequest<ResponseData<ReportBugRes>>
	{
        [Required]
        public required string BugTitle { get; set; }

        [Required]
        public required string BugDescription { get; set; }

        [Required]
        public List<IFormFile>? BugImages { get; set; }
    }
}

