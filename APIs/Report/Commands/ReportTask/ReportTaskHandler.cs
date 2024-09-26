using System;
using System.Xml.Linq;
using NCCUPass.Application.Features.Manager.Commands.AddTaskViolationTypes;
using NCCUPass.Application.Features.Notification.Commands.AddTaskNotificationToASpecificUser;
using NCCUPass.Application.Features.TakeAwayService.Commands.AddItemToBag;
using NCCUPass.Database.Entities;

namespace NCCUPass.Application.Features.Report.Commands.ReportTask
{
	public class ReportTaskHandler : BaseHandler, IRequestHandler<ReportTaskReq, ResponseData<ReportTaskRes>>
    {
		public ReportTaskHandler(IServiceProvider provider) : base(provider)
        {
		}


        public async Task<ResponseData<ReportTaskRes>> Handle(ReportTaskReq request, CancellationToken cancellationToken)
        {
            //init
            var userId = new ObjectId(UserContext.UserId);
            var taskId = new ObjectId(request.TaskId);
            var description = request.Description;
            var violationTypes = request.ViolationTypes;

            //check
            // 檢查是否類別都有在TaskViolationTypes內
            foreach (var violationType in violationTypes)
            {
                var taskViolationType = await UnitOfWork.TaskViolationTypes.FindOneAsync(_ => _.Name == violationType.ToString());
                if (taskViolationType == null)
                {
                    return new ResponseData<ReportTaskRes>(404, false, $"{violationType}檢舉類別不存在！", null);
                }
                
            }

            // operation 
            var ReportTask = new TaskViolationReports()
            {
                FromId = userId,
                TaskId = taskId,
                Description = description,
            };

            var taskViolationTypeRecords = new List<TaskViolationTypeRecords>();
            foreach (var violationType in violationTypes)
            {
                var taskViolationType = await UnitOfWork.TaskViolationTypes.FindOneAsync(_ => _.Name == violationType.ToString());

                var taskViolationTypeRecord = new TaskViolationTypeRecords()
                {
                    ViolationId = ReportTask.Id,
                    TypeId = taskViolationType.Id
                };
                taskViolationTypeRecords.Add(taskViolationTypeRecord);
            }
            UnitOfWork.StartTransaction();
            await UnitOfWork.TaskViolationReports.InsertOneAsync(ReportTask);
            await UnitOfWork.TaskViolationTypeRecords.InsertManyAsync(taskViolationTypeRecords);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<ReportTaskRes>(200, true, "成功遞送檢舉申請！", null);
        }
    }
}

