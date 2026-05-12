using System.Collections.Generic;
using MonTrack.Models;

namespace MonTrack.Exporters
{
    /// <summary>
    /// Interface for data export based on Open/Closed Principle.
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Exports transactions to a specified file path.
        /// Design by Contract (DbC) should be implemented in concrete classes.
        /// </summary>
        void Export(List<Transaction> transactions, string filePath);
    }
}
