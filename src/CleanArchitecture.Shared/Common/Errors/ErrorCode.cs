namespace CleanArchitecture.Shared.Common.Errors
{
    public enum ErrorCode
    {
        Internal = 0,
        VersionConflict = 1, // NuGet package versions different
        NotFound = 2,
        BadRequest = 3,
        Conflict = 4,
        Other = 5,
        Unauthorized = 6,
        ItemAlreadyExists = 7,
        UnprocessableEntity = 8
    }
}
