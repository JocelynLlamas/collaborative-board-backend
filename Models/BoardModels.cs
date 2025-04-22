using System;

namespace CollaborativeBoardApi.Models
{
    // Modelo para representar un punto del dibujo
    public class DrawPoint
    {
        public string ConnectionId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; }
        public float Size { get; set; }
        public bool IsNewLine { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Modelo para representar una nota en el tablero
    public class Note
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string Text { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Modelo para representar un usuario conectado
    public class BoardUser
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public DateTime ConnectedAt { get; set; }
    }

    // Modelo para representar una acción en el historial
    public class BoardAction
    {
        public string ActionType { get; set; }
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}