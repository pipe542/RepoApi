using System;
using System.Collections.Generic;

namespace WebApi1.Models;

public partial class Ponentes
{
    public int IdPonentes { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Departamento { get; set; } = null!;

    public string Empresa { get; set; } = null!;
}
