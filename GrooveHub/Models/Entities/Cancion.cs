using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Cancion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime AñoLanzamiento { get; set; } 

    public TimeOnly Duracion { get; set; }

    public int IdAlbum { get; set; }

    public virtual Album IdAlbumNavigation { get; set; } = null!;
}
