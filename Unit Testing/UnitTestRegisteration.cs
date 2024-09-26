using NCCUPass.Infrastructure.UnitOfWorks;
using NCCUPass.Application.Features.User;
using NCCUPass.Share.Attributes;
using NCCUPass.Share.Extensions;
using NCCUPass.Tests.Fixtures;
using Xunit;
using NCCUPass.Application.Features.User.Commands.Register;
using NCCUPass.Database.Entities;
using MongoDB.Bson;

namespace NCCUPass.Tests
{
    [Collection("BaseTestCollection")]
    public class UnitTestRegistration
    {

        private readonly IServiceProvider serviceProvider;
        [Inject] protected readonly IUnitOfWork UnitOfWork = null!;

        public UnitTestRegistration(BaseTestFixture fixture)
        {
            serviceProvider = fixture.ServiceProvider;
            serviceProvider.Inject(this);
        }

        [Fact]
        public async Task User_RegistrationReq_Returnstatusokay()
        {
            // Arrange:
            // Create a registration request
            var registrationRequest = new RegisterReq
            {
                NccuEmail = "110306011@nccu.edu.tw",
                Password = "20030316Daniel",
                AccountName = "lupoyu",
                RealName = new RealName
                {
                    FirstName = "Daniel",
                    LastName = "Lu"
                },
                Avatar = "https://nccupass.com/files/sys-files/images/def-ava-org.PNG",
                Birthday = new Birthday
                {
                    Date = new DateTime(2003, 03, 16), 
                    Show = true
                },
                Gender = GenderEnum.Male, 
                College = "NCCU",
                Department = "資訊管理系",
                Phone = "0987878787"
            };

            // Create an existing user (for testing)
            var existingUser = new Users
            {
                AccountName = "Dua",
                RealName = new RealName
                {
                    FirstName = "Dua",
                    LastName = "Lipa"
                },
                Avatar = "https://nccupass.com/files/sys-files/images/def-ava-org.PNG",
                Description = "A singer, songwriter and superstar.",
                RatingTakenTask = 0,
                RatingPublishTask = 0,
                Birthday = new Birthday
                {
                    Date = DateTime.Parse("1990-01-15"),
                    Show = true
                },
                Gender = new Gender
                {
                    Type = GenderEnum.Female,
                    Show = true
                },
                DepartmentId = ObjectId.GenerateNewId(), 
                Phone = "0987654321",
                IsActive = true
            };

            // create existing user's credential
            var existingUserCredential = new UserCreadentials
            {
                NccuEmail = "110306087@nccu.edu.tw", 
                PersonalEmail = "dualipa@gmail.com",
                Password = "$2a$12$.//zywfUlE.D00Zvnnf6w.p2DAIT8GCQqA2mXAkVZrA86DDM0Dc7y", 
                Salt = "$2a$12$.//zywfUlE.D00Zvnnf6w.", 
                VerifyToken = "03ac06b6-73aa-4fe0-910d-eb98e45fb582", 
                UserId = ObjectId.Parse("64ccc72e334bc5695240fd30"), 
                RefreshToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9",
                DeviceToken = "ccvSuo1bSDm-NZxBKuKsyy:APA91bF-UG4jiKxBp97DzjsO7GOFVGLuX6h2ouaqdmgqlXBE9fi8mzju2LSnYNTooSoiYpKo_tcMjbxwnH6o3zFYbg6iZ3MuRbiZvdVHzehK1ezZKEWilV52_IyQI5Fv0jNG-zmfx8_t"
            };

            await UnitOfWork.Users.InsertOneAsync(existingUser);
            await UnitOfWork.UserCredentials.InsertOneAsync(existingUserCredential);

            // Create existing department
            var existingDepartment = new Departments
            {
                College = "商學院",
                Department = "資訊管理系",
            };

            await UnitOfWork.Departments.InsertOneAsync(existingDepartment);

            // Instantiate RegisterHandler
            var registerHandler = new RegisterHandler(serviceProvider);

            // Act: Perform registration
            var registrationResult = await registerHandler.Handle(registrationRequest, CancellationToken.None);

            // Assert: Verify the registration result
            Assert.True(registrationResult.IsSuccess);
            Assert.Equal(200, registrationResult.Code);
            Assert.Equal("Register successfully! Please check out your email to verify your account!", registrationResult.Message);

            // Optionally, check the database or data store for the saved user information
            // Example: Retrieve the user from the database using UnitOfWork and assert its properties
            var user = await UnitOfWork.Users.FindOneAsync(u => u.AccountName == "TestUser");
            Assert.NotNull(user);
        }

        
    }
}

