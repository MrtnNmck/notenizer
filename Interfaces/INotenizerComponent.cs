namespace nsInterfaces
{
    /// <summary>
    /// Interafe for gui components containing notenizer data.
    /// </summary>
    public interface INotenizerComponent
    {
        void Init();

        bool IsDeletable { get; set; }
    }
}
