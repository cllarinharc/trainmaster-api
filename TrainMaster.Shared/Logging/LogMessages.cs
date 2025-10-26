namespace TrainMaster.Shared.Logging
{
    public static class LogMessages
    {
        //User
        public static string InvalidUserInputs() => "Message: Invalid inputs to User.";
        public static string NullOrEmptyUserEmail() => "Message: The Email field is null, empty, or whitespace.";
        public static string UpdatingErrorUser(Exception ex) => $"Message: Error updating User: {ex.Message}";
        public static string UpdatingSuccessUser() => "Message: Successfully updated User.";
        public static string UserNotFound(string action) => $"Message: User not found for {action} action.";
        public static string AddingUserError(Exception ex) => $"Message: Error adding a new User: {ex.Message}";
        public static string AddingUserSuccess() => "Message: Successfully added a new User.";
        public static string DeleteUserError(Exception ex) => $"Message: Error to delete a User: {ex.Message}";
        public static string DeleteUserSuccess() => "Message: Delete with success User.";
        public static string GetAllUserError(Exception ex) => $"Message: Error to loading the list User: {ex.Message}";
        public static string GetAllUserSuccess() => "Message: GetAll with success User.";
        public static string UserNotFound() => "Message: User not found.";
        public static string PasswordInvalid() => "Message: Incorrect current password.";
        public static string UpdatingSuccessPassword() => "Message: Password updated successfully.";

        //Pessoal Profile
        public static string InvalidPessoalProfileInputs() => "Message: Invalid inputs to Pessoal Profile.";
        public static string UpdatingErrorPessoalProfile(Exception ex) => $"Message: Error updating Pessoal Profile: {ex.Message}";
        public static string UpdatingSuccessPessoalProfile() => "Message: Successfully updated Pessoal Profile.";
        public static string AddingPessoalProfileError(Exception ex) => $"Message: Error adding a new Pessoal Profile: {ex.Message}";
        public static string AddingPessoalProfileSuccess() => "Message: Successfully added a new Pessoal Profile.";
        public static string DeletePessoalProfileError(Exception ex) => $"Message: Error to delete a Pessoal Profile: {ex.Message}";
        public static string DeletePessoalProfileSuccess() => "Message: Delete with success Pessoal Profile.";
        public static string GetAllPessoalProfileError(Exception ex) => $"Message: Error to loading the list Pessoal Profile: {ex.Message}";
        public static string GetAllPessoalProfileSuccess() => "Message: GetAll with success Pessoal Profile.";
        public static string FullNameExists() => "Message: User already exists with that name.";
        public static string AgeBelow16() => "Message: User must be 16 years or older.";

        //Address
        public static string InvalidAddressInputs() => "Message: Invalid inputs to Address.";
        public static string AddingAddressSuccess() => "Message: Successfully added a new Address.";
        public static string AddingAddressError(Exception ex) => $"Message: Error adding a new Address: {ex.Message}";
        public static string DeleteAddressError(Exception ex) => $"Message: Error to delete a Address: {ex.Message}";
        public static string DeleteAddressSuccess() => "Message: Delete with success Address.";
        public static string GetAllAddressError(Exception ex) => $"Message: Error to loading the list Address: {ex.Message}";
        public static string GetAllAddressSuccess() => "Message: GetAll with success Address.";
        public static string UpdatingErrorAddress(Exception ex) => $"Message: Error updating Address: {ex.Message}";
        public static string UpdatingSuccessAddress() => "Message: Successfully updated Address.";

        //Notification
        public static string GetAllNotificationError(Exception ex) => $"Message: Error to loading the list Notification: {ex.Message}";
        public static string GetAllNotificationSuccess() => "Message: GetAll with success Notification.";

        //Education Level
        public static string InvalidEducationLevelInputs() => "Message: Invalid inputs to Education Level.";
        public static string AddingEducationLevelSuccess() => "Message: Successfully added a new Education Level.";
        public static string AddingEducationLevelError(Exception ex) => $"Message: Error adding a new Education Level: {ex.Message}";
        public static string DeleteEducationLevelError(Exception ex) => $"Message: Error to delete a Education Level: {ex.Message}";
        public static string DeleteEducationLevelSuccess() => "Message: Delete with success Education Level.";
        public static string GetAllEducationLevelError(Exception ex) => $"Message: Error to loading the list Education Level: {ex.Message}";
        public static string GetAllEducationLevelSuccess() => "Message: GetAll with success Education Level.";
        public static string UpdatingErrorEducationLevel(Exception ex) => $"Message: Error updating Education Level: {ex.Message}";
        public static string UpdatingSuccessEducationLevel() => "Message: Successfully updated Education Level.";

        //Professional Profile
        public static string InvalidProfessionalProfileInputs() => "Message: Invalid inputs to Professional Profile.";
        public static string AddingProfessionalProfileSuccess() => "Message: Successfully added a new Professional Profile.";
        public static string AddingProfessionalProfileError(Exception ex) => $"Message: Error adding a new Professional Profile: {ex.Message}";
        public static string DeleteProfessionalProfileError(Exception ex) => $"Message: Error to delete a Professional Profile: {ex.Message}";
        public static string DeleteProfessionalProfileSuccess() => "Message: Delete with success Professional Profile.";
        public static string GetAllProfessionalProfileError(Exception ex) => $"Message: Error to loading the list Professional Profile: {ex.Message}";
        public static string GetAllProfessionalProfileSuccess() => "Message: GetAll with success Professional Profile.";
        public static string UpdatingErrorProfessionalProfile(Exception ex) => $"Message: Error updating Professional Profile: {ex.Message}";
        public static string UpdatingSuccessProfessionalProfile() => "Message: Successfully updated Professional Profile.";

        //Course
        public static string InvalidCourseInputs() => "Message: Invalid inputs to Course.";
        public static string AddingCourseSuccess() => "Message: Successfully added a new Course.";
        public static string AddingCourseError(Exception ex) => $"Message: Error adding a new Course: {ex.Message}";
        public static string DeleteCourseError(Exception ex) => $"Message: Error to delete a Course: {ex.Message}";
        public static string DeleteCourseSuccess() => "Message: Delete with success Course.";
        public static string GetAllCourseError(Exception ex) => $"Message: Error to loading the list Course: {ex.Message}";
        public static string GetAllCourseSuccess() => "Message: GetAll with success Course.";
        public static string UpdatingErrorCourse(Exception ex) => $"Message: Error updating Course: {ex.Message}";
        public static string UpdatingSuccessCourse() => "Message: Successfully updated Course.";
        public static string InvalidDateRangeCourse() => "Message: End date cannot be earlier than start date.";

        //Deparment
        public static string InvalidDepartmentInputs() => "Message: Invalid inputs to Department.";
        public static string AddingDepartmentSuccess() => "Message: Successfully added a new Department.";
        public static string AddingDepartmentError(Exception ex) => $"Message: Error adding a new Department: {ex.Message}";
        public static string DeleteDepartmentError(Exception ex) => $"Message: Error to delete a Department: {ex.Message}";
        public static string DeleteDepartmentSuccess() => "Message: Delete with success Department.";
        public static string GetAllDepartmentError(Exception ex) => $"Message: Error to loading the list Department: {ex.Message}";
        public static string GetAllDepartmentSuccess() => "Message: GetAll with success Department.";
        public static string UpdatingErrorDepartment(Exception ex) => $"Message: Error updating Department: {ex.Message}";
        public static string UpdatingSuccessDepartment() => "Message: Successfully updated Department.";
        public static string NameExistsDepartment() => "Message: Department already exists with that name.";

        //Team
        public static string InvalidTeamInputs() => "Message: Invalid inputs to Team.";
        public static string AddingTeamSuccess() => "Message: Successfully added a new Team.";
        public static string AddingTeamError(Exception ex) => $"Message: Error adding a new Team: {ex.Message}";
        public static string DeleteTeamError(Exception ex) => $"Message: Error to delete a Team: {ex.Message}";
        public static string DeleteTeamSuccess() => "Message: Delete with success Team.";
        public static string GetAllTeamError(Exception ex) => $"Message: Error to loading the list Team: {ex.Message}";
        public static string GetAllTeamSuccess() => "Message: GetAll with success Team.";
        public static string UpdatingErrorTeam(Exception ex) => $"Message: Error updating Team: {ex.Message}";
        public static string UpdatingSuccessTeam() => "Message: Successfully updated Team.";
        public static string DuplicateName() => "Message: This name not possible, check other name to team.";

        //PasswordHistory
        public static string InvalidHistoryPasswordInputs() => "Message: Invalid inputs to Password History.";
        public static string AddingHistoryPasswordSuccess() => "Message: Successfully added a new Password History.";
        public static string AddingHistoryPasswordError(Exception ex) => $"Message: Error adding a new Password History: {ex.Message}";
        public static string DeleteHistoryPasswordError(Exception ex) => $"Message: Error to delete a Password History: {ex.Message}";
        public static string DeleteHistoryPasswordSuccess() => "Message: Delete with success Password History.";
        public static string GetAllHistoryPasswordError(Exception ex) => $"Message: Error to loading the list Password History: {ex.Message}";
        public static string GetAllHistoryPasswordSuccess() => "Message: GetAll with success Password History.";
        public static string UpdatingErrorHistoryPassword(Exception ex) => $"Message: Error updating Password History: {ex.Message}";
        public static string UpdatingSuccessHistoryPassword() => "Message: Successfully updated Password History.";
    }
}