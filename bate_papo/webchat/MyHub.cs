using Microsoft.AspNetCore.SignalR;

namespace webchat
{
    public class MyHub : Hub
    //o Hub encapsula o websocket
    {
       public async Task EnviarMensagem(string mensagem)
        {
            await Clients.All.SendAsync("ReceberMensagem", mensagem);
        }
    }
}
