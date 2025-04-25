using System;
using MessagePack;

namespace CollaborativeBoardApi.Models
{
    // Modelo para representar un punto del dibujo
    [MessagePackObject]
    public class DrawPoint
    {
        [Key("connectionId")]
        public string ConnectionId { get; set; }

        [Key("x")]
        public float X { get; set; }

        [Key("y")]
        public float Y { get; set; }

        [Key("color")]
        public string Color { get; set; }

        [Key("size")]
        public float Size { get; set; }

        [Key("isNewLine")]
        public bool IsNewLine { get; set; }

        [Key("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    // Modelo para representar una nota en el tablero
    [MessagePackObject]
    public class Note
    {
        [Key("id")]
        public string Id { get; set; }
        [Key("text")]
        public string Text { get; set; }
        [Key("color")]
        public string Color { get; set; }
        [Key("x")]
        public float X { get; set; }
        [Key("y")]
        public float Y { get; set; }
        [Key("connectionId")]
        public string ConnectionId { get; set; }
        [Key("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Key("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }


    // Modelo para representar un usuario conectado
    [MessagePackObject]
    public class BoardUser
    {
        [Key("connectionId")]
        public string ConnectionId { get; set; }
        [Key("username")]
        public string Username { get; set; }
        [Key("connectedAt")]
        public DateTime ConnectedAt { get; set; }
    }

    // Modelo para representar una acción en el historial
    public class BoardAction
    {
        [Key("actionType")]
        public string ActionType { get; set; }
        [Key("username")]
        public string Username { get; set; }
        [Key("timestamp")]
        public DateTime Timestamp { get; set; }
        [Key("description")]
        public string Description { get; set; }
    }
}
