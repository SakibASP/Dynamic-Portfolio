namespace Portfolio.Utils
{
    public class Constant
    {
        //Sessions
        public const string myProfile = "myProfile";
        public const string myProfileCover = "myProfileCover";
        public const string myContact = "myContact";
        public const string myEducation = "myEducation";
        public const string myExperience = "myExperience";
        public const string mySkill = "mySkill";
        public const string myProject = "myProject";
        public const string portfolionSession = "_SakibPortfolioSession";

        //constant values
        public const string bangladeshTimezone = "Bangladesh Standard Time";

        //TempMessages
        public const string Success = "Success";
        public const string SuccessMessage = "Successfully done!";
        public const string SuccessRmvMsg = "Successfully removed!";
        public const string Error = "Error";
        public const string ErrorMessage = "Failed! Something has gone wrong!";

        //stored procedures
        public const string udspGetVisitors = "EXEC dbo.udspGetVisitors @PageNumber = @PageNumber,@PageSize=@PageSize,@StartDate=@StartDate,@EndDate = @EndDate,@SearchString = @SearchString";

    }
}
