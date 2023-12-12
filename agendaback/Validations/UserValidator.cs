using agendaback.Model.Agenda;
using agendaback.Repository.Interface;
using FluentValidation;

namespace agendaback.Validations
{
    public class UserValidator : AbstractValidator<UserAgenda>
    {
        private readonly IUserAgendaModelValidatorRepository _userModelValidatorRepository;
        public UserValidator(IUserAgendaModelValidatorRepository userModelValidatorRepository)
        {
            _userModelValidatorRepository = userModelValidatorRepository;

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("O campo Usuário é obrigatório.")
                .MaximumLength(30).WithMessage("O campo Usuário deve ter no máximo 30 caracteres.")
                .MustAsync(ExclusiveUser).WithMessage("Este usuário já está em uso.");
                //.MustAsync(UserFound).WithMessage("Não existe registro desse usuário no GLPI");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.")
                .MaximumLength(150).WithMessage("O campo Email deve ter no máximo 150 caracteres.")
                .EmailAddress().WithMessage("O campo Email deve ser um endereço de email válido.")
                .MustAsync(ExclusiveEmail).WithMessage("Este email já está em uso.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .MaximumLength(20).WithMessage("A senha deve ter no máximo 20 caracteres.")
                .Must(SpecialCharacters).WithMessage("A senha deve conter pelo menos um caractere especial.")
                .Must(SmallLetters).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Must(CapitalLetters).WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Must(NumericDigits).WithMessage("A senha deve conter pelo menos um dígito.");
        }
        private async Task<bool> ExclusiveEmail(string email, CancellationToken cancellationToken)
        {
            return await _userModelValidatorRepository.ExclusiveEmail(email);
        }
        private async Task<bool> ExclusiveUser(string username, CancellationToken cancellationToken)
        {
            return await _userModelValidatorRepository.ExclusiveUser(username);
        }
        //private async Task<bool> UserFound(string username, CancellationToken cancellationToken)
        //{
        //    return await _userModelValidatorRepository.UserFound(username);
        //}
        private bool SpecialCharacters(string senha)
        {
            return !string.IsNullOrEmpty(senha) && senha.Any(c => !char.IsLetterOrDigit(c));
        }

        private bool SmallLetters(string senha)
        {
            return !string.IsNullOrEmpty(senha) && senha.Any(c => char.IsUpper(c));
        }

        private bool CapitalLetters(string senha)
        {
            return !string.IsNullOrEmpty(senha) && senha.Any(c => char.IsLower(c));
        }

        private bool NumericDigits(string senha)
        {
            return !string.IsNullOrEmpty(senha) && senha.Any(c => char.IsDigit(c));
        }
    }
}
