namespace BarkAndBarker.Persistence.Models;

interface IModel
{
    public static readonly string QueryCreateTable = "SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'QueryCreateTable not set';";
    public static readonly int TableCreationOrder = 0;
}