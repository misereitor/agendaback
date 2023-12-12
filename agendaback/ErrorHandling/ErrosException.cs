namespace agendaback.ErrorHandling
{
    [Serializable]
    internal class ErrosException : Exception
    {
        public int StatusCode { get; }

        public ErrosException()
        {
        }

        public ErrosException(string? message) : base(message)
        {
        }

        public ErrosException(int statusCode, string? message) : base(message)
        {
            StatusCode = statusCode;
        }

        public ErrosException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}
