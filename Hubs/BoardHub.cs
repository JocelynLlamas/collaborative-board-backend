using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollaborativeBoardApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace CollaborativeBoardApi.Hubs
{
    public class BoardHub : Hub
    {
        // Almacenamiento en memoria para usuarios y notas
        private static List<BoardUser> ConnectedUsers = new List<BoardUser>();
        private static List<Note> BoardNotes = new List<Note>();
        private static List<BoardAction> ActionHistory = new List<BoardAction>(50); // Limitamos a las últimas 50 acciones

        // Se llama cuando un cliente se conecta
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine($"Cliente conectado: {Context.ConnectionId}");
        }

        // Se llama cuando un cliente se desconecta
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                ConnectedUsers.Remove(user);
                await Clients.Others.SendAsync("UserDisconnected", user);

                // Registrar en el historial
                AddToHistory("Disconnect", user.Username, $"{user.Username} se ha desconectado");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Método para que un usuario se una al tablero con un nombre
        public async Task JoinBoard(string username)
        {
            var user = new BoardUser
            {
                ConnectionId = Context.ConnectionId,
                Username = username,
                ConnectedAt = DateTime.Now
            };

            ConnectedUsers.Add(user);

            // Notificar a otros usuarios
            await Clients.Others.SendAsync("UserJoined", user);

            // Enviar datos actuales al nuevo usuario
            await Clients.Caller.SendAsync("CurrentBoardState", BoardNotes, ConnectedUsers, ActionHistory);

            // Registrar en el historial
            AddToHistory("Connect", username, $"{username} se ha unido al tablero");
        }

        // Método para transmitir un punto de dibujo
        public async Task DrawPoint(DrawPoint point)
        {
            point.ConnectionId = Context.ConnectionId;
            point.Timestamp = DateTime.Now;

            // Enviar a todos excepto al remitente
            await Clients.Others.SendAsync("NewDrawPoint", point);

            var user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (point.IsNewLine && user != null)
            {
                AddToHistory("Draw", user.Username, $"{user.Username} empezó un nuevo trazo");
            }
        }

        // Método para añadir una nueva nota
        public async Task AddNote(Note note)
        {
            note.Id = Guid.NewGuid().ToString();
            note.ConnectionId = Context.ConnectionId;
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;

            BoardNotes.Add(note);

            // Enviar a todos incluyendo al remitente
            await Clients.All.SendAsync("NoteAdded", note);

            var user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                AddToHistory("AddNote", user.Username, $"{user.Username} añadió una nota");
            }
        }

        // Método para actualizar una nota existente
        public async Task UpdateNote(Note note)
        {
            var existingNote = BoardNotes.FirstOrDefault(n => n.Id == note.Id);
            if (existingNote != null)
            {
                existingNote.Text = note.Text;
                existingNote.X = note.X;
                existingNote.Y = note.Y;
                existingNote.Color = note.Color;
                existingNote.UpdatedAt = DateTime.Now;

                // Enviar a todos incluyendo al remitente
                await Clients.All.SendAsync("NoteUpdated", existingNote);

                var user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    AddToHistory("UpdateNote", user.Username, $"{user.Username} actualizó una nota");
                }
            }
        }

        // Método para eliminar una nota
        public async Task DeleteNote(string noteId)
        {
            var note = BoardNotes.FirstOrDefault(n => n.Id == noteId);
            if (note != null)
            {
                BoardNotes.Remove(note);

                // Enviar a todos incluyendo al remitente
                await Clients.All.SendAsync("NoteDeleted", noteId);

                var user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    AddToHistory("DeleteNote", user.Username, $"{user.Username} eliminó una nota");
                }
            }
        }

        // Método para registrar acciones en el historial
        private void AddToHistory(string actionType, string username, string description)
        {
            var action = new BoardAction
            {
                ActionType = actionType,
                Username = username,
                Timestamp = DateTime.Now,
                Description = description
            };

            // Mantener solo las últimas 50 acciones
            if (ActionHistory.Count >= 50)
                ActionHistory.RemoveAt(0);

            ActionHistory.Add(action);

            // No necesitamos enviar esto a los clientes aquí
            // porque se enviará con el próximo estado del tablero
        }
    }
}