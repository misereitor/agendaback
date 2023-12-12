using Microsoft.AspNetCore.SignalR;

namespace agendaback.Services
{
    public class HubSocket : Hub
    {
        public async Task EnviarMensagem(string mensagem)
        {
            await Clients.All.SendAsync("ReceberMensagem", mensagem);
        }
    }
}
