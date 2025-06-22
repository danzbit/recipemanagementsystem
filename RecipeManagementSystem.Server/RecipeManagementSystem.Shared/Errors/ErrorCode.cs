namespace RecipeManagementSystem.Shared.Errors;

public enum ErrorCode
{
    None,
    EntityNotFound,
    InvalidId,
    UserAlreadyExist,
    UserNotFound,
    Unauthorized,
    UpdateFailed,
    AddFailed,
    UserCreation,
    PdfStatusNotFound,
    PdfIsNotCompletedYet,
    PdfFileNotExist,
    CollaborationInviteAlreadySent,
    CannotAcceptInvite,
    NotFoundHandler, //
    DeserializationFailed,
    InviteIsExpired,
    ItemAlreadyAddedToCache,
    ItemNotExistInoCache,
    JobNotFound,
    InvalidDate,
    EntityAlreadyExist,
    DeleteFailed,
    UserNotConnected,
    RecipeNotConnected
}