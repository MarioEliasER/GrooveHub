﻿using System;
using System.Collections.Generic;

namespace GrooveHub.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int Rol { get; set; }
}
