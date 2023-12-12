using agendaback.Data;
using agendaback.Model.Agenda;
using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;
using agendaback.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Util
{
    public class BackgroundServices(IServiceProvider services, IHubContext<HubSocket> hubContext) : BackgroundService
    {
        private readonly IServiceProvider _services = services;
        private readonly IHubContext<HubSocket> _hubSocket = hubContext;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Adicione a lógica para consultar o banco de dados aqui
                using (var scope = _services.CreateScope())
                {
                    var dbContextAgenda = scope.ServiceProvider.GetRequiredService<ContextDBAgenda>();
                    var dbContextGLPITI = scope.ServiceProvider.GetRequiredService<ContextDBGLPITI>();
                    var dbContextGLPISistemas = scope.ServiceProvider.GetRequiredService<ContextDBGLPISistemas>();

                    List<TicketAgenda> ticketsAgenda = await dbContextAgenda.TicketAgenda.ToListAsync(cancellationToken: stoppingToken);
                    List<TicketGLPITI> ticketsGLPITI = await dbContextGLPITI.TicketGLPITI.ToListAsync(cancellationToken: stoppingToken);
                    List<TicketGLPISistemas> ticketsGLPISistemas = await dbContextGLPISistemas.TicketGLPISistemas.ToListAsync(cancellationToken: stoppingToken);

                    foreach (var ticket in ticketsAgenda)
                    {
                        TicketGLPITI? ticTI = ticketsGLPITI.FirstOrDefault(t => ticket.GLPI == 1 && t.Id == ticket.Ticket_id && t.Status != ticket.Status);
                        TicketGLPISistemas? ticSistemas = ticketsGLPISistemas.FirstOrDefault(t => ticket.GLPI == 2 && t.Id == ticket.Ticket_id &&  t.Status != ticket.Status);
                        if (ticTI != null )
                        {
                            ticket.Status = ticTI.Status;
                            if (ticTI.Status >= 5) ticket.Editable = false;
                            dbContextAgenda.TicketAgenda.Update(ticket);
                            await dbContextAgenda.SaveChangesAsync(stoppingToken);
                            await _hubSocket.Clients.All.SendAsync("ticketagenda", "ticketagenda", cancellationToken: stoppingToken);
                        }
                        if (ticSistemas != null)
                        {
                            ticket.Status = ticSistemas.Status;
                            if(ticSistemas.Status >= 5) ticket.Editable = false;
                            dbContextAgenda.TicketAgenda.Update(ticket);
                            await dbContextAgenda.SaveChangesAsync(stoppingToken);
                            await _hubSocket.Clients.All.SendAsync("ticketagenda", "ticketagenda", cancellationToken: stoppingToken);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
