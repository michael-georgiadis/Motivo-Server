using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motivo.ApiModels;
using Motivo.ApiModels.Goals.Outbound;
using Motivo.Authentication;
using Motivo.Data;
using Motivo.Identity;

namespace Motivo.Controllers
{
    /// <summary>
    ///     The controller handling the goals of the user. This includes
    ///     Getting, Creation, Deletion, Modification,
    /// </summary>
    [Route(GoalRoutes.Controller)]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        protected MotivoDbContext MotivoDbContext;

        protected UserManager<MotivoUser> UserManager;

        public GoalsController(MotivoDbContext motivoDbContext,
            UserManager<MotivoUser> userManager)
        {
            MotivoDbContext = motivoDbContext;
            UserManager = userManager;
        }


        [HttpGet]
        [AuthorizeToken]
        [Route(GoalRoutes.GetGoals)]
        public async Task<ApiResponse<GeneralGoal>> GetGoalsAsync()
        {
            // Gets User
            var username = HttpContext.User?.Identity?.Name;
            var user = await UserManager.FindByNameAsync(username);

            var userGoals = await MotivoDbContext.Goals
                .Where(goal => goal.User == user)
                .Select(goal => new GeneralGoal
                {
                    GoalId = goal.Id,
                    NumericCurrent = goal.NumericCurrent,
                    NumericGoal = goal.NumericGoal,
                    Title = goal.Title
                }).ToListAsync();

            return new ApiResponse<GeneralGoal>
            {
                Response = userGoals
            };
        }


        [HttpPost]
        [AuthorizeToken]
        [Route(GoalRoutes.SetGoal)]
        public async Task<ApiResponse<GeneralGoal>> SetNewGoalAsync([FromBody] GeneralGoal goal)
        {
            // Get User
            var user = await UserManager.GetUserFromHttpContext(HttpContext);


            // This tries to create a new goal model to the database. If it fails
            try
            {
                var newGoal = new GoalDataModel
                {
                    User = user,
                    Title = goal.Title,
                    NumericCurrent = goal.NumericCurrent,
                    NumericGoal = goal.NumericGoal,
                };

                MotivoDbContext.Goals.Add(newGoal);
                await MotivoDbContext.SaveChangesAsync();

                goal.GoalId = newGoal.Id;

                return new ApiResponse<GeneralGoal>
                {
                    Response = new List<GeneralGoal>() { goal }
                };
            }
            // If it fails it will create this message.
            // TODO: Implement a better exception handling.
            catch (DbUpdateException ex)
            {
                return new ApiResponse<GeneralGoal>
                {
                    ErrorMessage = ex.InnerException?.Message
                };
            }
        }


        [HttpDelete]
        [AuthorizeToken]
        [Route(GoalRoutes.DeleteGoal)]
        public async Task<ApiResponse> DeleteGoalAsync(int goalId)
        {
            // Find the goal from the goal Id and then tries to delete it.
            try
            {
                var goalToBeDeleted = await MotivoDbContext.Goals.FindAsync(goalId);
                MotivoDbContext.Goals.Remove(goalToBeDeleted);
                await MotivoDbContext.SaveChangesAsync();
            }
            // If it fails it will create this message.
            // TODO: Implement a better exception handling.
            catch (DbUpdateException ex)
            {
                return new ApiResponse
                {
                    ErrorMessage = ex.InnerException?.Message
                };
            }

            return new ApiResponse
            {
                ErrorMessage = null
            };
        }


        [HttpPut]
        [AuthorizeToken]
        [Route(GoalRoutes.UpdateGoal)]
        public async Task<ApiResponse> UpdateGoalAsync([FromBody] GeneralGoal goal)
        {
            try
            {
                var user = await UserManager.GetUserFromHttpContext(HttpContext);
                var goalToBeUpdated = await MotivoDbContext.Goals.FirstOrDefaultAsync(dbGoal => dbGoal.Id == goal.GoalId);
                goalToBeUpdated.NumericCurrent = goal.NumericCurrent;
                goalToBeUpdated.NumericGoal = goal.NumericGoal;
                goalToBeUpdated.Title = goal.Title;

                MotivoDbContext.Goals.Update(goalToBeUpdated);
                await MotivoDbContext.SaveChangesAsync();

                return new ApiResponse
                {
                    // TODO: Make a helper function that converts a GeneralGoal object to GoalDataObject
                    ErrorMessage = null
                };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse
                {
                    ErrorMessage = ex.InnerException?.Message
                };
            }
        }
    }
}