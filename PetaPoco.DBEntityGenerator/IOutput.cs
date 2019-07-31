namespace PetaPoco.DBEntityGenerator
{
    using System;

    public interface IOutput : IDisposable
    {
        void WriteLine(string text);
    }
}
