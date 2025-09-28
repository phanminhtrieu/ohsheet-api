namespace CleanArchitecture.Core.Common.Constants
{
    public static class EmailSubjects
    {
        public const string WELCOME = "Welcome to Oh Sheet";
        public const string HAVE_NEW_FEEDBACK = "You have new feedback";
    }

    public static class EmailHtmlMessages
    {
        public const string WELCOME = "Welcome {0} to Oh Sheet, I hope you happy all the time and have a nice day!!"; // TODO: Changes this to html template, can stored in database
    }
}
