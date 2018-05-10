using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CrowdDj.BL.PoCos
{
    /// <summary>
    /// Jede Entität muss eine Id und ein Concurrency-Feld haben
    /// </summary>
    public interface IEntityObject
    {
        /// <summary>
        /// Eindeutige Identitaet des Objektes.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Die Version dieses Datenbank-Objektes. Diese Version wird beim Erzeugen (Insert) 
        /// automatisch und mit jeder Änderung neu gesetzt. 
        /// </summary>
        byte[] RowVersion { get; set; }
    }
}
