namespace agendaback.Repository.Interface
{
    public interface IUserAgendaModelValidatorRepository
    {
        Task<bool> ExclusiveEmail(string email);
        Task<bool> ExclusiveUser(string userName);
        Task<bool> UserFound(string userName);
    }
}
