namespace NaucnaCentrala.Interfaces.Services
{
    public interface IEmailService
    {
        bool SendEmail(string subject, string message);
    }
}
