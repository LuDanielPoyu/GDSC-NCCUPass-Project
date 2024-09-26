using System;
using NCCUPass.Database.Entities;

namespace NCCUPass.Application.Features.Report.Commands.ReportTask
{
	public class ReportTaskReq : IRequest<ResponseData<ReportTaskRes>>
    {

        [Required]
        public string TaskId { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public List<TaskViolationTypesEnum> ViolationTypes { get; set; } = null!;

    }
}

