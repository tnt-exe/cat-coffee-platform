namespace DAO.Helper
{
    public sealed record ErrorResponse(int StatusCode, string StatusPhrase, dynamic Errors, DateTime Timestamp);
}
