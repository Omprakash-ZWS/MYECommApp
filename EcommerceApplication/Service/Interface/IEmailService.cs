using EcommerceApplication.Models.EmailModel;

namespace EcommerceApplication.Service.Interface
{
    
     public interface IEmailService
    {
        Task SendEmail(Message message);
    }
}
