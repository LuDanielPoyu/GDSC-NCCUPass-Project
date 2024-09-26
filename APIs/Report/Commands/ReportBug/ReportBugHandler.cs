using System;
using System.Threading.Tasks;
using NCCUPass.Application.Features.Chat.Commands.AddTaskChatRoom;
using NCCUPass.Application.Features.RestaurantManage.Commands.EditProduct;
using NCCUPass.Application.Features.Task.NormalTask.Commands.AcceptCandidate;
using NCCUPass.Application.Features.Task.NormalTask.Commands.ApplyNormalTask;
using NCCUPass.Database.Entities;
using NCCUPass.Database.Enums;

namespace NCCUPass.Application.Features.Report.Commands.ReportBug
{
	public class ReportBugHandler : BaseHandler, IRequestHandler<ReportBugReq,ResponseData<ReportBugRes>>
	{
		public ReportBugHandler(IServiceProvider provider) : base(provider)
		{
		}

        public async Task<ResponseData<ReportBugRes>> Handle(ReportBugReq request, CancellationToken cancellationToken)
        {
            //init
            var fromType = UserContext.Role;
            var userId = new ObjectId(UserContext.UserId);

            var bugTitle = request.BugTitle;
            var bugDescription = request.BugDescription;
            var bugImages = request.BugImages;

            //check & operation
            var BugReport = new BugReports()
            {
                FromType = fromType,
                FromId = userId,
                BugTitle = bugTitle,
                BugDescription = bugDescription,
            };
           
            UnitOfWork.StartTransaction();
            await UnitOfWork.BugReports.InsertBugReportAsync(BugReport, bugImages);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseData<ReportBugRes>(200, true, "成功回報錯誤!非常感謝您的回饋！", null);

        }
    }
}

