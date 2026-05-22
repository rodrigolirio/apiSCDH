using System;

namespace SCDH.Models;

public class ContratoHabitacional
{
    public Guid Id {get; set;}
    public string NumeroContrato {get; set;} =string.Empty;
    public string CpfCliente {get; set;} = string.Empty;
    public decimal ValorImovel {get; set;}
    public string CaminhoArquivo {get; set;} =string.Empty;

}
