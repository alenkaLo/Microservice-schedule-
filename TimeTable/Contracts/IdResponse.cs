namespace TimeTable.Contracts
{
   // public record IdResponse(Guid Id);

    public record IdResponse(
       Guid? Id,
       bool IsSuccess = true,
       string? ErrorMessage = null)
    {
        // Фабричные методы для удобного создания ответов
        public static IdResponse Success(Guid id) => new(id);
        public static IdResponse Failure(string errorMessage) => new(
            Id: null,
            IsSuccess: false,
            ErrorMessage: errorMessage);
    }

}
